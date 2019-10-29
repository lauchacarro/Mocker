using Microsoft.AspNetCore.Mvc;
using Mocker.Extensions.Validations;
using Mocker.Models;
using Mocker.Models.Mock;
using Mocker.Services.Abstracts;
using System.Threading.Tasks;

namespace Mocker.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMockService _mockService;

        public HomeController(IMockService mockService)
        {
            _mockService = mockService;
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

    }
}