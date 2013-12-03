using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IS24RestApi.Tests
{
    public class PublishChannelTests : TestBase
    {
        public PublishChannelTests()
            : base(@"http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0")
        { }

        [Fact]
        public async Task PublishChannel_Get_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/publishchannel", Http.Url.AbsoluteUri);
                return new publishChannels { publishChannel = new PublishChannel[] { } };
            });

            var cs = await Client.PublishChannels.GetAsync();
        }

        [Fact]
        public async Task PublishChannel_Get_CanDeserializeResponse()
        {
            Http.RespondWith(m =>
            {
                return new publishChannels
                {
                    publishChannel = new[] { 
                        new PublishChannel { id = 4711, idSpecified = true },
                        new PublishChannel { id = 4712, idSpecified = true },
                    }
                };
            });

            var pcs = (await Client.PublishChannels.GetAsync()).ToList();

            Assert.Equal(2, pcs.Count);
            Assert.Equal(4711, pcs[0].id);
            Assert.Equal(4712, pcs[1].id);
        }
    }
}
