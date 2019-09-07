using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Mocker.Extensions
{
    public static class HttpResponseExtension
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
    }
}
