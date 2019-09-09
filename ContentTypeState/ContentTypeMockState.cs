using Microsoft.AspNetCore.Mvc;
using Mocker.Models.Mock;

namespace Mocker.ContentTypeState
{
    public class ContentTypeMockState : IContentTypeMockState
    {
        public virtual IActionResult CreateObjectResult(MockModel request)
        {
            return new ContentResult
            {
                ContentType = request.ContentType,
                StatusCode = request.StatusCode,
                Content = request.Body
            };
        }
    }
}
