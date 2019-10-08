using Microsoft.AspNetCore.Http;
using Mocker.Extensions;
using Mocker.Services.Abstracts;
using System.Threading.Tasks;

namespace Mocker.Middlewares
{
    public class GetMockMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IGetMockMiddlewareService _mockService;

        public GetMockMiddleware(RequestDelegate next, IGetMockMiddlewareService mockService)
        {
            _next = next;
            _mockService = mockService;
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

            await httpContext.Request.HaveToFollowNextMiddleware(async () =>
            {
                await _next(httpContext);
            });
        }
    }
}
