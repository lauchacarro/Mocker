using Microsoft.Extensions.Options;
using Mocker.Enums;
using Mocker.Extensions;
using Mocker.Models;
using Mocker.Models.Mock;
using Mocker.Models.Settings;
using Mocker.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mocker.Services.Concretes
{
    public class MockService : IMockService
    {
        private readonly IGitHubService _githubService;
        private readonly GitHubSetting _githubSetting;
        private readonly string _guidMethodsFolderPath;

        public MockService(IGitHubService githubService, IOptions<GitHubSetting> githubSetting)
        {
            _githubService = githubService;
            _githubSetting = githubSetting.Value;
            _guidMethodsFolderPath = Path.Combine(_githubSetting.HttpMethodsFolderPath, "guidmethods");
        }

        public async Task<Guid> Create(IEnumerable<MockModel> request)
        {
            Guid guid = Guid.NewGuid();

            GuidMethodsModel guidMethodsModel = new GuidMethodsModel
            {
                HttpMethods = request.Select(x => x.HttpMethod.ToUpper()).ToArray()
            };

            await _githubService.CreateFile(_guidMethodsFolderPath, guid, JsonSerializer.Serialize(guidMethodsModel));

            foreach (MockModel mock in request)
            {
                string content = JsonSerializer.Serialize(mock);
                string path = Path.Combine(_githubSetting.HttpMethodsFolderPath, mock.HttpMethod.ToUpper());
                await _githubService.CreateFile(path, guid, content);
            }

            return guid;
        }

        public async Task<GuidMethodsModel> GetHttpMethodsByGuid(Guid guid)
        {
            GuidMethodsModel guidMethods = null;
            string jsonGuidMethods = await _githubService.GetFileContent(_guidMethodsFolderPath, guid);

            jsonGuidMethods.IsNotNull(() =>
            {
                guidMethods = JsonSerializer.Deserialize<GuidMethodsModel>(jsonGuidMethods);
            });

            return guidMethods;
        }

        public async Task<MockModel> GetMock(Guid guid, string httpMethod)
        {
            MockModel mock = null;
            GuidMethodsModel guidMethods = await GetHttpMethodsByGuid(guid);

            await guidMethods.IsNotNullAsync(async () =>
            {
                httpMethod = guidMethods.HttpMethods.Contains(httpMethod) ? httpMethod : "GET";

                string path = Path.Combine(_githubSetting.HttpMethodsFolderPath, httpMethod.ToUpper());

                string content = await _githubService.GetFileContent(path, guid);
                mock = JsonSerializer.Deserialize<MockModel>(content);
            });

            return mock;
        }

        public ValidateResult Validate(IEnumerable<MockModel> request)
        {
            ValidateResult validateResult = new ValidateResult();
            List<ErrorMessageCodeEnum> errorMessages = new List<ErrorMessageCodeEnum>();


            request.ValidateDuplicateMethod(() =>
            {
                errorMessages.Add(ErrorMessageCodeEnum.HttpMethodDuplicate);
            });


            foreach (MockModel mock in request)
            {
                mock.IsStatusCodeSmallerThanOne(() =>
                {
                    errorMessages.Add(ErrorMessageCodeEnum.StatusCodeSmallerThanOne);
                })
                .IsContentTypeInvalid(() =>
                {
                    errorMessages.Add(ErrorMessageCodeEnum.InvalidContentType);
                })
                .IsCharsetInvalid(() =>
                {
                    errorMessages.Add(ErrorMessageCodeEnum.InvalidCharset);
                })
                .IsHttpMethodInvalid(() =>
                {
                    errorMessages.Add(ErrorMessageCodeEnum.invalidMethod);
                });
            }


            return new ValidateResult()
            {
                Success = !errorMessages.Any(),
                ErrorMessages = errorMessages.ToArray()
            };
        }
    }
}
