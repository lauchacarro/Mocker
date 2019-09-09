using System;
using System.Threading.Tasks;
using Mocker.Models.File;

namespace Mocker.Services.Abstracts
{
    public interface IFileService
    {
        Task<Guid> Create(FileModel file);
        Task<FileModel> GetFile(Guid guid);
    }
}
