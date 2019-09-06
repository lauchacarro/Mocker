using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mocker.Models.Requests;

namespace Mocker.ContentTypeState
{
    public interface IContentTypeMockState
    {
        ObjectResult CreateObjectResult(CreateMockRequest request);
        string CreateMockContent(CreateMockRequest request);
    }
}
