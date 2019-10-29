using Mocker.Models.Postman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mocker.Extensions
{
    public static class PostmanExtension
    {
        public static PostmanRequest IsUrlInvalid(this PostmanRequest request, Action callback)
        {
            if (Uri.TryCreate(request.Url, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                return request;

            callback();
            return request;
        }

        public static PostmanRequest IsMethodInvalid(this PostmanRequest request, Action callback)
        {
            if (string.IsNullOrWhiteSpace(request.HttpMethod))
                callback();
            return request;
        }
    }
}
