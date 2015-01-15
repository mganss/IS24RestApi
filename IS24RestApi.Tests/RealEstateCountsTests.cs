using System.Reactive.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.Common;
using IS24RestApi.Offer.ListElement;

namespace IS24RestApi.Tests
{
    public class RealEstateCountsTests: ImportExportTestBase
    {
        public RealEstateCountsTests()
            : base(@"http://rest.sandbox-immobilienscout24.de/restapi/api")
        {
        }

        [Fact]
        public async Task RealEstateCounts_Get_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestatecounts", Http.Url.GetLeftPart(UriPartial.Path));
                return new Realestate.Counts.RealEstateCounts
                {
                    Is24PublishedRealEstatesCount = 10,
                    Is24NotPublishedRealEstatesCount = 20
                };
            });

            var re = await Client.RealEstateCounts.GetAsync();
        }

        [Fact]
        public async Task RealEstateCounts_Get_CanDeserializeResponse()
        {
            Http.RespondWith(m => new Realestate.Counts.RealEstateCounts
            {
                Is24PublishedRealEstatesCount = 10,
                Is24NotPublishedRealEstatesCount = 20
            });

            var re = await Client.RealEstateCounts.GetAsync();

            Assert.IsType<Realestate.Counts.RealEstateCounts>(re);
            Assert.Equal(10, re.Is24PublishedRealEstatesCount);
            Assert.Equal(20, re.Is24NotPublishedRealEstatesCount);
        }
    }
}
