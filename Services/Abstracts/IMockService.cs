using Mocker.Models;
using Mocker.Models.Requests;
using System;
using System.Threading.Tasks;

namespace Mocker.Services.Abstracts
{
    public interface IMockService
    {
        Task<ValidateResult> Validate(CreateMockRequest request);
        Task<OperationResult<Guid>> Create(CreateMockRequest request);
        Task<OperationResult<CreateMockRequest>> GetMock(Guid guid);
    }
}
