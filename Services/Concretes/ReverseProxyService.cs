using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Mocker.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mocker.Services.Concretes
{
    public class ReverseProxyService : IReverseProxyService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        public async Task Process(HttpContext context)
        {
            var targetUri = BuildTargetUri(context.Request);

            var targetRequestMessage = CreateTargetMessage(context, targetUri);

            using var responseMessage = await _httpClient.SendAsync(targetRequestMessage, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);

            await ProcessResponseContent(context, responseMessage);

        }

        private async Task ProcessResponseContent(HttpContext context, HttpResponseMessage responseMessage)
        {
            context.Response.StatusCode = (int)responseMessage.StatusCode;

            foreach (var header in responseMessage.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }

            foreach (var header in responseMessage.Content.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }

            context.Response.Headers.Remove("transfer-encoding");

            var content = await responseMessage.Content.ReadAsByteArrayAsync();

            await context.Response.Body.WriteAsync(content);

        }

        private HttpRequestMessage CreateTargetMessage(HttpContext context, Uri targetUri)
        {
            targetUri = new Uri(QueryHelpers.AddQueryString(targetUri.OriginalString, new Dictionary<string, string>()));

            var streamContent = new StreamContent(context.Request.Body);

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = targetUri,
                Method = new HttpMethod(context.Request.Method),
                Content = streamContent
            };

            foreach (var header in context.Request.Headers)
            {
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
            
            requestMessage.Headers.Host = targetUri.Host + ":" + targetUri.Port;

            return requestMessage;
        }

        private Uri BuildTargetUri(HttpRequest request)
        {
            if (request.Headers.TryGetValue("Mocker-Url", out StringValues values))
            {
                foreach (string item in values)
                {
                    if (Uri.TryCreate(item, UriKind.RelativeOrAbsolute, out Uri result))
                    {
                        return result;
                    }
                }
            }

            return null;
        }

    }
}
