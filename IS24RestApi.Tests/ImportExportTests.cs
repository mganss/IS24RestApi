using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IS24RestApi.Tests
{
    public class ImportExportTests
    {
        IS24Client Client { get; set; }
        HttpStub Http { get; set; }

        public ImportExportTests()
        {
            Http = new HttpStub();
            Client = new IS24Client
            {
                HttpFactory = new HttpFactory(Http),
                BaseUrlPrefix = @"http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0",
                AccessToken = "AccessToken",
                AccessTokenSecret = "AccessTokenSecret",
                ConsumerKey = "ConsumerKey",
                ConsumerSecret = "ConsumerSecret"
            };
        }

        [Fact]
        public async Task GetRealEstateAsync_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/test", Http.Url.ToString());
                return new ApartmentRent { title = "Test" };
            });

            var re = await Client.GetRealEstateAsync("test");
        }

        [Fact]
        public async Task GetRealEstateAsync_RequestsCorrectExternalIdPrefix()
        {
            Http.RespondWith(m =>
            {
                Assert.True(Http.Url.ToString().EndsWith("/ext-test"));
                return new ApartmentRent { title = "test" };
            });

            var re = await Client.GetRealEstateAsync("test", isExternal: true);
        }

        [Fact]
        public async Task GetRealEstateAsync_ResourceDoesNotExist_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.NotFound, ResponseObject = new messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var re = await Client.GetRealEstateAsync("test");
            });
        }

        [Fact]
        public async Task GetRealEstateAsync_CanDeserializeResponse()
        {
            Http.RespondWith(m =>
            {
                return new ApartmentRent { title = "test" };
            });

            var re = await Client.GetRealEstateAsync("test");

            Assert.IsType<ApartmentRent>(re);
            Assert.Equal("test", re.title);
        }
    }
}
