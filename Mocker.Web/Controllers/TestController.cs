using Microsoft.AspNetCore.Mvc;
using Mocker.Web.Models.Test;
namespace Mocker.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpPost("[action]")]
        public IActionResult Form([FromForm]TestRequest request)
        {
            return Ok(request);
        }

        [HttpPost("[action]")]
        public IActionResult Raw(TestRequest request)
        {
            return Ok(request);
        }

        [HttpPost("[action]")]
        public IActionResult RawText([FromBody]string request)
        {
            return Ok(request);
        }
    }
}