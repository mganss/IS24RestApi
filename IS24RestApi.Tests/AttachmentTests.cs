using IS24RestApi.Common;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.VideoUpload;
using IS24RestApi.Rest;
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
    public class AttachmentTests : ImportExportTestBase
    {
        public AttachmentTests()
            : base(@"https://rest.sandbox-immobilienscout24.de/restapi/api")
        { }

        [Fact]
        public async Task Attachment_Get_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment/1", Http.Url.ToString());
                return new Picture { Id = 1 };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            var a = await re.Attachments.GetAsync("1");
        }

        [Fact]
        public async Task Attachment_Get_ResourceDoesNotExist_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.NotFound, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var re = new RealEstateItem(new ApartmentRent { Id = 1 }, Client.Connection);
                var a = await re.Attachments.GetAsync("x");
            });
        }

        [Fact]
        public async Task Attachment_Get_CanDeserializeResponse()
        {
            Http.RespondWith(m =>
            {
                return new Picture { Id = 1 };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 1 }, Client.Connection);
            var a = await re.Attachments.GetAsync("1");

            Assert.IsType<Picture>(a);
            Assert.Equal(1, a.Id);
        }

        [Fact]
        public async Task Attachment_Update_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("PUT", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment/1", Http.Url.AbsoluteUri);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.UpdateAsync(new Picture { Id = 1 });
        }

        [Fact]
        public async Task Attachment_Update_CallSucceeds_NoExceptionThrown()
        {
            Http.RespondWith(m =>
            {
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.UpdateAsync(new Picture { Id = 1 });
        }

        [Fact]
        public async Task Attachment_Update_PostsAttachmentObject()
        {
            Http.RespondWith(m =>
            {
                var a = new BaseXmlDeserializer().Deserialize<Attachment>(new RestResponse { Content = Http.RequestBody });
                Assert.IsAssignableFrom<Attachment>(a);
                Assert.Equal(1, a.Id);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.UpdateAsync(new Picture { Id = 1 });
        }

        [Fact]
        public async Task Attachment_Update_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
                await re.Attachments.UpdateAsync(new Picture { Id = 1 });
            });
        }

        [Fact]
        public async Task Attachment_GetAll_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment", Http.Url.AbsoluteUri);
                return new Attachments { Attachment = { } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            var a = await re.Attachments.GetAsync();
        }

        [Fact]
        public async Task Attachment_GetAll_CanDeserializeResponse()
        {
            Http.RespondWith(m =>
            {
                return new Attachments
                {
                    Attachment = {
                        new Attachment { Id = 4711 },
                        new Attachment { Id = 4712 },
                    }
                };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            var a = (await re.Attachments.GetAsync()).ToList();

            Assert.Equal(2, a.Count);
            Assert.Equal(4711, a[0].Id);
            Assert.Equal(4712, a[1].Id);
        }

        [Fact]
        public async Task Attachment_Delete_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment/1", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.DeleteAsync("1");
        }

        [Fact]
        public async Task Attachment_Create_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("POST", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment", Http.Url.AbsoluteUri);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource with id [4711] has been created.", Id = "4711" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.CreateAsync(new Picture { Title = "Test" }, @"..\..\test.jpg");
        }

        [Fact]
        public async Task Attachment_CreateLink_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("POST", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment", Http.Url.AbsoluteUri);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource with id [4711] has been created.", Id = "4711" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.CreateAsync(new Link { Url = "http://www.example.com" });
        }

        [Fact]
        public async Task Attachment_CreateLink_CallSucceeds_NoExceptionThrown()
        {
            Http.RespondWith(m =>
            {
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource with id [4711] has been created.", Id = "4711" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.CreateAsync(new Link { Url = "http://www.example.com" });
        }

        [Fact]
        public async Task Attachment_CreateLink_PostsAttachmentObject()
        {
            Http.RespondWith(m =>
            {
                var l = new BaseXmlDeserializer().Deserialize<Attachment>(new RestResponse { Content = Http.RequestBody });
                Assert.IsAssignableFrom<Link>(l);
                Assert.Equal("http://www.example.com", ((Link)l).Url);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource with id [4711] has been created.", Id = "4711" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.CreateAsync(new Link { Url = "http://www.example.com" });
        }

        [Fact]
        public async Task Attachment_CreateLink_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
                await re.Attachments.CreateAsync(new Link { Url = "http://www.example.com" });
            });
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
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource with id [4711] has been created.", Id = "4711" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.CreateAsync(new Picture { Title = "Test" }, @"..\..\test.jpg");
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
                Assert.Equal("Test", a.Title);

                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource with id [4711] has been created.", Id = "4711" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.CreateAsync(new Picture { Title = "Test" }, @"..\..\test.jpg");
        }

        [Fact]
        public async Task Attachment_Create_CanDeserializeResponse()
        {
            Http.RespondWith(m =>
            {
                var msgs = new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource with id [4711] has been created.", Id = "4711" } } };
                return msgs;
            });

            var att = new Picture { Title = "Test" };

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.CreateAsync(att, @"..\..\test.jpg");

            Assert.Equal(4711, att.Id);
        }

        [Fact]
        public async Task Attachment_Create_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
                await re.Attachments.CreateAsync(new Picture { Id = 1 }, @"..\..\test.jpg");
            });
        }

        [Fact]
        public async Task Attachment_CreateVideo_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/videouploadticket", Http.Url.AbsoluteUri);
                return new VideoUploadTicket { Auth = "secret", UploadUrl = "http://www.example.com/test", VideoId = "xyz" };
            }).ThenWith(m =>
            {
                Assert.Equal("POST", m);
                Assert.Equal("secret", Http.Parameters.Single(p => p.Name == "auth").Value);
                Assert.Equal("http://www.example.com/test", Http.Url.AbsoluteUri);
                return "ok";
            }).ThenWith(m =>
            {
                Assert.Equal("POST", m);
                var v = new BaseXmlDeserializer().Deserialize<Attachment>(new RestResponse { Content = Http.RequestBody });
                Assert.Equal("xyz", ((StreamingVideo)v).VideoId);
                Assert.Equal("Video", ((StreamingVideo)v).Title);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment", Http.Url.AbsoluteUri);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource with id [4711] has been created.", Id = "4711" } } };
            });

            var video = new StreamingVideo { Title = "Video" };
            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.CreateStreamingVideoAsync(video, @"..\..\test.avi");
        }

        [Fact]
        public async Task Attachment_CreateVideo_HasFile()
        {
            var bytes = File.ReadAllBytes(@"..\..\test.avi");

            Http.RespondWith(m =>
            {
                return new VideoUploadTicket { Auth = "secret", UploadUrl = "http://www.example.com/test", VideoId = "xyz" };
            }).ThenWith(m =>
            {
                var file = Http.Files.Single(f => f.Name == "videofile");
                Assert.Equal("application/octet-stream", file.ContentType);
                Assert.Equal("test.avi", file.FileName);
                Assert.Equal(bytes.Length, file.ContentLength);
                var ms = new MemoryStream();
                file.Writer(ms);
                AssertEx.CollectionEqual(bytes, ms.ToArray());
                return "ok";
            }).ThenWith(m =>
            {
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource with id [4711] has been created.", Id = "4711" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.CreateStreamingVideoAsync(new StreamingVideo { Title = "Video" }, @"..\..\test.avi");
        }

        [Fact]
        public async Task Attachment_CreateVideo_CanDeserializeResponse()
        {
            Http.RespondWith(m =>
            {
                return new VideoUploadTicket { Auth = "secret", UploadUrl = "http://www.example.com/test", VideoId = "xyz" };
            }).ThenWith(m =>
            {
                return "ok";
            }).ThenWith(m =>
            {
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource with id [4711] has been created.", Id = "4711" } } };
            });

            var video = new StreamingVideo { Title = "Video" };
            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.CreateStreamingVideoAsync(video, @"..\..\test.avi");
            Assert.Equal(4711, video.Id);
            Assert.Equal("xyz", video.VideoId);
        }

        [Fact]
        public async Task Attachment_CreateVideo_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var video = new StreamingVideo { Title = "Video" };
                var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
                await re.Attachments.CreateStreamingVideoAsync(video, @"..\..\test.avi");
            });
        }

        [Fact]
        public async Task Attachment_CreateVideo_ErrorOccursAtPostVideo_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new VideoUploadTicket { Auth = "secret", UploadUrl = "http://www.example.com/test", VideoId = "xyz" };
            }).ThenWith(m =>
            {
                return new HttpStubResponse { ResponseObject = "fail", StatusCode = HttpStatusCode.BadRequest };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var video = new StreamingVideo { Title = "Video" };
                var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
                await re.Attachments.CreateStreamingVideoAsync(video, @"..\..\test.avi");
            });
        }

        [Fact]
        public async Task Attachment_CreateVideo_ErrorOccursAtPostAttachment_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new VideoUploadTicket { Auth = "secret", UploadUrl = "http://www.example.com/test", VideoId = "xyz" };
            }).ThenWith(m =>
            {
                return "ok";
            }).ThenWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var video = new StreamingVideo { Title = "Video" };
                var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
                await re.Attachments.CreateStreamingVideoAsync(video, @"..\..\test.avi");
            });
        }

        [Fact]
        public async Task AttachmentsOrder_Get_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment/attachmentsorder", Http.Url.ToString());
                return new AttachmentsOrder.List { AttachmentId = { 1, 2, 3 } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            var list = await re.Attachments.AttachmentsOrder.GetAsync();
        }

        [Fact]
        public async Task AttachmentsOrder_Get_CanDeserializeResponse()
        {
            Http.RespondWith(m =>
            {
                return new AttachmentsOrder.List { AttachmentId = { 1, 2, 3 } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 1 }, Client.Connection);
            var list = await re.Attachments.AttachmentsOrder.GetAsync();

            Assert.IsType<AttachmentsOrder.List>(list);
            AssertEx.CollectionEqual<long>(new long[] { 1, 2, 3 }, list.AttachmentId);
        }

        [Fact]
        public async Task AttachmentsOrder_Update_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("PUT", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment/attachmentsorder", Http.Url.AbsoluteUri);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.AttachmentsOrder.UpdateAsync(new AttachmentsOrder.List { AttachmentId = { 3, 2, 1 } });
        }

        [Fact]
        public async Task AttachmentsOrder_Update_CallSucceeds_NoExceptionThrown()
        {
            Http.RespondWith(m =>
            {
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.AttachmentsOrder.UpdateAsync(new AttachmentsOrder.List { AttachmentId = { 3, 2, 1 } });
        }

        [Fact]
        public async Task AttachmentsOrder_Update_PostsAttachmentObject()
        {
            Http.RespondWith(m =>
            {
                var list = new BaseXmlDeserializer().Deserialize<AttachmentsOrder.List>(new RestResponse { Content = Http.RequestBody });
                Assert.IsAssignableFrom<AttachmentsOrder.List>(list);
                AssertEx.CollectionEqual<long>(new long[] { 3, 2, 1 }, list.AttachmentId);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            await re.Attachments.AttachmentsOrder.UpdateAsync(new AttachmentsOrder.List { AttachmentId = { 3, 2, 1 } });
        }

        [Fact]
        public async Task AttachmentsOrder_Update_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
                await re.Attachments.AttachmentsOrder.UpdateAsync(new AttachmentsOrder.List { AttachmentId = { 3, 2, 1 } });
            });
        }

        [Fact]
        public async Task CalculatesCorrectHash()
        {
            var a = new Attachment();
            await a.CalculateCheckSumAsync(@"..\..\test.jpg");
            Assert.Equal("9c2210b068d609fb655f1c3423698dd1", a.ExternalCheckSum);
        }

        [Fact]
        public async Task SyncWorks()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment", Http.Url.AbsoluteUri);
                return new Attachments
                {
                    Attachment = {
                        new Picture { Id = 1, ExternalId = "Z0", Title = "Zimmer 0", ExternalCheckSum = "9c2210b068d609fb655f1c3423698dd1" },
                        new Picture { Id = 2, ExternalId = "Z2", Title = "Zimmer 2", ExternalCheckSum = "4711" },
                        new Picture { Id = 30, ExternalId = "Z3", Title = "Zimmer 3", ExternalCheckSum = "9c2210b068d609fb655f1c3423698dd1" },
                        new StreamingVideo { Id = 3, ExternalId = "572d7a1c40b15b36d1b4443a05fe2c4e", Title = "Video", ExternalCheckSum = "bb84e757201eba7d1840153179297e8a" },
                        new PDFDocument { Id = 4, ExternalId = "P1", Title = "Test", ExternalCheckSum = "24c43a4388ae2ea98322fa7016dd3274" },
                        new Link { Id = 5, ExternalId = "L1", Title = "Test", Url = "http://www.example.com/" }
                    }
                };
            }).ThenWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment/1", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "" } } };
            }).ThenWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment/2", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "" } } };
            }).ThenWith(m =>
            {
                Assert.Equal("POST", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment", Http.Url.AbsoluteUri);
                var meta = Http.Files.Single(f => f.Name == "metadata");
                Assert.Equal("application/xml", meta.ContentType);
                Assert.Equal("body.xml", meta.FileName);
                var ms = new MemoryStream();
                meta.Writer(ms);
                var bytes = ms.ToArray();
                Assert.Equal(bytes.Length, meta.ContentLength);
                var content = Encoding.UTF8.GetString(bytes);
                var a = new BaseXmlDeserializer().Deserialize<Attachment>(new RestResponse { Content = content });
                Assert.IsAssignableFrom<Attachment>(a);
                Assert.Equal("Zimmer 1", a.Title);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource with id [6] has been created.", Id = "6" } } };
            }).ThenWith(m =>
            {
                Assert.Equal("POST", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment", Http.Url.AbsoluteUri);
                var meta = Http.Files.Single(f => f.Name == "metadata");
                Assert.Equal("application/xml", meta.ContentType);
                Assert.Equal("body.xml", meta.FileName);
                var ms = new MemoryStream();
                meta.Writer(ms);
                var bytes = ms.ToArray();
                Assert.Equal(bytes.Length, meta.ContentLength);
                var content = Encoding.UTF8.GetString(bytes);
                var a = new BaseXmlDeserializer().Deserialize<Attachment>(new RestResponse { Content = content });
                Assert.IsAssignableFrom<Attachment>(a);
                Assert.Equal("Zimmer 2", a.Title);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource with id [7] has been created.", Id = "7" } } };
            }).ThenWith(m =>
            {
                Assert.Equal("PUT", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment/4", Http.Url.AbsoluteUri);
                var a = new BaseXmlDeserializer().Deserialize<Attachment>(new RestResponse { Content = Http.RequestBody });
                Assert.IsAssignableFrom<Attachment>(a);
                Assert.Equal("Test Update", a.Title);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            }).ThenWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment/attachmentsorder", Http.Url.ToString());
                return new AttachmentsOrder.List { AttachmentId = { 30, 4, 6, 7 } };
            }).ThenWith(m =>
            {
                Assert.Equal("PUT", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711/attachment/attachmentsorder", Http.Url.AbsoluteUri);
                var list = new BaseXmlDeserializer().Deserialize<AttachmentsOrder.List>(new RestResponse { Content = Http.RequestBody });
                Assert.IsAssignableFrom<AttachmentsOrder.List>(list);
                AssertEx.CollectionEqual<long>(new long[] { 6, 7, 4, 30 }, list.AttachmentId);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var re = new RealEstateItem(new ApartmentRent { Id = 4711 }, Client.Connection);
            var a1 = new KeyValuePair<Attachment, string>(new Picture { ExternalId = "Z1", Title = "Zimmer 1" }, @"..\..\test.jpg");
            var a2 = new KeyValuePair<Attachment, string>(new Picture { ExternalId = "Z2", Title = "Zimmer 2" }, @"..\..\test.jpg");
            var a3 = new KeyValuePair<Attachment, string>(new Picture { ExternalId = "Z3", Title = "Zimmer 3" }, @"..\..\test.jpg");
            var pdf = new KeyValuePair<Attachment, string>(new PDFDocument { ExternalId = "P1", Title = "Test Update" }, @"..\..\test.pdf");
            var video = new KeyValuePair<Attachment, string>(new StreamingVideo { Title = "Video" }, @"..\..\test.avi");
            var link = new KeyValuePair<Attachment, string>(new Link { Title = "Test", Url = "http://www.example.com/" }, null);
            var atts = new[] { a1, link, video, a2, pdf, a3 };

            await re.Attachments.UpdateAsync(atts);

            Assert.Equal("572d7a1c40b15b36d1b4443a05fe2c4e", video.Key.ExternalId);
            Assert.Equal(4, pdf.Key.Id);
            Assert.Equal(6, a1.Key.Id);
            Assert.Equal(7, a2.Key.Id);
            Assert.Equal(3, video.Key.Id);
            Assert.Equal(5, link.Key.Id);
            Assert.Equal("9c2210b068d609fb655f1c3423698dd1", a1.Key.ExternalCheckSum);
            Assert.Equal("9c2210b068d609fb655f1c3423698dd1", a2.Key.ExternalCheckSum);
            Assert.Equal("bb84e757201eba7d1840153179297e8a", video.Key.ExternalCheckSum);
            Assert.Equal("24c43a4388ae2ea98322fa7016dd3274", pdf.Key.ExternalCheckSum);
        }
    }
}
