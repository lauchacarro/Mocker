using Microsoft.AspNetCore.Mvc;
using Mocker.Models.Mock;

namespace Mocker.ContentTypeState
{
    public interface IContentTypeMockState
    {
        IActionResult CreateObjectResult(MockModel request);
    }
}
