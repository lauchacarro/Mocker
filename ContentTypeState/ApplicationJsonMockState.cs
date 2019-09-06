using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Mocker.Models.Requests;

namespace Mocker.ContentTypeState
{
    public class ApplicationJsonMockState : IContentTypeMockState
    {
        public string CreateMockContent(CreateMockRequest request)
        {
            throw new NotImplementedException();
        }

        public ObjectResult CreateObjectResult(CreateMockRequest request)
        {
            ObjectResult objectResult = new ObjectResult(Newtonsoft.Json.JsonConvert.DeserializeObject<object>(request.Body))
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
