using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mocker.Extensions
{
    public static class HttpExtension
    {
        public static void AddHeaders(this HttpResponse response, params KeyValuePair<string, string>[] headers)
        {
            if (headers != null && headers.Any())
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    response.Headers.Add(header.Key, header.Value);
                }
            }
        }

        public static HttpRequest HasQueryValues(this HttpRequest request, Action<IQueryCollection> callback)
        {
            if (request.Query.Count > 0)
                callback(request.Query);

            return request;
        }
    }
}
