using Microsoft.AspNetCore.Builder;
using Mocker.Middlewares;

namespace Mocker.Extensions.Middlewares
{
    public static class MockerMiddlewareExtension
    {
        public static IApplicationBuilder UseMocker(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MockerMiddleware>();
        }
    }
}
