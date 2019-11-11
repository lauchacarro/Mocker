using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Mocker.Web.Services.Abstracts
{
    public interface IGetMockMiddlewareService
    {
        Task GetMock(HttpContext context);
        Task GetRawMock(HttpContext context);
    }
}
