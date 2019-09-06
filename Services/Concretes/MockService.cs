using Mocker.Enums;
using Mocker.Models;
using Mocker.Models.Requests;
using Mocker.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mocker.Services.Concretes
{
    public class MockService : IMockService
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IGitHubService _githubService;

        public MockService(IContentTypeService contentTypeService, IGitHubService githubService)
        {
            _contentTypeService = contentTypeService;
            _githubService = githubService;
        }

        public async Task<OperationResult<Guid>> Create(CreateMockRequest request)
        {
            OperationResult<Guid> operationResult = new OperationResult<Guid>();
            Guid guid = Guid.NewGuid();
            string content = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            var taskGuidMethods = _githubService.CreateFile("mocks/httpmethods/guidmethods", guid, "{\"httpmethods\":[ \"GET\" ] }");
            var taskCreateMock = _githubService.CreateFile("mocks/httpmethods/" + request.HttpMethod, guid, content);
            Task.WaitAll(taskGuidMethods, taskCreateMock);

            operationResult.Result = guid;
            operationResult.Success = true;

            return operationResult;
        }

        public async Task<OperationResult<CreateMockRequest>> GetMock(Guid guid)
        {
            string content = await _githubService.GetFileContent("mocks/httpmethods/GET", guid);
            CreateMockRequest mock = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateMockRequest>(content);
            OperationResult<CreateMockRequest> operationResult = new OperationResult<CreateMockRequest>
            {
                Success = true,
                Result = mock
            };
            return operationResult;
        }

        public async Task<ValidateResult> Validate(CreateMockRequest request)
        {

            List<ErrorMessageCodeEnum> errorMessages = new List<ErrorMessageCodeEnum>();
            if (request.StatusCode <= 0)
            {
                errorMessages.Add(ErrorMessageCodeEnum.StatusCodeSmallerThanOne);
            }
            if (!_contentTypeService.Validate(request.ContentType))
            {
                errorMessages.Add(ErrorMessageCodeEnum.InvalidContentType);
            }
            if (string.IsNullOrWhiteSpace(request.Charset))
            {
                errorMessages.Add(ErrorMessageCodeEnum.InvalidCharset);
            }
            if (string.IsNullOrWhiteSpace(request.HttpMethod))
            {
                errorMessages.Add(ErrorMessageCodeEnum.invalidMethod);
            }

            return new ValidateResult()
            {
                Success = !errorMessages.Any(),
                ErrorMessages = errorMessages.ToArray()
            };
        }
    }
}
