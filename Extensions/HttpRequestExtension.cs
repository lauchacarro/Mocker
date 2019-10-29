using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mocker.Extensions
{
    public static class HttpRequestExtension
    {
        public static HttpRequest HasQueryValues(this HttpRequest request, Action<IQueryCollection> callback)
        {
            if (request.Query.Count > 0)
                callback(request.Query);

            return request;
        }
        public static async Task<HttpRequest> HasFiles(this HttpRequest request, Func<IFormFile, Task> callback)
        {
            if (request.Form.Files.Count > 0)
                await callback(request.Form.Files.First());

            return request;
        }
        public static HttpRequest GetDelayMs(this HttpRequest request, Action<int> callback)
        {
            if (request.Query.TryGetValue("delayms", out StringValues delaymsStr) && int.TryParse(delaymsStr, out int delayms))
                callback(delayms);
            return request;
        }
    }
}
