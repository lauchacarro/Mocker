using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Mocker.Enums;
using Mocker.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mocker.Extensions
{
    public static class ValidateMockModelExtension
    {
        public static IEnumerable<MockModel> ValidateDuplicateMethod(this IEnumerable<MockModel> mocks, Action callback)
        {
            if (mocks.GroupBy(x => x.HttpMethod.ToLower()).Any(x => x.Count() > 1))
                callback();

            return mocks;
        }

        public static MockModel ValidateStatusCodeSmallerThanOne(this MockModel mock, Action callback)
        {
            if (mock.StatusCode <= 0)
                callback();

            return mock;
        }

        public static MockModel ValidateContentTypeInvalid(this MockModel mock, Action callback)
        {
            IEnumerable<ContentTypeEnum> contentTypeNames = new List<ContentTypeEnum>();

            contentTypeNames.AddEnums()
            .NoContains(mock.ContentType, () =>
            {
                callback();
            });

            return mock;
        }

        public static MockModel ValidateCharsetInvalid(this MockModel mock, Action callback)
        {
            IEnumerable<CharsetEnum> charsetNames = new List<CharsetEnum>();

            charsetNames.AddEnums()
            .NoContains(mock.Charset, () =>
            {
                callback();
            });

            return mock;
        }

        public static MockModel ValidateHttpMethodInvalid(this MockModel mock, Action callback)
        {
            IEnumerable<HttpMethod> httpMethodNames = new List<HttpMethod>();

            httpMethodNames.AddEnums()
            .NoContains(mock.HttpMethod, () =>
            {
                callback();
            });

            return mock;
        }
    }
}
