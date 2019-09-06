using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mocker.Attributes;
using Mocker.ContentTypeState;
using Mocker.Models;
using Mocker.Services.Abstracts;

namespace Mocker.Controllers
{
    [Route("api/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMockService _mockService;
        private readonly IContentTypeService _contentTypeService;

        public HomeController(IMockService mockService, IContentTypeService contentTypeService)
        {
            _mockService = mockService;
            _contentTypeService = contentTypeService;
        }

        [HttpAll("{guid}")]
        public async Task<IActionResult> Index(Guid guid)
        {
            var mock = await _mockService.GetMock(guid, Request.Method);
            IContentTypeMockState state = _contentTypeService.GetState(mock.Result.ContentType);
            return state.CreateObjectResult(mock.Result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(MockModel request)
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

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateMany(MockModelEnumerable request)
        {
            var validateResult = await _mockService.Validate(request.Mocks.ToArray());
            if (validateResult.Success)
            {
                OperationResult<Guid> operationResult = await _mockService.Create(request.Mocks.ToArray());
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