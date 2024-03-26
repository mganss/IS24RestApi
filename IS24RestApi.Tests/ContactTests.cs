using IS24RestApi.Common;
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
    public class ContactTests : ImportExportTestBase
    {
        public ContactTests()
            : base(@"https://rest.sandbox-immobilienscout24.de/restapi/api")
        { }

        [Fact]
        public async Task Contact_Get_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.Get, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/contact/4711", RestClient.BuildUri(r).ToString());
                return new RealtorContactDetails { Id = 4711 };
            });

            var contact = await Client.Contacts.GetAsync("4711");
        }

        [Fact]
        public async Task Contact_Get_RequestsCorrectExternalIdPrefix()
        {
            RestClient.RespondWith(r =>
            {
                Assert.EndsWith("/ext-Hans%20Meiser", RestClient.BuildUri(r).AbsoluteUri);
                return new RealtorContactDetails { ExternalId = "Hans Meiser", Id = 4711 };
            });

            var contact = await Client.Contacts.GetAsync("Hans Meiser", isExternal: true);
        }

        [Fact]
        public async Task Contact_Get_ResourceDoesNotExist_ThrowsIS24Exception()
        {
            RestClient.RespondWith(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.NotFound, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var contact = await Client.Contacts.GetAsync("test");
            });
        }

        [Fact]
        public async Task Contact_Get_CanDeserializeResponse()
        {
            RestClient.RespondWith(r =>
            {
                return new RealtorContactDetails { Id = 4711 };
            });

            var contact = await Client.Contacts.GetAsync("4711");

            Assert.IsType<RealtorContactDetails>(contact);
            Assert.Equal(4711, contact.Id);
        }

        [Fact]
        public async Task Contact_Create_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.Post, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/contact", RestClient.BuildUri(r).ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Contact with id [4711] has been created.", Id = "4711" } } };
            });

            var contact = new RealtorContactDetails { Lastname = "Meiser" };

            await Client.Contacts.CreateAsync(contact);
        }

        [Fact]
        public async Task Contact_Create_CallSucceeds_RealEstateObjectHasNewId()
        {
            RestClient.RespondWith(r =>
            {
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Contact with id [4711] has been created.", Id = "4711" } } };
            });

            var contact = new RealtorContactDetails { Lastname = "Meiser" };

            await Client.Contacts.CreateAsync(contact);

            Assert.Equal(4711, contact.Id);
        }

        [Fact]
        public async Task Contact_Create_PostsRealtorObject()
        {
            RestClient.RespondWith(r =>
            {
                var c = r.Body() as RealtorContactDetails;
                Assert.IsType<RealtorContactDetails>(c);
                Assert.Equal("Meiser", c.Lastname);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Contact with id [4711] has been created.", Id = "4711" } } };
            });

            var contact = new RealtorContactDetails { Lastname = "Meiser" };

            await Client.Contacts.CreateAsync(contact);
        }

        [Fact]
        public async Task Contact_Create_ErrorOccurs_ThrowsIS24Exception()
        {
            RestClient.RespondWith(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.Contacts.CreateAsync(new RealtorContactDetails());
            });
        }

        [Fact]
        public async Task Contact_Update_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.Put, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/contact/4711", RestClient.BuildUri(r).ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var contact = new RealtorContactDetails { Id = 4711 };

            await Client.Contacts.UpdateAsync(contact);
        }

        [Fact]
        public async Task Contact_Create_CallSucceeds_NoExceptionThrown()
        {
            RestClient.RespondWith(r =>
            {
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var contact = new RealtorContactDetails { Id = 4711 };

            await Client.Contacts.UpdateAsync(contact);
        }

        [Fact]
        public async Task Contact_Update_PostsRealEstateObject()
        {
            RestClient.RespondWith(r =>
            {
                var c = r.Body() as RealtorContactDetails;
                Assert.IsType<RealtorContactDetails>(c);
                Assert.Equal(4711, c.Id);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var contact = new RealtorContactDetails { Id = 4711 };

            await Client.Contacts.UpdateAsync(contact);
        }

        [Fact]
        public async Task Contact_Update_ExternalIdIsUsedIfIdIsNull()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/contact/ext-test", RestClient.BuildUri(r).ToString());
                var c = r.Body() as RealtorContactDetails;
                Assert.IsType<RealtorContactDetails>(c);
                Assert.Null(c.Id);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var contact = new RealtorContactDetails { ExternalId = "test" };

            await Client.Contacts.UpdateAsync(contact);
        }

        [Fact]
        public async Task Contact_Update_ErrorOccurs_ThrowsIS24Exception()
        {
            RestClient.RespondWith(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.Contacts.UpdateAsync(new RealtorContactDetails { Id = 1 });
            });
        }

        [Fact]
        public async Task Contact_GetAll_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.Get, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/contact", RestClient.BuildUri(r).ToString());
                return new RealtorContactDetailsList { RealtorContactDetails = { } };
            });

            var cs = await Client.Contacts.GetAsync();
        }

        [Fact]
        public async Task Contact_GetAll_CanDeserializeResponse()
        {
            RestClient.RespondWith(r =>
            {
                return new RealtorContactDetailsList
                {
                    RealtorContactDetails = {
                        new RealtorContactDetails { Id = 4711 },
                        new RealtorContactDetails { Id = 4712 },
                    }
                };
            });

            var cs = (await Client.Contacts.GetAsync()).ToList();

            Assert.Equal(2, cs.Count);
            Assert.Equal(4711, cs[0].Id);
            Assert.Equal(4712, cs[1].Id);
        }

        [Fact]
        public async Task Contact_Delete_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.Delete, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/contact/4711", RestClient.BuildUri(r).ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "" } } };
            });

            await Client.Contacts.DeleteAsync("4711");
        }

        [Fact]
        public async Task Contact_DeleteAssignToContact_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.Delete, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/contact/4711?assigntocontactid=4712", RestClient.BuildUri(r).ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "" } } };
            });

            await Client.Contacts.DeleteAsync("4711", "4712");
        }
    }
}
