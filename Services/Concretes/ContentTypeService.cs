using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Mocker.ContentTypeState;
using Mocker.Enums;
using Mocker.Services.Abstracts;

namespace Mocker.Services.Concretes
{
    public class ContentTypeService : IContentTypeService
    {
        private readonly List<string> contentTypeNames = Enum.GetNames(typeof(ContentTypeEnum)).Select(x => x.ToLower()).ToList();
        public ContentTypeEnum ConvertToEnum(string contentType)
        {
            ContentTypeEnum[] listEnum = Enum.GetValues(typeof(ContentTypeEnum)).Cast<ContentTypeEnum>().ToArray();

            for (int i = 0; i < contentTypeNames.Count; i++)
            {
                if (contentTypeNames[i].ToLower() == NormalizeContentType(contentType))
                {
                    return listEnum[i];
                }
            }
            throw new InvalidEnumArgumentException(nameof(contentType), -1, typeof(ContentTypeEnum));


        }
        public bool Validate(string contentType)
        {
            if (contentTypeNames.Contains(NormalizeContentType(contentType)))
            {
                return true;
            }

            foreach (var member in typeof(ContentTypeEnum).GetMembers())
            {
                var valueAttributes = member.GetCustomAttributes(typeof(DescriptionAttribute), false);

                var description = ((DescriptionAttribute)valueAttributes[0]).Description;
                if (NormalizeContentType(description) == NormalizeContentType(contentType))
                {
                    return true;
                }
            }

            return false;


        }
        public string NormalizeContentType(string contentType)
        {
            return contentType.Replace("/", "").Replace("-", "").Replace("+", "").Replace(" ", "").ToLower();
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
