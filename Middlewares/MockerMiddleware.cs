using Microsoft.AspNetCore.Http;
using Mocker.Extensions.Middlewares;
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
            bool followNextMiddleware = true;

            await httpContext.Request.RouteGetMockAsync(async () =>
            {
                followNextMiddleware = false;
                await _mockService.GetMock(httpContext);
            });

            await httpContext.Request.RouteGetRawMockAsync(async () =>
            {
                followNextMiddleware = false;
                await _mockService.GetRawMock(httpContext);
            });

            await httpContext.Request.RouteReverseProxyAsync(async () =>
            {
                followNextMiddleware = false;
                await _reverseProxyService.Process(httpContext);
            });

            if (followNextMiddleware) await _next(httpContext);
        }
    }
}
