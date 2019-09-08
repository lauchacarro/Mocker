using Microsoft.AspNetCore.Mvc;
using Mocker.Attributes;
using Mocker.ContentTypeState;
using Mocker.Extensions;
using Mocker.Models;
using Mocker.Services.Abstracts;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            MockModel mock = await _mockService.GetMock(guid, Request.Method);
            IContentTypeMockState state = _contentTypeService.GetState(mock.ContentType);

            Request.HasQueryValues((query) =>
            {
                mock.ResolveDynamicBody(query);
            });

            Response.AddHeaders(mock.Headers);

            return state.CreateObjectResult(mock);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(MockModelEnumerable request)
        {
            ObjectResult statusCodeResult = new ObjectResult(null);

            ValidateResult validateResult = _mockService.Validate(request.Mocks);

            (await validateResult.Success(async () =>
            {
                Guid guid = await _mockService.Create(request.Mocks);
                statusCodeResult = Created("/api/" + guid, guid);
            }))
            .Error((errorMessages) =>
            {
                statusCodeResult = BadRequest(errorMessages);
            });

            return statusCodeResult;


        }
    }
}