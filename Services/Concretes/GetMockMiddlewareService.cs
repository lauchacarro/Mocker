using Microsoft.AspNetCore.Http;
using Mocker.Extensions;
using Mocker.Models.Mock;
using Mocker.Services.Abstracts;
using System;
using System.Threading.Tasks;

namespace Mocker.Services.Concretes
{
    public class GetMockMiddlewareService : IGetMockMiddlewareService
    {
        private readonly IMockService _mockService;

        public GetMockMiddlewareService(IMockService mockService)
        {
            _mockService = mockService;
        }

        public async Task GetMock(HttpContext context)
        {
            string[] paths = context.Request.Path.Value.Split('/');
            Guid guid = Guid.Parse(paths[2]);
            MockModel mock = await _mockService.GetMock(guid, context.Request.Method);
            (await mock.IsNotNullAsync(async () =>
            {
                context.Request.HasQueryValues((query) =>
                {
                    mock.ResolveDynamicBody(query);
                });

                await context.Response.WriteMock(mock);
            }))
            .IsNull(() =>
            {
                context.Response.StatusCode = 404;
            });
        }
        public async Task GetRawMock(HttpContext context)
        {
            string[] paths = context.Request.Path.Value.Split('/');
            Guid guid = Guid.Parse(paths[3]);
            MockModel mock = await _mockService.GetMock(guid, context.Request.Method);
            await mock.IsNotNullAsync(async () =>
            {
                mock.ContentType = "text/plain";
                await context.Response.WriteMock(mock);
            });
        }
    }
}