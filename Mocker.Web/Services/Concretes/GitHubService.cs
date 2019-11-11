using Microsoft.Extensions.Options;
using Mocker.Web.Models.Settings;
using Mocker.Web.Services.Abstracts;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mocker.Web.Services.Concretes
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubSetting _githubSetting;

        public GitHubService(IOptions<GitHubSetting> githubSetting)
        {
            _githubSetting = githubSetting.Value;
            _client = new GitHubClient(new ProductHeaderValue(nameof(Mocker)));
            Credentials basicAuth = new Credentials(_githubSetting.User, Environment.GetEnvironmentVariable("GITHUB_PASSWORD") ?? _githubSetting.Pass);


            _client.Credentials = basicAuth;
            _githubSetting = githubSetting.Value;
        }

        public async Task<string> CreateBlob(string content)
        {
            BlobReference blobReference = await _client.Git.Blob.Create(
                _githubSetting.RepositoryID,
                new NewBlob()
                {
                    Content = content,
                    Encoding = EncodingType.Utf8
                }
                );

            return blobReference.Sha;
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

        public async Task<string> GetBlobContent(string sha)
        {
            try
            {
                Blob blob = await _client.Git.Blob.Get(_githubSetting.RepositoryID, sha);
                return blob.Content;
            }
            catch (Octokit.NotFoundException)
            {
                return null;
            }
        }

        public async Task<string> GetFileContent(string path, Guid guid)
        {
            try
            {
                IReadOnlyList<RepositoryContent> contents = await _client.Repository.Content.GetAllContents(_githubSetting.RepositoryID, Path.Combine(path, guid.ToString()));
                return contents.First().Content;
            }
            catch (Octokit.NotFoundException)
            {
                return null;
            }
        }
    }
}
