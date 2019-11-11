using System;
using System.Threading.Tasks;

namespace Mocker.Web.Extensions
{
    public static class ObjectExtension
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
    }
}
