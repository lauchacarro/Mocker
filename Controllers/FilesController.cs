using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mocker.Extensions;
using Mocker.Models.File;
using Mocker.Services.Abstracts;

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
            FileModel file = await _fileService.GetFile(guid);
            byte[] bytes = Convert.FromBase64String(file.Base64);

            return File(bytes, file.ContentType, file.Name);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            IActionResult statusCodeResult = BadRequest();

            await Request.HasFiles(async (file) =>
            {
                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                    byte[] bytes = stream.ConvertToBlob();

                    FileModel fileModel = new FileModel
                    {
                        Name = file.FileName,
                        Base64 = stream.ToBase64String(),
                        ContentType = file.ContentType
                    };
                    Guid guid = await _fileService.Create(fileModel);
                    statusCodeResult = Created("/api/" + guid, guid);
                }
            });

            return statusCodeResult;
        }
    }
}