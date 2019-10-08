using Microsoft.AspNetCore.Http;
using Mocker.Extensions;
using Mocker.Models.Mock;
using Mocker.Services.Abstracts;
using System;
using System.Threading.Tasks;

namespace Mocker.Middlewares
{
    public class GetMockMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMockService _mockService;

        public GetMockMiddleware(RequestDelegate next, IMockService mockService)
        {
            _next = next;
            _mockService = mockService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string[] paths = httpContext.Request.Path.Value.Split('/');
            paths = (await paths.PathHasGuidAsync(async () =>
            {
                Guid guid = Guid.Parse(paths[2]);
                MockModel mock = await _mockService.GetMock(guid, httpContext.Request.Method);
                mock.IsNotNull(() =>
                {
                    httpContext.Request.HasQueryValues((query) =>
                    {
                        mock.ResolveDynamicBody(query);
                    });

                    httpContext.Response.AddHeaders(mock.Headers);
                    httpContext.Response.StatusCode = mock.StatusCode;
                    httpContext.Response.ContentType = mock.ContentType;
                    httpContext.Response.WriteAsync(mock.Body);

                })
                .IsNull(() =>
                {
                    httpContext.Response.StatusCode = 404;
                });
            }));
            paths = (await paths.PathHasNoGuidAsync(async () =>
            {
                await _next(httpContext);
            }));
        }
    }
}
