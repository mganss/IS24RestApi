using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace IS24RestApi.Tests
{
    static class Extensions
    {
        public static NameValueCollection ParseQueryString(this Uri uri) => HttpUtility.ParseQueryString(uri.Query);

        public static object Body(this IRestRequest r) => r.Parameters.Single(p => p.Type == ParameterType.RequestBody).Value;
    }
}
