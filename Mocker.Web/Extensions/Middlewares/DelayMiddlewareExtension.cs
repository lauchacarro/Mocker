using Microsoft.AspNetCore.Builder;
using Mocker.Web.Middlewares;

namespace Mocker.Web.Extensions.Middlewares
{
    public static class DelayMiddlewareExtension
    {
        public static IApplicationBuilder UseDelay(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DelayMiddleware>();
        }
    }
}
