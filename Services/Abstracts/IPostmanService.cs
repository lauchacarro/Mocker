using Mocker.Models;
using Mocker.Models.Postman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mocker.Services.Abstracts
{
    public interface IPostmanService
    {
        ValidateResult Validate(PostmanRequest request);
        Task<PostmanResponse> SendRequest(PostmanRequest request);
    }
}
