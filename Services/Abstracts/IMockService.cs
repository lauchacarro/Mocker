using Mocker.Models.Mock;
using Mocker.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mocker.Services.Abstracts
{
    public interface IMockService
    {
        ValidateResult Validate(IEnumerable<MockModel> request);
        Task<Guid> Create(IEnumerable<MockModel> request);
        Task<MockModel> GetMock(Guid guid, string httpMethod);
        Task<GuidMethodsModel> GetHttpMethodsByGuid(Guid guid);
    }
}
