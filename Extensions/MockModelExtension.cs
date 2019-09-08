using Microsoft.AspNetCore.Http;
using Mocker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mocker.Extensions
{
    public static class MockModelExtension
    {
        public static MockModel ResolveDynamicBody(this MockModel mock, IQueryCollection query)
        {
            foreach (string key in query.Keys)
            {
                mock.Body = mock.Body.Replace("{{ " + key + " }}", query[key]);
            }

            return mock;
        }
    }
}
