using Microsoft.AspNetCore.Http;
using Mocker.Web.Models.Mock;

namespace Mocker.Web.Extensions
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
