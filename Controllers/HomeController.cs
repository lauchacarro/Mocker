using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Mocker.Attributes;
using Mocker.Models;
using Mocker.Models.Requests;
using Mocker.Services.Abstracts;
using System;
using System.Threading.Tasks;

namespace Mocker.Controllers
{
    [Route("api/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMockService _mockService;

        public HomeController(IMockService mockService)
        {
            _mockService = mockService;
        }

        [HttpAll("{guid}")]
        public async Task<IActionResult> Index(Guid guid)
        {
            var mock = await _mockService.GetMock(guid);
            ObjectResult objectResult = new ObjectResult(Newtonsoft.Json.JsonConvert.DeserializeObject<object>(mock.Result.Body));
            objectResult.StatusCode = mock.Result.StatusCode;
            MediaTypeCollection mediaType = new MediaTypeCollection();
            mediaType.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(mock.Result.ContentType));
            objectResult.ContentTypes = mediaType;
            return objectResult;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateMockRequest request)
        {
            var validateResult = await _mockService.Validate(request);
            if (validateResult.Success)
            {
                OperationResult<Guid> operationResult = await _mockService.Create(request);
                if (operationResult.Success)
                {
                    return Created("/api/" + operationResult.Result, operationResult.Result);
                }
                else
                {
                    return BadRequest(operationResult.ErrorMessages);
                }
            }
            else
            {
                return BadRequest(validateResult.ErrorMessages);
            }

        }


    }
}