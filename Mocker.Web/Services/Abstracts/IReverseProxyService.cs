using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Mocker.Web.Services.Abstracts
{
    public interface IReverseProxyService
    {
        Task Process(HttpContext context);
    }
}
