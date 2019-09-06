using System;
using System.Threading.Tasks;
using Mocker.Models;

namespace Mocker.Services.Abstracts
{
    public interface IMockService
    {
        Task<ValidateResult> Validate(params MockModel[] request);
        Task<OperationResult<Guid>> Create(params MockModel[] request);
        Task<OperationResult<MockModel>> GetMock(Guid guid, string httpMethod);
    }
}
