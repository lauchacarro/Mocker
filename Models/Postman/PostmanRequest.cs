using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mocker.Models.Postman
{
    public class PostmanRequest
    {
        public string Url { get; set; }
        public string HttpMethod { get; set; }
        public string ContentType { get; set; }
        public IEnumerable<KeyValue> Headers { get; set; }
    }
}
