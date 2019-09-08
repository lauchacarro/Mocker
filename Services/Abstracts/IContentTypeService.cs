using Mocker.ContentTypeState;
using Mocker.Enums;

namespace Mocker.Services.Abstracts
{
    public interface IContentTypeService
    {
        ContentTypeEnum ConvertToEnum(string contentType);
        IContentTypeMockState GetState(string contentType);
    }
}
