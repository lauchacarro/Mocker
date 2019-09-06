using Mocker.ContentTypeState;
using Mocker.Enums;

namespace Mocker.Services.Abstracts
{
    public interface IContentTypeService
    {
        bool Validate(string contentType);
        ContentTypeEnum ConvertToEnum(string contentType);
        string NormalizeContentType(string contentType);
        IContentTypeMockState GetState(string contentType);
    }
}
