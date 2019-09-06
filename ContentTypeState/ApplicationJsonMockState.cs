using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Mocker.Models;

namespace Mocker.ContentTypeState
{
    public class ApplicationJsonMockState : ContentTypeMockState
    {
        public override IActionResult CreateObjectResult(MockModel request)
        {
            ObjectResult objectResult = (ObjectResult)base.CreateObjectResult(request);
            objectResult.Value = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(request.Body);
            return objectResult;
        }
    }
}
