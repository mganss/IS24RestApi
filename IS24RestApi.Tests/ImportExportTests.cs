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
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711", Http.Url.ToString());
                return new ApartmentRent { title = "Test" };
            });

            var re = await Client.GetRealEstateAsync("4711");
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

        [Fact]
        public async Task CreateRealEstateAsync_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("POST", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate", Http.Url.ToString());
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_CREATED, message = "RealEstate with id [4711] has been created." } } };
            });

            var re = new ApartmentRent { title = "Test" };

            await Client.CreateRealEstateAsync(re);
        }

        [Fact]
        public async Task CreateRealEstateAsync_CallSucceeds_RealEstateObjectHasNewId()
        {
            Http.RespondWith(m =>
            {
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_CREATED, message = "RealEstate with id [4711] has been created." } } };
            });

            var re = new ApartmentRent { title = "Test" };

            await Client.CreateRealEstateAsync(re);

            Assert.Equal(4711, re.id);
        }

        [Fact]
        public async Task CreateRealEstateAsync_PostsRealEstateObject()
        {
            Http.RespondWith(m =>
            {
                var re = new BaseXmlDeserializer().Deserialize<RealEstate>(new RestResponse { Content = Http.RequestBody });
                Assert.IsType<ApartmentRent>(re);
                Assert.Equal("Test", re.title);
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_CREATED, message = "RealEstate with id [4711] has been created." } } };
            });

            var r = new ApartmentRent { title = "Test" };

            await Client.CreateRealEstateAsync(r);
        }

        [Fact]
        public async Task CreateRealEstateAsync_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.CreateRealEstateAsync(new ApartmentRent());
            });
        }

        [Fact]
        public async Task UpdateRealEstateAsync_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("PUT", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711", Http.Url.ToString());
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, message = "" } } };
            });

            var re = new ApartmentRent { id = 4711, title = "Test" };

            await Client.UpdateRealEstateAsync(re);
        }

        [Fact]
        public async Task CreateRealEstateAsync_CallSucceeds_NoExceptionThrown()
        {
            Http.RespondWith(m =>
            {
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, message = "" } } };
            });

            var re = new ApartmentRent { id = 4711, title = "Test" };

            await Client.UpdateRealEstateAsync(re);
        }

        [Fact]
        public async Task UpdateRealEstateAsync_PostsRealEstateObject()
        {
            Http.RespondWith(m =>
            {
                var re = new BaseXmlDeserializer().Deserialize<RealEstate>(new RestResponse { Content = Http.RequestBody });
                Assert.IsType<ApartmentRent>(re);
                Assert.Equal(4711, re.id);
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, message = "" } } };
            });

            var r = new ApartmentRent { id = 4711, idSpecified = true, title = "Test" };

            await Client.UpdateRealEstateAsync(r);
        }

        [Fact]
        public async Task UpdateRealEstateAsync_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.UpdateRealEstateAsync(new ApartmentRent());
            });
        }
    }
}
