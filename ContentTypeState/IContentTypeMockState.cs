﻿using Microsoft.AspNetCore.Mvc;
using Mocker.Models;

namespace Mocker.ContentTypeState
{
    public interface IContentTypeMockState
    {
        IActionResult CreateObjectResult(MockModel request);
    }
}
