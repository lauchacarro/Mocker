using Mocker.Enums;
using Mocker.Extensions;
using Mocker.Models;
using Mocker.Models.Postman;
using Mocker.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mocker.Services.Concretes
{
    public class PostmanService : IPostmanService
    {
        private readonly IHttpClientFactory _clientFactory;

        public PostmanService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<PostmanResponse> SendRequest(PostmanRequest request)
        {
            HttpMethod httpMethod = new HttpMethod(request.HttpMethod);
            var httpRquest = new HttpRequestMessage(httpMethod, request.Url);

            HttpClient client = _clientFactory.CreateClient(nameof(PostmanService));

            var httpResponse = await client.SendAsync(httpRquest);

            PostmanResponse response = new PostmanResponse();
            response.StatusCode = (int)httpResponse.StatusCode;
            response.StatusCodeText = httpResponse.StatusCode.ToString();
            response.Body = await httpResponse.Content.ReadAsStringAsync();
            response.ContentType = httpResponse.Content.Headers.ContentType.MediaType;
            List<KeyValue> headerList = new List<KeyValue>();
            foreach (KeyValuePair<string, IEnumerable<string>> header in httpResponse.Content.Headers)
            {
                if(header.Value.Count() > 1)
                {
                    foreach (string value in header.Value)
                    {
                        headerList.Add(new KeyValue(header.Key, value));
                    }
                }
                else if (header.Value.Count() == 1)
                {
                    headerList.Add(new KeyValue(header.Key, header.Value.First()));
                }
                else
                {
                    headerList.Add(new KeyValue(header.Key));
                }
            }

            response.Headers = headerList;

            return response;
        }

        public ValidateResult Validate(PostmanRequest request)
        {
            List<ErrorMessageCodeEnum> errorMessages = new List<ErrorMessageCodeEnum>();

            request.IsUrlInvalid(() =>
            {
                errorMessages.Add(ErrorMessageCodeEnum.InvalidUrl);
            })
            .IsMethodInvalid(() =>
            {
                errorMessages.Add(ErrorMessageCodeEnum.invalidMethod);
            });

            return new ValidateResult()
            {
                Success = !errorMessages.Any(),
                ErrorMessages = errorMessages.ToArray()
            };
        }
    }
}
