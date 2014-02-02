using IS24RestApi.Common;
using IS24RestApi.Offer.RealEstates;
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
    public class PublishTests : ImportExportTestBase
    {
        public PublishTests()
            : base(@"http://rest.sandbox-immobilienscout24.de/restapi/api")
        { }
    
        [Fact]
        public async Task Publish_Publish_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal(4711, int.Parse(Http.Parameters.Single(p => p.Name == "realestate").Value));
                Assert.Equal(ImportExportClient.ImmobilienscoutPublishChannelId, int.Parse(Http.Parameters.Single(p => p.Name == "publishchannel").Value));
                var url = Http.Url.GetLeftPart(UriPartial.Path);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/publish", url);
                return new PublishObjects { PublishObject = { new PublishObject { Id = "4711" } } };
            }).ThenWith(m =>
            {
                Assert.Equal(4711, int.Parse(Http.Parameters.Single(p => p.Name == "publishchannel").Value));
                return new PublishObjects { PublishObject = { new PublishObject { Id = "4711" } } };
            }).ThenWith(m =>
            {
                Assert.Equal(4711, int.Parse(Http.Parameters.Single(p => p.Name == "publishchannel").Value));
                return new PublishObjects { PublishObject = { new PublishObject { Id = "4711" } } };
            });

            await Client.Publish.PublishAsync(new ApartmentRent { Id = 4711 });
            await Client.Publish.PublishAsync(new ApartmentRent { Id = 4711 }, 4711);
            await Client.Publish.PublishAsync(new ApartmentRent { Id = 4711 }, new PublishChannel { Id = 4711 });
        }

        [Fact]
        public async Task Publish_Publish_NotPublished_PublishesRealEstate()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                return new PublishObjects { PublishObject = { } };
            }).ThenWith(m =>
            {
                Assert.Equal("POST", m);
                var po = new BaseXmlDeserializer().Deserialize<PublishObject>(new RestResponse { Content = Http.RequestBody });
                Assert.Equal(4711, po.RealEstate.Id);
                Assert.Equal(ImportExportClient.ImmobilienscoutPublishChannelId, po.PublishChannel.Id);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Publish Object with id [4711] has been created." } } };
            });

            await Client.Publish.PublishAsync(new ApartmentRent { Id = 4711 });
        }

        [Fact]
        public async Task Publish_Publish_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                return new PublishObjects { PublishObject = { } };
            }).ThenWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.Publish.PublishAsync(new ApartmentRent { Id = 4711 });
            });
        }

        [Fact]
        public async Task Publish_Unpublish_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal(4711, int.Parse(Http.Parameters.Single(p => p.Name == "realestate").Value));
                Assert.Equal(ImportExportClient.ImmobilienscoutPublishChannelId, int.Parse(Http.Parameters.Single(p => p.Name == "publishchannel").Value));
                var url = Http.Url.GetLeftPart(UriPartial.Path);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/publish", url);
                return new PublishObjects { PublishObject = { } };
            }).ThenWith(m =>
            {
                Assert.Equal(4711, int.Parse(Http.Parameters.Single(p => p.Name == "publishchannel").Value));
                return new PublishObjects { PublishObject = { } };
            }).ThenWith(m =>
            {
                Assert.Equal(4711, int.Parse(Http.Parameters.Single(p => p.Name == "publishchannel").Value));
                return new PublishObjects { PublishObject = { } };
            });

            await Client.Publish.UnpublishAsync(new ApartmentRent { Id = 4711 });
            await Client.Publish.UnpublishAsync(new ApartmentRent { Id = 4711 }, 4711);
            await Client.Publish.UnpublishAsync(new ApartmentRent { Id = 4711 }, new PublishChannel { Id = 4711 });
        }

        [Fact]
        public async Task Publish_Unpublish_Published_UnpublishesRealEstate()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                return new PublishObjects { PublishObject = { new PublishObject { Id = "4711" } } };
            }).ThenWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/publish/4711", Http.Url.AbsoluteUri);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "" } } };
            });

            await Client.Publish.UnpublishAsync(new ApartmentRent { Id = 4711 });
        }

        [Fact]
        public async Task Publish_Unpublish_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                return new PublishObjects { PublishObject = { new PublishObject { Id = "4711" } } };
            }).ThenWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.Publish.UnpublishAsync(new ApartmentRent { Id = 4711 });
            });
        }
    }
}
