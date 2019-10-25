using Microsoft.AspNetCore.Mvc;
using Mocker.Extensions;
using Mocker.Models;
using Mocker.Models.Mock;
using Mocker.Models.Postman;
using Mocker.Services.Abstracts;
using System.Threading.Tasks;

namespace Mocker.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMockService _mockService;
        private readonly IPostmanService _postmanService;

        public HomeController(IMockService mockService, IPostmanService postmanService)
        {
            _mockService = mockService;
            _postmanService = postmanService;
        }

        [HttpGet]
        [HttpGet("api")]
        public IActionResult Index()
        {
            return Redirect("https://mocker.cloud");
        }

        [HttpPost("api/[action]")]
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

        [HttpPost("api/[action]")]
        public async Task<IActionResult> Postman(PostmanRequest request)
        {
            ObjectResult statusCodeResult = new ObjectResult(null);

            ValidateResult validateResult = _postmanService.Validate(request);

            (await validateResult.Success(async () =>
            {
                PostmanResponse response = await _postmanService.SendRequest(request);
                statusCodeResult = Ok(response);
            }))
            .Error((errorMessages) =>
            {
                statusCodeResult = BadRequest(errorMessages);
            });

            return statusCodeResult;
        }
    }
}