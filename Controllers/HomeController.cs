using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mocker.Attributes;
using Mocker.ContentTypeState;
using Mocker.Extensions;
using Mocker.Models;
using Mocker.Models.Mock;
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
            IActionResult result = null;
            MockModel mock = await _mockService.GetMock(guid, Request.Method);
            mock.IsNotNull(() =>
            {
                IContentTypeMockState state = _contentTypeService.GetState(mock.ContentType);

                Request.HasQueryValues((query) =>
                {
                    mock.ResolveDynamicBody(query);
                });

                Response.AddHeaders(mock.Headers);

                result = state.CreateObjectResult(mock);
            })
            .IsNull(() =>
            {
                result = NotFound();
            });

            return result;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(MockModelEnumerable request)
        {
            ObjectResult statusCodeResult = new ObjectResult(null);

            ValidateResult validateResult = _mockService.Validate(request.Mocks);

            (await validateResult.Success(async () =>
            {
                GuidResponse guidResponse = new GuidResponse()
                {
                    Guid = await _mockService.Create(request.Mocks)
                };
                statusCodeResult = Created("/api/" + guidResponse.Guid, guidResponse);
            }))
            .Error((errorMessages) =>
            {
                statusCodeResult = BadRequest(errorMessages);
            });

            return statusCodeResult;


        }
    }
}