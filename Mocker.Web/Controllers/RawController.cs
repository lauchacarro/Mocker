using Microsoft.AspNetCore.Mvc;
using Mocker.Web.Extensions;
using Mocker.Web.Models.File;
using Mocker.Web.Services.Abstracts;
using System;
using System.Threading.Tasks;

namespace Mocker.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RawController : ControllerBase
    {
        private readonly IFileService _fileService;

        public RawController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("files/{guid}")]
        public async Task<IActionResult> RawFile(Guid guid)
        {
            IActionResult result = NotFound();
            FileModel file = await _fileService.GetFile(guid);
            file.IsNotNull(() =>
            {
                result = new ContentResult
                {
                    ContentType = "text/plain",
                    StatusCode = 200,
                    Content = file.Base64
                };
            });
            return result;
        }
    }
}