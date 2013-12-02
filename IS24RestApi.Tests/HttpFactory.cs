using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi.Tests
{
    class HttpFactory : IHttpFactory
    {
        public IHttp Object { get; set; }

        public HttpFactory(IHttp httpFactory)
        {
            Object = httpFactory;
        }

        public IHttp Create()
        {
            return Object;
        }
    }
}
