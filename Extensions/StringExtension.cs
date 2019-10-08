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

        public static async Task<string[]> PathHasGuidAsync(this string[] str, Func<Task> callback)
        {
            if (str.Length >= 3 && Guid.TryParse(str[2], out Guid guid))
                await callback();
            return str;
        }

        public static async Task<string[]> PathHasNoGuidAsync(this string[] str, Func<Task> callback)
        {
            if (str.Length < 3 || (str.Length >= 3 && !Guid.TryParse(str[2], out _)))
                await callback();
            return str;
        }

    }
}
