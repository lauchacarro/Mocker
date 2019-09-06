using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Mocker.Models;

namespace Mocker.ContentTypeState
{
    public class ApplicationJsonMockState : ContentTypeMockState
    {
        public override ObjectResult CreateObjectResult(MockModel request)
        {
            ObjectResult objectResult = base.CreateObjectResult(request);
            objectResult.Value = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(request.Body);
            return objectResult;
        }
    }
}
