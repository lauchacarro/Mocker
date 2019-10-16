using Microsoft.AspNetCore.Mvc;
using Mocker.Extensions;
using Mocker.Models.File;
using Mocker.Services.Abstracts;
using System;
using System.Threading.Tasks;
using Mocker.Models.Test;
namespace Mocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpPost("[action]")]
        public IActionResult FormData([FromForm]TestRequest request)
        {
            return Ok(request);
        }
    }
}