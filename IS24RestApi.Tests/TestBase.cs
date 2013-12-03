using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi.Tests
{
    public class TestBase
    {
        public ImportExportClient Client { get; set; }
        public HttpStub Http { get; set; }

        public TestBase(string baseUrlPrefix)
        {
            Http = new HttpStub();

            var connection = new IS24Connection
            {
                HttpFactory = new HttpFactory(Http),
                BaseUrlPrefix = baseUrlPrefix,
                AccessToken = "AccessToken",
                AccessTokenSecret = "AccessTokenSecret",
                ConsumerKey = "ConsumerKey",
                ConsumerSecret = "ConsumerSecret"
            };

            Client = new ImportExportClient(connection);
        }
    }
}
