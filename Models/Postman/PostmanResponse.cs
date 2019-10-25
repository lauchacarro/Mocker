using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mocker.Models.Postman
{
    public class PostmanResponse
    {
        public string ContentType { get; set; }
        public int StatusCode { get; set; }
        public string StatusCodeText { get; set; }
        public string Body { get; set; }
        public IEnumerable<KeyValue> Headers { get; set; }
        public long TimeRequest { get; set; }
    }
}
