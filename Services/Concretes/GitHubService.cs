using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mocker.Models.Settings;
using Mocker.Services.Abstracts;
using Octokit;

namespace Mocker.Services.Concretes
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubSetting _githubSetting;

        public GitHubService(IOptions<GitHubSetting> githubSetting)
        {
            _githubSetting = githubSetting.Value;
            _client = new GitHubClient(new ProductHeaderValue(nameof(Mocker)));
            Credentials basicAuth = new Credentials(_githubSetting.User, Environment.GetEnvironmentVariable("GITHUB_PASSWORD"));
            _client.Credentials = basicAuth;
            _githubSetting = githubSetting.Value;
        }

        public async Task CreateFile(string path, Guid guid, string content)
        {

            await _client.Repository.Content.CreateFile(
                                _githubSetting.RepositoryID,
                                Path.Combine(path, guid.ToString()),
                                new CreateFileRequest($"Create Mock",
                                                      content,
                                                      _githubSetting.Branch));

        }

        public async Task<string> GetFileContent(string path, Guid guid)
        {
            IReadOnlyList<RepositoryContent> contents = await _client.Repository.Content.GetAllContents(_githubSetting.RepositoryID, Path.Combine(path, guid.ToString()));
            return contents[0].Content;
        }
    }
}
