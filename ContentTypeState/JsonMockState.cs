using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Mocker.Models.Mock;

namespace Mocker.ContentTypeState
{
    public class JsonMockState : ContentTypeMockState
    {
        public override IActionResult CreateObjectResult(MockModel request)
        {
            ObjectResult objectResult = new ObjectResult(request.Body)
            {
                StatusCode = request.StatusCode,
                Value = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(request.Body)
            };

            MediaTypeCollection mediaType = new MediaTypeCollection
            {
                new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(request.ContentType)
            };
            objectResult.ContentTypes = mediaType;

            return objectResult;
        }
    }
}
