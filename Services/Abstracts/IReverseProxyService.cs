using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Mocker.Services.Abstracts
{
    public interface IReverseProxyService
    {
        Task Process(HttpContext context);
    }
}
