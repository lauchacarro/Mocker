using Microsoft.AspNetCore.Http;
using Mocker.Web.Extensions.Validations;
using System;
using System.Threading.Tasks;

namespace Mocker.Web.Extensions.Middlewares
{
    public static class RouteExtension
    {
        public static async Task<HttpRequest> RouteGetRawMockAsync(this HttpRequest request, Func<Task> callback)
        {
            string[] paths = request.Path.Value.Split('/');

            if (paths.Length >= 4 && paths[1].ToLower() == "api" && paths[2].ToLower() == "raw" && Guid.TryParse(paths[3], out _))
                await callback();
            return request;
        }
        public static async Task<HttpRequest> RouteGetMockAsync(this HttpRequest request, Func<Task> callback)
        {
            string[] paths = request.Path.Value.Split('/');

            if (paths.Length >= 3 && paths[1].ToLower() == "api" && Guid.TryParse(paths[2], out _))
                await callback();
            return request;
        }
        public static async Task<HttpRequest> RouteReverseProxyAsync(this HttpRequest request, Func<Task> callback)
        {
            string[] paths = request.Path.Value.Split('/');
            if (paths.Length >= 3 && paths[1].ToLower() == "api" && paths[2].ToLower() == "postman" && request.HasUrlValid())
                await callback();
            return request;
        }
    }
}
