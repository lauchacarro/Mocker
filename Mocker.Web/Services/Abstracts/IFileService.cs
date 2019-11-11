using Mocker.Web.Models;
using Mocker.Web.Models.File;
using System;
using System.Threading.Tasks;

namespace Mocker.Web.Services.Abstracts
{
    public interface IFileService
    {
        ValidateResult Validate(FileModel request);
        Task<Guid> Create(FileModel file);
        Task<FileModel> GetFile(Guid guid);
    }
}
