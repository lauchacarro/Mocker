using Mocker.Models;
using System;
using System.Threading.Tasks;

namespace Mocker.Services.Abstracts
{
    public interface IMockService
    {
        Task<ValidateResult> Validate(params MockModel[] request);
        Task<Guid> Create(params MockModel[] request);
        Task<MockModel> GetMock(Guid guid, string httpMethod);
        Task<GuidMethodsModel> GetHttpMethodsByGuid(Guid guid);
    }
}
