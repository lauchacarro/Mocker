using Microsoft.Extensions.Configuration;
using Mocker.Services.Abstracts;
using Octokit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mocker.Services.Concretes
{
    public class GitHubService : IGitHubService
    {
        private readonly IConfiguration _configuration;
        private readonly GitHubClient _client;

        public GitHubService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new GitHubClient(new ProductHeaderValue("Mocker"));
            var basicAuth = new Credentials(_configuration["GitHub:User"], _configuration["GitHub:Pass"]);
            _client.Credentials = basicAuth;
        }

        public async Task CreateFile(string path, Guid guid, string content)
        {

            await _client.Repository.Content.CreateFile(
                                long.Parse(_configuration["GitHub:RepositoryID"]),
                                Path.Combine(path, guid.ToString()),
                                new CreateFileRequest("File creation",
                                                      content,
                                                      _configuration["GitHub:Branch"]));

        }

        public Task<string> GetFileContent(string path, Guid guid, string content)
        {
            throw new NotImplementedException();
        }
    }
}
