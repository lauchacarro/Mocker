using Mocker.Models.File;
using System;
using System.Threading.Tasks;

namespace Mocker.Services.Abstracts
{
    public interface IFileService
    {
        Task<Guid> Create(FileModel file);
        Task<FileModel> GetFile(Guid guid);
    }
}
