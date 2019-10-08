using Microsoft.AspNetCore.Builder;
using Mocker.Middlewares;

namespace Mocker.Extensions
{
    public static class GetMockMiddlewareExtension
    {
        public static IApplicationBuilder UseGetMock(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GetMockMiddleware>();
        }
    }
}
