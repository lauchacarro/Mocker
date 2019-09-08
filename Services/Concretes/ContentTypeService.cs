using Mocker.ContentTypeState;
using Mocker.Enums;
using Mocker.Extensions;
using Mocker.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
                    return new ApplicationJsonMockState();
                case ContentTypeEnum.TextHtml:
                    return new TextHtmlMockState();
                case ContentTypeEnum.ApplicationXWWWFormUrlencoded:
                case ContentTypeEnum.ApplicationXHtmlXml:
                case ContentTypeEnum.ApplicationXml:
                case ContentTypeEnum.MultipartFormData:
                case ContentTypeEnum.TextCss:
                case ContentTypeEnum.TextCsv:
                case ContentTypeEnum.TextJson:
                case ContentTypeEnum.TextPlain:
                case ContentTypeEnum.TextXml:
                    return new ContentTypeMockState();
                default:
                    throw new InvalidEnumArgumentException(nameof(contentType), (int)@enum, typeof(ContentTypeEnum));
            }
        }
    }
}
