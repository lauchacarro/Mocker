using Microsoft.AspNetCore.Mvc;
using Mocker.Models;

namespace Mocker.ContentTypeState
{
    public interface IContentTypeMockState
    {
        ObjectResult CreateObjectResult(MockModel request);
    }
}
