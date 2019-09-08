using System;
using System.Collections.Generic;
using System.Linq;

namespace Mocker.Extensions
{
    public static class EnumDataExtension
    {
        public static string NormalizeData(this string original)
        {
            return original.Replace("/", "").Replace("-", "").Replace("+", "").Replace(" ", "").ToLower();
        }

        public static IEnumerable<string> NormalizeEnumerableData<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Select(x => x.ToString().NormalizeData());
        }

        public static IEnumerable<T> AddEnums<T>(this IEnumerable<T> enumerable) where T : Enum
        {
            return enumerable.Concat(Enum.GetValues(typeof(T)).Cast<T>());
        }

        public static IEnumerable<T> Contains<T>(this IEnumerable<T> enumerable, string original, Action<T> callback)
        {
            original = original.NormalizeData();
            var enumerableNormalized = enumerable.NormalizeEnumerableData();

            if (enumerableNormalized.Contains(original))
            {
                var index = enumerableNormalized.ToList().IndexOf(original);
                callback(enumerable.ToArray()[index]);
            }

            return enumerable;
        }

        public static IEnumerable<T> NoContains<T>(this IEnumerable<T> enumerable, string original, Action callback)
        {
            original = original.NormalizeData();
            var enumerableNormalized = enumerable.NormalizeEnumerableData();

            if (!enumerableNormalized.Contains(original))
            {
                callback();
            }

            return enumerable;
        }
    }
}
