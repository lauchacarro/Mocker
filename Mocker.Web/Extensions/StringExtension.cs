using System;

namespace Mocker.Web.Extensions
{
    public static class StringExtension
    {
        public static bool IsUrlValid(this string str)
        {
            return Uri.TryCreate(str, UriKind.RelativeOrAbsolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
