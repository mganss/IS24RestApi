using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi.Tests
{
    public class TestBase<T>
    {
        public T Client { get; set; }
        public RestClientStub RestClient { get; set; }

        public TestBase(string baseUrlPrefix, Func<IIS24Connection, T> createClient)
        {
            RestClient = new RestClientStub();

            var connection = new IS24Connection
            {
                RestClientFactory = (baseUrl, oAuth) => {
                    RestClient = new RestClientStub(baseUrl, RestClient);
                    return RestClient;
                },
                BaseUrlPrefix = baseUrlPrefix,
                AccessToken = "AccessToken",
                AccessTokenSecret = "AccessTokenSecret",
                ConsumerKey = "ConsumerKey",
                ConsumerSecret = "ConsumerSecret"
            };

            Client = createClient(connection);
        }
    }

    public class ImportExportTestBase: TestBase<ImportExportClient>
    {
        public ImportExportTestBase(string baseUrlPrefix): base(baseUrlPrefix, c => new ImportExportClient(c)) { }
    }
}
