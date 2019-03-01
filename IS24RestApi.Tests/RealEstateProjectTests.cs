using IS24RestApi.Common;
using IS24RestApi.Offer.RealEstateProject;
using IS24RestApi.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IS24RestApi.Tests
{
    public class RealEstateProjectTests: ImportExportTestBase
    {
        public RealEstateProjectTests()
            : base(@"https://rest.sandbox-immobilienscout24.de/restapi/api")
        { }

        [Fact]
        public async Task RealEstateProject_GetAllProjects_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject", Http.Url.ToString());
                return new RealEstateProjects { RealEstateProject = { new RealEstateProject { Id = 4711 } } };
            });

            var RealEstateProject = await Client.RealEstateProjects.GetAllAsync();
        }

        [Fact]
        public async Task RealEstateProject_Get_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject/4711", Http.Url.ToString());
                return new RealEstateProject { Id = 4711 };
            });

            var RealEstateProject = await Client.RealEstateProjects.GetAsync(4711);
        }

        [Fact]
        public async Task RealEstateProject_Get_ResourceDoesNotExist_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.NotFound, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var project = await Client.RealEstateProjects.GetAsync(4711);
            });
        }

        [Fact]
        public async Task RealEstateProject_Get_CanDeserializeResponse()
        {
            Http.RespondWith(m =>
            {
                return new RealEstateProject { Id = 4711 };
            });

            var project = await Client.RealEstateProjects.GetAsync(4711);

            Assert.IsType<RealEstateProject>(project);
            Assert.Equal(4711, project.Id);
        }

        [Fact]
        public async Task RealEstateProject_Update_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("PUT", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject/4711", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var project = new RealEstateProject { Id = 4711 };

            await Client.RealEstateProjects.UpdateAsync(project);
        }

        [Fact]
        public async Task RealEstateProject_Update_PostsRealEstateObject()
        {
            Http.RespondWith(m =>
            {
                var c = new BaseXmlDeserializer().Deserialize<RealEstateProject>(new RestResponse { Content = Http.RequestBody });
                Assert.IsType<RealEstateProject>(c);
                Assert.Equal(4711, c.Id);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var project = new RealEstateProject { Id = 4711 };

            await Client.RealEstateProjects.UpdateAsync(project);
        }

        [Fact]
        public async Task RealEstateProject_Update_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.RealEstateProjects.UpdateAsync(new RealEstateProject { Id = 1 });
            });
        }

        [Fact]
        public async Task RealEstateProject_Create_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("POST", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject/4711/realestateprojectentry", Http.Url.ToString());
                return new RealEstateProjectEntries();
            });

            var entries = new RealEstateProjectEntries();

            await Client.RealEstateProjects.AddAsync(4711, entries);
        }

        [Fact]
        public async Task RealEstateProject_Create_CallSucceeds()
        {
            Http.RespondWith(m =>
            {
                var e = new BaseXmlDeserializer().Deserialize<RealEstateProjectEntries>(new RestResponse { Content = Http.RequestBody });
                Assert.IsType<RealEstateProjectEntries>(e);
                Assert.Equal(1, e.RealEstateProjectEntry.Count);
                Assert.Equal(1, e.RealEstateProjectEntry.Single().RealEstateId);

                return new RealEstateProjectEntries
                {
                    RealEstateProjectEntry = { new RealEstateProjectEntry { RealEstateId = 1,
                        MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, Message = "real estate with id 1 was added to project 4711"  } }
                };
            });

            var entries = new RealEstateProjectEntries { RealEstateProjectEntry = { new RealEstateProjectEntry { RealEstateId = 1 } } };
            var result = await Client.RealEstateProjects.AddAsync(4711, entries);

            Assert.Equal(MessageCode.MESSAGE_RESOURCE_CREATED, result.RealEstateProjectEntry.Single().MessageObject.MessageCode);
            Assert.Equal(1, result.RealEstateProjectEntry.Single().RealEstateId);
        }

        [Fact]
        public async Task RealEstateProject_Create_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.RealEstateProjects.AddAsync(4711, new RealEstateProjectEntries());
            });
        }

        [Fact]
        public async Task RealEstateProject_GetAll_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject/4711/realestateprojectentry", Http.Url.ToString());
                return new RealEstateProjectEntries();
            });

            await Client.RealEstateProjects.GetAllAsync(4711);
        }

        [Fact]
        public async Task RealEstateProject_GetAll_CallSucceeds()
        {
            Http.RespondWith(m =>
            {
                return new RealEstateProjectEntries
                {
                    RealEstateProjectEntry = { new RealEstateProjectEntry { RealEstateId = 1 } }
                };
            });

            var result = await Client.RealEstateProjects.GetAllAsync(4711);

            Assert.Equal(1, result.RealEstateProjectEntry.Single().RealEstateId);
        }

        [Fact]
        public async Task RealEstateProject_GetAll_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.RealEstateProjects.GetAllAsync(4711);
            });
        }

        [Fact]
        public async Task RealEstateProject_Remove_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject/4711/realestateprojectentry/1", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "" } } };
            }).ThenWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject/4711/realestateprojectentry/ext-test", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "" } } };
            }).ThenWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await Client.RealEstateProjects.RemoveAsync(4711, "1");
            await Client.RealEstateProjects.RemoveAsync(4711, "test", true);
            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.RealEstateProjects.RemoveAsync(4711, "2");
            });
        }

        [Fact]
        public async Task RealEstateProject_RemoveAll_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject/4711/realestateprojectentry", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "" } } };
            }).ThenWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await Client.RealEstateProjects.RemoveAsync(4711);
            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.RealEstateProjects.RemoveAsync(4712);
            });
        }
    }
}
