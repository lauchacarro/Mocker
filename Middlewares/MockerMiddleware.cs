﻿using Microsoft.AspNetCore.Http;
using Mocker.Extensions;
using Mocker.Services.Abstracts;
using System.Threading.Tasks;

namespace Mocker.Middlewares
{
    public class MockerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IGetMockMiddlewareService _mockService;
        private readonly IReverseProxyService _reverseProxyService;

        public MockerMiddleware(RequestDelegate next, IGetMockMiddlewareService mockService, IReverseProxyService reverseProxyService)
        {
            _next = next;
            _mockService = mockService;
            _reverseProxyService = reverseProxyService;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            await httpContext.Request.HaveToRunGetMockAsync(async () =>
            {
                await _mockService.GetMock(httpContext);
            });

            await httpContext.Request.HaveToRunGetRawMockAsync(async () =>
            {
                await _mockService.GetRawMock(httpContext);
            });

            await httpContext.Request.HaveToRunReverseProxy(async () =>
            {
                await _reverseProxyService.Process(httpContext);
            });

            await httpContext.Request.HaveToFollowNextMiddleware(async () =>
            {
                await _next(httpContext);
            });
        }
    }
}
