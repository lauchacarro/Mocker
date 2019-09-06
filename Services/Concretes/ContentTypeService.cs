using Mocker.Enums;
using Mocker.Services.Abstracts;
using System;
using System.ComponentModel;
using System.Linq;

namespace Mocker.Services.Concretes
{
    public class ContentTypeService : IContentTypeService
    {
        public bool Validate(string contentType)
        {
            var enumType = typeof(ContentTypeEnum);

            var contentTypeNames = Enum.GetNames(enumType).Select(x => x.ToLower());
            if (contentTypeNames.Contains(NormalizeContentType()))
            {
                return true;
            }

            foreach (var member in enumType.GetMembers())
            {
                var valueAttributes = member.GetCustomAttributes(typeof(DescriptionAttribute), false);

                var description = ((DescriptionAttribute)valueAttributes[0]).Description;
                if (description == NormalizeContentType())
                {
                    return true;
                }
            }

            return false;

            string NormalizeContentType()
            {
                return contentType.Replace("/", "").Replace("-", "").Replace("+", "").Replace(" ", "").ToLower();
            }
        }


    }
}
