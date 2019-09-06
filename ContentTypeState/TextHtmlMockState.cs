using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mocker.Models;

namespace Mocker.ContentTypeState
{
    public class TextHtmlMockState : ContentTypeMockState
    {
        public override IActionResult CreateObjectResult(MockModel request)
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
