using Microsoft.Extensions.Options;
using Mocker.Extensions;
using Mocker.Models.File;
using Mocker.Models.Settings;
using Mocker.Services.Abstracts;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mocker.Services.Concretes
{
    public class FileService : IFileService
    {
        private readonly IGitHubService _githubService;
        private readonly GitHubSetting _githubSetting;
        public FileService(IGitHubService githubService, IOptions<GitHubSetting> githubSetting)
        {
            _githubService = githubService;
            _githubSetting = githubSetting.Value;

        }

        public async Task<Guid> Create(FileModel file)
        {
            Guid guid = Guid.NewGuid();

            await _githubService.CreateFile(_githubSetting.FilePath, guid, JsonSerializer.Serialize(file));

            return guid;
        }

        public async Task<FileModel> GetFile(Guid guid)
        {
            FileModel file = null;
            string jsonFile = await _githubService.GetFileContent(_githubSetting.FilePath, guid);

            jsonFile.IsNotNull(() =>
            {
                file = JsonSerializer.Deserialize<FileModel>(jsonFile);
            });

            return file;
        }
    }
}
