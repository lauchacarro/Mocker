using Microsoft.Extensions.Options;
using Mocker.Enums;
using Mocker.Models;
using Mocker.Models.Settings;
using Mocker.Services.Abstracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mocker.Services.Concretes
{
    public class MockService : IMockService
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IGitHubService _githubService;
        private readonly GitHubSetting _githubSetting;
        private readonly string _guidMethodsFolderPath;

        public MockService(IContentTypeService contentTypeService, IGitHubService githubService, IOptions<GitHubSetting> githubSetting)
        {
            _contentTypeService = contentTypeService;
            _githubService = githubService;
            _githubSetting = githubSetting.Value;
            _guidMethodsFolderPath = Path.Combine(_githubSetting.HttpMethodsFolderPath, "guidmethods");
        }

        public async Task<Guid> Create(params MockModel[] request)
        {
            Guid guid = Guid.NewGuid();

            GuidMethodsModel guidMethodsModel = new GuidMethodsModel
            {
                HttpMethods = request.Select(x => x.HttpMethod).ToArray()
            };

            await _githubService.CreateFile(_guidMethodsFolderPath, guid, JsonConvert.SerializeObject(guidMethodsModel));

            foreach (MockModel mock in request)
            {
                string content = JsonConvert.SerializeObject(mock);
                string path = Path.Combine(_githubSetting.HttpMethodsFolderPath, mock.HttpMethod.ToUpper());
                await _githubService.CreateFile(path, guid, content);
            }

            return guid;
        }

        public async Task<GuidMethodsModel> GetHttpMethodsByGuid(Guid guid)
        {
            string jsonGuidMethodsModel = await _githubService.GetFileContent(_guidMethodsFolderPath, guid);

            return JsonConvert.DeserializeObject<GuidMethodsModel>(jsonGuidMethodsModel);
        }

        public async Task<MockModel> GetMock(Guid guid, string httpMethod)
        {
            GuidMethodsModel guidMethodsModel = await GetHttpMethodsByGuid(guid);

            httpMethod = guidMethodsModel.HttpMethods.Contains(httpMethod) ? httpMethod : "GET";

            string path = Path.Combine(_githubSetting.HttpMethodsFolderPath, httpMethod.ToUpper());

            string content = await _githubService.GetFileContent(path, guid);

            return JsonConvert.DeserializeObject<MockModel>(content);
        }

        public async Task<ValidateResult> Validate(params MockModel[] request)
        {

            List<ErrorMessageCodeEnum> errorMessages = new List<ErrorMessageCodeEnum>();

            if (request.GroupBy(x => x.HttpMethod.ToLower()).Any(x => x.Count() > 1))
            {
                errorMessages.Add(ErrorMessageCodeEnum.HttpMethodDuplicate);
                return new ValidateResult()
                {
                    Success = false,
                    ErrorMessages = errorMessages.ToArray()
                };
            }

            foreach (MockModel mock in request)
            {
                if (mock.StatusCode <= 0)
                {
                    errorMessages.Add(ErrorMessageCodeEnum.StatusCodeSmallerThanOne);
                }
                if (!_contentTypeService.Validate(mock.ContentType))
                {
                    errorMessages.Add(ErrorMessageCodeEnum.InvalidContentType);
                }
                if (string.IsNullOrWhiteSpace(mock.Charset))
                {
                    errorMessages.Add(ErrorMessageCodeEnum.InvalidCharset);
                }
                if (string.IsNullOrWhiteSpace(mock.HttpMethod))
                {
                    errorMessages.Add(ErrorMessageCodeEnum.invalidMethod);
                }
            }


            return new ValidateResult()
            {
                Success = !errorMessages.Any(),
                ErrorMessages = errorMessages.ToArray()
            };
        }
    }
}
