﻿namespace Mocker.Web.Models.Mock
{
    public class MockModel
    {
        public string HttpMethod { get; set; }
        public int StatusCode { get; set; }
        public string ContentType { get; set; }
        public string Charset { get; set; }
        public KeyValue[] Headers { get; set; }
        public string Body { get; set; }
    }
}
