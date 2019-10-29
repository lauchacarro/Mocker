using System;
using System.Threading.Tasks;

namespace Mocker.Extensions
{
    public static class StringExtension
    {
        public static object IsNotNull(this object str, Action callback)
        {
            if (str != null)
                callback();
            return str;
        }

        public static async Task<object> IsNotNullAsync(this object str, Func<Task> callback)
        {
            if (str != null)
                await callback();
            return str;
        }
        public static object IsNull(this object str, Action callback)
        {
            if (str is null)
                callback();
            return str;
        }

        public static bool IsHaveToRunGetMock(this string[] str)
        {
            return str.Length >= 3 && Guid.TryParse(str[2], out _);
        }

        public static bool IsHaveToRunGetRawMockAsync(this string[] str)
        {
            return str.Length >= 4 && str[2].ToLower() == "raw" && Guid.TryParse(str[3], out _);
        }

        public static bool IsHaveToRunReverseProxy(this string[] str)
        {
            return str.Length >= 3 && str[2].ToLower() == "postman";
        }
    }
}
