using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Mocker.ContentTypeState;
using Mocker.Enums;
using Mocker.Extensions;
using Mocker.Services.Abstracts;

namespace Mocker.Services.Concretes
{
    public class ContentTypeService : IContentTypeService
    {
        private readonly IEnumerable<ContentTypeEnum> _enums;

        public ContentTypeService()
        {
            _enums = new List<ContentTypeEnum>().AddEnums();
        }

        public ContentTypeEnum ConvertToEnum(string contentType)
        {
            ContentTypeEnum result = ContentTypeEnum.TextPlain;

            _enums.Contains(contentType, (@enum) =>
            {
                result = @enum;
            });

            return result;
        }

        public IContentTypeMockState GetState(string contentType)
        {
            ContentTypeEnum @enum = ConvertToEnum(contentType);

            switch (@enum)
            {
                case ContentTypeEnum.ApplicationJson:
                case ContentTypeEnum.TextJson:

                    return new JsonMockState();
                case ContentTypeEnum.TextCss:
                case ContentTypeEnum.TextHtml:
                case ContentTypeEnum.ApplicationJavascript:
                case ContentTypeEnum.ApplicationXHtmlXml:

                case ContentTypeEnum.ApplicationXml:
                case ContentTypeEnum.TextXml:

                case ContentTypeEnum.MultipartFormData:
                case ContentTypeEnum.ApplicationXWWWFormUrlencoded:

                case ContentTypeEnum.TextCsv:
                case ContentTypeEnum.TextPlain:
                    return new ContentTypeMockState();

                default:
                    throw new InvalidEnumArgumentException(nameof(contentType), (int)@enum, typeof(ContentTypeEnum));
            }
        }
    }
}
