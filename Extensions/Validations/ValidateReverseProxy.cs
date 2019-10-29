using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;

namespace Mocker.Extensions.Validations
{
    public static class ValidateReverseProxy
    {
        public static bool HasUrlValid(this HttpRequest request)
        {
            return request.Headers.TryGetValue("Mocker-Url", out StringValues values) && values[0].IsUrlValid();

        }
        public static HttpRequest GetUrlFromHeader(this HttpRequest request, Action<Uri> callback)
        {
            if (request.HasUrlValid())
                callback(new Uri(request.Headers["Mocker-Url"][0]));

            return request;
        }
    }
}
