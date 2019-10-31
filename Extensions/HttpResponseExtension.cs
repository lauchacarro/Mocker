﻿using Microsoft.AspNetCore.Http;
using Mocker.Models;
using Mocker.Models.Mock;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mocker.Extensions
{
    public static class HttpResponseExtension
    {
        public static async Task<HttpResponse> WriteMock(this HttpResponse response, MockModel mock)
        {
            response.AddHeaders(mock.Headers);
            response.StatusCode = mock.StatusCode;
            response.ContentType = $"{mock.ContentType}; charset={mock.Charset.ToLower()}";

            string body = mock.ContentType switch
            {
                "application/json" => ((Func<string>)(() =>
                {
                    object bodyObject = JsonSerializer.Deserialize<object>(mock.Body);
                    return JsonSerializer.Serialize(bodyObject);
                }))(),

                _ => mock.Body
            };

            await response.WriteAsync(body);
            return response;
        }

        public static void AddHeaders(this HttpResponse response, params KeyValue[] headers)
        {
            if (headers != null && headers.Any())
            {
                string allowHeaders = string.Empty;
                foreach (KeyValue header in headers)
                {
                    response.Headers.Add(WebUtility.UrlEncode(header.Key), WebUtility.UrlEncode(header.Value));
                    allowHeaders += header.Key.ToLower();
                    if (headers.Last() != header)
                        allowHeaders += ", ";
                }
            }
        }
    }
}