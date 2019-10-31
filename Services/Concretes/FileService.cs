using Microsoft.Extensions.Options;
using Mocker.Enums;
using Mocker.Extensions;
using Mocker.Extensions.Validations;
using Mocker.Models;
using Mocker.Models.File;
using Mocker.Models.Settings;
using Mocker.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            string content = JsonSerializer.Serialize(file);
            string sha = await _githubService.CreateBlob(content);
            await _githubService.CreateFile(_githubSetting.FilePath, guid, sha);

            return guid;
        }

        public async Task<FileModel> GetFile(Guid guid)
        {
            FileModel file = null;
            string sha = await _githubService.GetFileContent(_githubSetting.FilePath, guid);
            await sha.IsNotNullAsync(async () =>
            {
                string base64File = await _githubService.GetBlobContent(sha);

                base64File.IsNotNull(() =>
                {
                    string jsonFile = Encoding.UTF8.GetString(Convert.FromBase64String(base64File));
                    jsonFile.IsNotNull(() =>
                    {
                        file = JsonSerializer.Deserialize<FileModel>(jsonFile);
                    });
                });
            });


            return file;
        }

        public ValidateResult Validate(FileModel request)
        {
            ValidateResult validateResult = new ValidateResult();
            List<ErrorMessageCodeEnum> errorMessages = new List<ErrorMessageCodeEnum>();

            request.Lenght.IsGreaterThan100MB(() =>
            {
                errorMessages.Add(ErrorMessageCodeEnum.FileGreaterThan100MB);
            });

            return new ValidateResult()
            {
                Success = !errorMessages.Any(),
                ErrorMessages = errorMessages.ToArray()
            };
        }
    }
}
