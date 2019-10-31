using Microsoft.AspNetCore.Mvc;
using Mocker.Extensions;
using Mocker.Extensions.Validations;
using Mocker.Models;
using Mocker.Models.File;
using Mocker.Services.Abstracts;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> Index(Guid guid)
        {
            IActionResult statusCodeResult = NotFound();
            FileModel file = await _fileService.GetFile(guid);

            file.IsNotNull(() =>
            {
                byte[] bytes = Convert.FromBase64String(file.Base64);

                statusCodeResult = File(bytes, file.ContentType, file.Name);
            });

            return statusCodeResult;
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            IActionResult statusCodeResult = BadRequest();

            await Request.HasFiles(async (file) =>
            {
                FileModel fileModel = await file.ToFileModelAsync();

                ValidateResult validateResult = _fileService.Validate(fileModel);

                (await validateResult.Success(async () =>
                {
                    GuidResponse guidResponse = new GuidResponse()
                    {
                        Guid = await _fileService.Create(fileModel)
                    };
                    statusCodeResult = Created("/api/" + guidResponse.Guid, guidResponse);
                }))
                .Error((errorMessages) =>
                {
                    statusCodeResult = BadRequest(errorMessages);
                });

            });

            return statusCodeResult;
        }
    }
}