using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Mocker.Enums;
using Mocker.Models;
using Mocker.Models.Settings;
using Mocker.Services.Abstracts;
using Newtonsoft.Json;

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

        public async Task<OperationResult<Guid>> Create(params MockModel[] request)
        {
            OperationResult<Guid> operationResult = new OperationResult<Guid>();
            Guid guid = Guid.NewGuid();

            GuidMethodsModel guidMethodsModel = new GuidMethodsModel();
            guidMethodsModel.HttpMethods = request.Select(x => x.HttpMethod).ToArray();


            await _githubService.CreateFile(_guidMethodsFolderPath, guid, JsonConvert.SerializeObject(guidMethodsModel));



            foreach (MockModel mock in request)
            {
                string content = JsonConvert.SerializeObject(mock);

                await _githubService.CreateFile(Path.Combine(_githubSetting.HttpMethodsFolderPath, mock.HttpMethod.ToUpper()), guid, content);
            }



            operationResult.Result = guid;
            operationResult.Success = true;

            return operationResult;
        }

        public async Task<OperationResult<MockModel>> GetMock(Guid guid, string httpMethod)
        {
            string jsonGuidMethodsModel = await _githubService.GetFileContent(_guidMethodsFolderPath, guid);
            GuidMethodsModel guidMethodsModel = JsonConvert.DeserializeObject<GuidMethodsModel>(jsonGuidMethodsModel);

            if (!guidMethodsModel.HttpMethods.Contains(httpMethod))
            {
                httpMethod = "GET";
            }

            string content = await _githubService.GetFileContent(Path.Combine(_githubSetting.HttpMethodsFolderPath, httpMethod.ToUpper()), guid);
            MockModel mock = JsonConvert.DeserializeObject<MockModel>(content);
            OperationResult<MockModel> operationResult = new OperationResult<MockModel>
            {
                Success = true,
                Result = mock
            };
            return operationResult;
        }

        public async Task<ValidateResult> Validate(params MockModel[] request)
        {

            List<ErrorMessageCodeEnum> errorMessages = new List<ErrorMessageCodeEnum>();

            if (request.GroupBy(x => x.HttpMethod.ToLower()).Any(x => x.Count() > 1))
            {
                errorMessages.Add(ErrorMessageCodeEnum.HttpMethodDuplicate);
                return new ValidateResult()
                {
                    Success = !errorMessages.Any(),
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
