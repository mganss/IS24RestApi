using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IS24RestApi.Tests
{
    public class AttachmentTests : TestBase
    {
        public AttachmentTests()
            : base(@"http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0")
        { }

        [Fact]
        public async Task Attachment_Get_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment/1", Http.Url.ToString());
                return new Picture { id = 1, idSpecified = true };
            });

            var a = await Client.Attachments.GetAsync(new ApartmentRent { id = 4711, idSpecified = true }, "1");
        }

        [Fact]
        public async Task Attachment_Get_ResourceDoesNotExist_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.NotFound, ResponseObject = new messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var a = await Client.Attachments.GetAsync(new ApartmentRent(), "x");
            });
        }

        [Fact]
        public async Task Attachment_Get_CanDeserializeResponse()
        {
            Http.RespondWith(m =>
            {
                return new Picture { id = 1, idSpecified = true };
            });

            var a = await Client.Attachments.GetAsync(new ApartmentRent { id = 4711, idSpecified = true }, "1");

            Assert.IsType<Picture>(a);
            Assert.Equal(1, a.id);
        }

        [Fact]
        public async Task Attachment_Update_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("PUT", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment/1", Http.Url.AbsoluteUri);
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, message = "" } } };
            });

            await Client.Attachments.UpdateAsync(new ApartmentRent { id = 4711, idSpecified = true }, new Picture { id = 1, idSpecified = true });
        }

        [Fact]
        public async Task Attachment_Update_CallSucceeds_NoExceptionThrown()
        {
            Http.RespondWith(m =>
            {
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, message = "" } } };
            });

            await Client.Attachments.UpdateAsync(new ApartmentRent { id = 4711, idSpecified = true }, new Picture { id = 1, idSpecified = true });
        }

        [Fact]
        public async Task Attachment_Update_PostsAttachmentObject()
        {
            Http.RespondWith(m =>
            {
                var a = new BaseXmlDeserializer().Deserialize<Attachment>(new RestResponse { Content = Http.RequestBody });
                Assert.IsAssignableFrom<Attachment>(a);
                Assert.Equal(1, a.id);
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, message = "" } } };
            });

            await Client.Attachments.UpdateAsync(new ApartmentRent { id = 4711, idSpecified = true }, new Picture { id = 1, idSpecified = true });
        }

        [Fact]
        public async Task Attachment_Update_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.Attachments.UpdateAsync(new ApartmentRent { id = 4711, idSpecified = true }, new Picture { id = 1, idSpecified = true });
            });
        }

        [Fact]
        public async Task Attachment_GetAll_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment", Http.Url.AbsoluteUri);
                return new Attachments { attachment = new Attachment[] { } };
            });

            var a = await Client.Attachments.GetAsync(new ApartmentRent { id = 4711, idSpecified = true });
        }

        [Fact]
        public async Task Attachment_GetAll_CanDeserializeResponse()
        {
            Http.RespondWith(m =>
            {
                return new Attachments
                {
                    attachment = new [] { 
                        new Attachment { id = 4711, idSpecified = true },
                        new Attachment { id = 4712, idSpecified = true },
                    }
                };
            });

            var a = (await Client.Attachments.GetAsync(new ApartmentRent { id = 4711, idSpecified = true })).ToList();

            Assert.Equal(2, a.Count);
            Assert.Equal(4711, a[0].id);
            Assert.Equal(4712, a[1].id);
        }

        [Fact]
        public async Task Attachment_Delete_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment/1", Http.Url.ToString());
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_DELETED, message = "" } } };
            });

            await Client.Attachments.DeleteAsync(new ApartmentRent { id = 4711, idSpecified = true }, "1");
        }

        [Fact]
        public async Task Attachment_Create_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("POST", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment", Http.Url.AbsoluteUri);
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_CREATED, message = "Resource with id [4711] has been created." } } };
            });

            await Client.Attachments.CreateAsync(new ApartmentRent { id = 4711, idSpecified = true }, new Picture { title = "Test" }, @"..\..\test.jpg");
        }

        [Fact]
        public async Task Attachment_Create_HasFile()
        {
            var bytes = File.ReadAllBytes(@"..\..\test.jpg");

            Http.RespondWith(m =>
            {
                var file = Http.Files.Single(f => f.Name == "attachment");
                Assert.Equal("image/jpeg", file.ContentType);
                Assert.Equal("test.jpg", file.FileName);
                Assert.Equal(bytes.Length, file.ContentLength);
                var ms = new MemoryStream();
                file.Writer(ms);
                AssertEx.CollectionEqual(bytes, ms.ToArray());
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_CREATED, message = "Resource with id [4711] has been created." } } };
            });

            await Client.Attachments.CreateAsync(new ApartmentRent { id = 4711, idSpecified = true }, new Picture { title = "Test" }, @"..\..\test.jpg");
        }

        [Fact]
        public async Task Attachment_Create_HasMetadata()
        {
            Http.RespondWith(m =>
            {
                var meta = Http.Files.Single(f => f.Name == "metadata");
                Assert.Equal("application/xml", meta.ContentType);
                Assert.Equal("body.xml", meta.FileName);
                var ms = new MemoryStream();
                meta.Writer(ms);
                var bytes = ms.ToArray();
                Assert.Equal(bytes.Length, meta.ContentLength);
                var content = Encoding.UTF8.GetString(bytes);
                var a = new BaseXmlDeserializer().Deserialize<Attachment>(new RestResponse { Content = content });
                Assert.IsType<Picture>(a);
                Assert.Equal("Test", a.title);

                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_CREATED, message = "Resource with id [4711] has been created." } } };
            });

            await Client.Attachments.CreateAsync(new ApartmentRent { id = 4711, idSpecified = true }, new Picture { title = "Test" }, @"..\..\test.jpg");
        }

        [Fact]
        public async Task Attachment_Create_CanDeserializeResponse()
        {
            Http.RespondWith(m =>
            {
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_CREATED, message = "Resource with id [4711] has been created." } } };
            });

            var att = new Picture { title = "Test" };

            await Client.Attachments.CreateAsync(new ApartmentRent { id = 4711, idSpecified = true }, att, @"..\..\test.jpg");

            Assert.Equal(4711, att.id);
        }

        [Fact]
        public async Task Attachment_Create_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.Attachments.CreateAsync(new ApartmentRent { id = 4711, idSpecified = true }, new Picture { id = 1, idSpecified = true }, @"..\..\test.jpg");
            });
        }
    }
}
