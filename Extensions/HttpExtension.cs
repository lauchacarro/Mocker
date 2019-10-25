using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Mocker.Models;
using Mocker.Models.Mock;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mocker.Extensions
{
    public static class HttpExtension
    {
        public static void AddHeaders(this HttpResponse response, params KeyValue[] headers)
        {
            if (headers != null && headers.Any())
            {
                string allowHeaders = string.Empty;
                foreach (KeyValue header in headers)
                {
                    response.Headers.Add(WebUtility.UrlEncode(header.Key), WebUtility.UrlEncode(header.Value));
                    allowHeaders += header.Key.ToLower();
                    if (headers.Last() != header)
                        allowHeaders += ", ";
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
                await callback(request.Form.Files.First());

            return request;
        }

        public static async Task<HttpRequest> HaveToRunGetMockAsync(this HttpRequest request, Func<Task> callback)
        {
            string[] paths = request.Path.Value.Split('/');
            if (paths.IsHaveToRunGetMock())
                await callback();
            return request;
        }

        public static async Task<HttpRequest> HaveToRunGetRawMockAsync(this HttpRequest request, Func<Task> callback)
        {
            string[] paths = request.Path.Value.Split('/');
            if (paths.IsHaveToRunGetRawMockAsync())
                await callback();
            return request;
        }

        public static async Task<HttpRequest> HaveToFollowNextMiddleware(this HttpRequest request, Func<Task> callback)
        {
            string[] paths = request.Path.Value.Split('/');
            if (!paths.IsHaveToRunGetMock() && !paths.IsHaveToRunGetRawMockAsync())
                await callback();
            return request;
        }

        public static async Task<HttpResponse> WriteMock(this HttpResponse response, MockModel mock)
        {
            response.AddHeaders(mock.Headers);
            response.StatusCode = mock.StatusCode;
            response.ContentType = $"{mock.ContentType}; charset={mock.Charset.ToLower()}";

            string body = mock.ContentType switch
            {
                "application/json" => ((Func<string>)(() =>
                {
                    object bodyObject = JsonSerializer.Deserialize<object>(mock.Body);
                    return JsonSerializer.Serialize(bodyObject);
                }))(),

                _ => mock.Body
            };

            await response.WriteAsync(body);
            return response;
        }

        public static HttpRequest GetDelayMs(this HttpRequest request, Action<int> callback)
        {
            if (request.Query.TryGetValue("delayms", out StringValues delaymsStr) && int.TryParse(delaymsStr, out int delayms))
                callback(delayms);
            return request;
        }
    }
}
