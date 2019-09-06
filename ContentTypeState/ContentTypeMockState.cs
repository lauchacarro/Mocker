using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Mocker.Models;

namespace Mocker.ContentTypeState
{
    public class ContentTypeMockState : IContentTypeMockState
    {
        public virtual ObjectResult CreateObjectResult(MockModel request)
        {
            ObjectResult objectResult = new ObjectResult(request.Body)
            {
                StatusCode = request.StatusCode
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
