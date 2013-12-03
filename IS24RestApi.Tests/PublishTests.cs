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
    public class PublishTests : TestBase
    {
        public PublishTests()
            : base(@"http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0")
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
                return new publishObjects { publishObject = new[] { new PublishObject { id = "4711" } } };
            });

            await Client.Publish.PublishAsync(new ApartmentRent { id = 4711, idSpecified = true });
        }

        [Fact]
        public async Task Publish_Publish_NotPublished_PublishesRealEstate()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                return new publishObjects { publishObject = new PublishObject[] { } };
            }).ThenWith(m =>
            {
                Assert.Equal("POST", m);
                var po = new BaseXmlDeserializer().Deserialize<PublishObject>(new RestResponse { Content = Http.RequestBody });
                Assert.Equal(4711, po.realEstate.id);
                Assert.Equal(ImportExportClient.ImmobilienscoutPublishChannelId, po.publishChannel.id);
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_CREATED, message = "Publish Object with id [4711] has been created." } } };
            });

            await Client.Publish.PublishAsync(new ApartmentRent { id = 4711, idSpecified = true });
        }

        [Fact]
        public async Task Publish_Publish_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                return new publishObjects { publishObject = new PublishObject[] { } };
            }).ThenWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.Publish.PublishAsync(new ApartmentRent { id = 4711, idSpecified = true });
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
                return new publishObjects { publishObject = new PublishObject[] { } };
            });

            await Client.Publish.UnpublishAsync(new ApartmentRent { id = 4711, idSpecified = true });
        }

        [Fact]
        public async Task Publish_Unpublish_Published_UnpublishesRealEstate()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                return new publishObjects { publishObject = new [] { new PublishObject { id = "4711" } } };
            }).ThenWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/publish/4711", Http.Url.AbsoluteUri);
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_DELETED, message = "" } } };
            });

            await Client.Publish.UnpublishAsync(new ApartmentRent { id = 4711, idSpecified = true });
        }

        [Fact]
        public async Task Publish_Unpublish_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                return new publishObjects { publishObject = new[] { new PublishObject { id = "4711" } } };
            }).ThenWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.Publish.UnpublishAsync(new ApartmentRent { id = 4711, idSpecified = true });
            });
        }
    }
}
