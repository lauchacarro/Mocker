using Microsoft.AspNetCore.Http;
using Mocker.Extensions;
using System.Threading.Tasks;

namespace Mocker.Middlewares
{
    public class DelayMiddleware
    {
        private readonly RequestDelegate _next;
        public DelayMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next(httpContext);

            int delay = 0;
            httpContext.Request.GetDelayMs(delayms =>
            {
                delay = delayms;
            });

            await Task.Delay(delay);
        }
    }
}
