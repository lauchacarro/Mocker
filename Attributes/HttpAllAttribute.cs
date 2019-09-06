using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mocker.Attributes
{
    public class HttpAllAttribute : HttpMethodAttribute
    {
        public HttpAllAttribute(string template) : base(GetHttpMethods(), template)
        {

        }

        private static IEnumerable<string> GetHttpMethods()
        {
            return Enum.GetNames(typeof(HttpMethod)).ToList();

        }
    }
}
