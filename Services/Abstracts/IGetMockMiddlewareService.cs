using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Mocker.Services.Abstracts
{
    public interface IGetMockMiddlewareService
    {
        Task GetMock(HttpContext context);
        Task GetRawMock(HttpContext context);
    }
}
