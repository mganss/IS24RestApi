using IS24RestApi.Common;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IS24RestApi.Tests
{
    public class PublishChannelTests : ImportExportTestBase
    {
        public PublishChannelTests()
            : base(@"https://rest.sandbox-immobilienscout24.de/restapi/api")
        { }

        [Fact]
        public async Task PublishChannel_Get_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.GET, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/publishchannel", RestClient.BuildUri(r).AbsoluteUri);
                return new PublishChannels { PublishChannel = { } };
            });

            var cs = await Client.PublishChannels.GetAsync();
        }

        [Fact]
        public async Task PublishChannel_Get_CanDeserializeResponse()
        {
            RestClient.RespondWith(r =>
            {
                return new PublishChannels
                {
                    PublishChannel = { 
                        new PublishChannel { Id = 4711 },
                        new PublishChannel { Id = 4712 },
                    }
                };
            });

            var pcs = (await Client.PublishChannels.GetAsync()).ToList();

            Assert.Equal(2, pcs.Count);
            Assert.Equal(4711, pcs[0].Id);
            Assert.Equal(4712, pcs[1].Id);
        }
    }
}
