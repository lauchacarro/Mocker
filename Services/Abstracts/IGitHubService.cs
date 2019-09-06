using System;
using System.Threading.Tasks;

namespace Mocker.Services.Abstracts
{
    public interface IGitHubService
    {
        Task CreateFile(string path, Guid guid, string content);
        Task<string> GetFileContent(string path, Guid guid);
    }
}
