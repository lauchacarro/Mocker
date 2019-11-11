using Microsoft.AspNetCore.Builder;
using Mocker.Web.Middlewares;

namespace Mocker.Web.Extensions.Middlewares
{
    public static class MockerMiddlewareExtension
    {
        public static IApplicationBuilder UseMocker(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MockerMiddleware>();
        }
    }
}
