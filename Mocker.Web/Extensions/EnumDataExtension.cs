﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Mocker.Web.Extensions
{
    public static class EnumDataExtension
    {
        public static string NormalizeData(this string original)
        {
            return original.Replace("/", "").Replace("-", "").Replace("+", "").Replace(" ", "").ToLower();
        }
        public static IEnumerable<string> NormalizeEnumerableData<T>(this IEnumerable<T> enumerable) where T : Enum
        {
            return enumerable.Select(x => x.ToString().NormalizeData());
        }
        public static IEnumerable<T> AddEnums<T>(this IEnumerable<T> enumerable) where T : Enum
        {
            return enumerable.Concat(Enum.GetValues(typeof(T)).Cast<T>());
        }
        public static IEnumerable<T> NoContains<T>(this IEnumerable<T> enumerable, string original, Action callback) where T : Enum
        {
            original = original.NormalizeData();

            if (!enumerable.ContainsBase(original))
            {
                callback();
            }

            return enumerable;
        }
        private static bool ContainsBase<T>(this IEnumerable<T> enumerable, string original) where T : Enum
        {
            original = original.NormalizeData();
            var enumerableNormalized = enumerable.NormalizeEnumerableData();

            return enumerableNormalized.Contains(original);
        }
    }
}