using Microsoft.AspNetCore.Builder;
using Mocker.Middlewares;

namespace Mocker.Extensions
{
    public static class DelayMiddlewareExtension
    {
        public static IApplicationBuilder UseDelay(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DelayMiddleware>();
        }
    }
}
