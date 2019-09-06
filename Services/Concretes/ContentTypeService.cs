using Mocker.Enums;
using Mocker.Services.Abstracts;
using System;
using System.ComponentModel;
using System.Linq;

namespace Mocker.Services.Concretes
{
    public class ContentTypeService : IContentTypeService
    {
        public ContentTypeEnum ConvertToEnum(string contentType)
        {
            var enumType = typeof(ContentTypeEnum);

            var contentTypeNames = Enum.GetNames(enumType).Select(x => x.ToLower());

            foreach (string ctName in contentTypeNames)
            {
                if(ctName.ToLower() == NormalizeContentType(contentType))
                {
                    return (ContentTypeEnum)Enum.Parse(enumType, ctName);
                }
            }
            return ContentTypeEnum.TextPlain;

        }
        public bool Validate(string contentType)
        {
            var enumType = typeof(ContentTypeEnum);

            var contentTypeNames = Enum.GetNames(enumType).Select(x => x.ToLower());
            if (contentTypeNames.Contains(NormalizeContentType(contentType)))
            {
                return true;
            }

            foreach (var member in enumType.GetMembers())
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
        string NormalizeContentType(string contentType)
        {
            return contentType.Replace("/", "").Replace("-", "").Replace("+", "").Replace(" ", "").ToLower();
        }


    }
}
