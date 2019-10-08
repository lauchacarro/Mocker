using Microsoft.AspNetCore.Mvc;
using Mocker.Attributes;
using Mocker.ContentTypeState;
using Mocker.Extensions;
using Mocker.Models.File;
using Mocker.Models.Mock;
using Mocker.Services.Abstracts;
using System;
using System.Threading.Tasks;

namespace Mocker.Controllers
{
    [Route("raw")]
    [ApiController]
    public class RawController : ControllerBase
    {
        private readonly IMockService _mockService;
        private readonly IFileService _fileService;
        private readonly IContentTypeService _contentTypeService;

        public RawController(IMockService mockService, IFileService fileService, IContentTypeService contentTypeService)
        {
            _mockService = mockService;
            _fileService = fileService;
            _contentTypeService = contentTypeService;
        }
        [HttpAll("{guid}")]
        public async Task<IActionResult> RawMock(Guid guid)
        {
            IActionResult result = NotFound();
            MockModel mock = await _mockService.GetMock(guid, Request.Method);
            mock.IsNotNull(() =>
            {
                mock.ContentType = "text/plain";
                IContentTypeMockState state = _contentTypeService.GetState(mock.ContentType);
                result = state.CreateObjectResult(mock);
            });
            return result;
        }

        [HttpAll("files/{guid}")]
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