using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mocker.Models.File;
using Mocker.Models.Settings;
using Mocker.Services.Abstracts;
using Newtonsoft.Json;

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

            await _githubService.CreateFile(_githubSetting.FilePath, guid, JsonConvert.SerializeObject(file));

            return guid;
        }

        public async Task<FileModel> GetFile(Guid guid)
        {
            string jsonFileModel = await _githubService.GetFileContent(_githubSetting.FilePath, guid);

            return JsonConvert.DeserializeObject<FileModel>(jsonFileModel);
        }
    }
}
