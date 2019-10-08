using Microsoft.AspNetCore.Http;
using Mocker.Models.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mocker.Extensions
{
    public static class HttpExtension
    {
        public static void AddHeaders(this HttpResponse response, params MockHeader[] headers)
        {
            if (headers != null && headers.Any())
            {
                foreach (MockHeader header in headers)
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
    }
}
