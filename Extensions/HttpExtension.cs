using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mocker.Extensions
{
    public static class HttpExtension
    {
        public static void AddHeaders(this HttpResponse response, params KeyValuePair<string, string>[] headers)
        {
            if (headers != null && headers.Any())
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    response.Headers.Add(header.Key, header.Value);
                }
            }
        }

        public static HttpRequest HasQueryValues(this HttpRequest request, Action<IQueryCollection> callback)
        {
            if (request.Query.Count > 0)
                callback(request.Query);

            return request;
        }
        public static async Task<HttpRequest> HasFiles(this HttpRequest request, Func<IFormFile, Task> callback)
        {
            if (request.Form.Files.Count > 0)
                await callback(request.Form.Files[0]);

            return request;
        }
    }
}
