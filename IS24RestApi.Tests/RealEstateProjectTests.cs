using IS24RestApi.Common;
using IS24RestApi.Offer.RealEstateProject;
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
    public class RealEstateProjectTests: ImportExportTestBase
    {
        public RealEstateProjectTests()
            : base(@"https://rest.sandbox-immobilienscout24.de/restapi/api")
        { }

        [Fact]
        public async Task RealEstateProject_GetAllProjects_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.GET, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject", RestClient.BuildUri(r).ToString());
                return new RealEstateProjects { RealEstateProject = { new RealEstateProject { Id = 4711 } } };
            });

            var RealEstateProject = await Client.RealEstateProjects.GetAllAsync();
        }

        [Fact]
        public async Task RealEstateProject_Get_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.GET, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject/4711", RestClient.BuildUri(r).ToString());
                return new RealEstateProject { Id = 4711 };
            });

            var RealEstateProject = await Client.RealEstateProjects.GetAsync(4711);
        }

        [Fact]
        public async Task RealEstateProject_Get_ResourceDoesNotExist_ThrowsIS24Exception()
        {
            RestClient.RespondWith(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.NotFound, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var project = await Client.RealEstateProjects.GetAsync(4711);
            });
        }

        [Fact]
        public async Task RealEstateProject_Get_CanDeserializeResponse()
        {
            RestClient.RespondWith(r =>
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
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.PUT, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject/4711", RestClient.BuildUri(r).ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var project = new RealEstateProject { Id = 4711 };

            await Client.RealEstateProjects.UpdateAsync(project);
        }

        [Fact]
        public async Task RealEstateProject_Update_PostsRealEstateObject()
        {
            RestClient.RespondWith(r =>
            {
                var c = r.Body() as RealEstateProject;
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
            RestClient.RespondWith(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.RealEstateProjects.UpdateAsync(new RealEstateProject { Id = 1 });
            });
        }

        [Fact]
        public async Task RealEstateProject_Create_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.POST, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject/4711/realestateprojectentry", RestClient.BuildUri(r).ToString());
                return new RealEstateProjectEntries();
            });

            var entries = new RealEstateProjectEntries();

            await Client.RealEstateProjects.AddAsync(4711, entries);
        }

        [Fact]
        public async Task RealEstateProject_Create_CallSucceeds()
        {
            RestClient.RespondWith(r =>
            {
                var e = r.Body() as RealEstateProjectEntries;
                Assert.IsType<RealEstateProjectEntries>(e);
                Assert.Single(e.RealEstateProjectEntry);
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
            RestClient.RespondWith(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.RealEstateProjects.AddAsync(4711, new RealEstateProjectEntries());
            });
        }

        [Fact]
        public async Task RealEstateProject_GetAll_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.GET, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject/4711/realestateprojectentry", RestClient.BuildUri(r).ToString());
                return new RealEstateProjectEntries();
            });

            await Client.RealEstateProjects.GetAllAsync(4711);
        }

        [Fact]
        public async Task RealEstateProject_GetAll_CallSucceeds()
        {
            RestClient.RespondWith(r =>
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
            RestClient.RespondWith(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.RealEstateProjects.GetAllAsync(4711);
            });
        }

        [Fact]
        public async Task RealEstateProject_Remove_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.DELETE, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject/4711/realestateprojectentry/1", RestClient.BuildUri(r).ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "" } } };
            }).ThenWith(r =>
            {
                Assert.Equal(Method.DELETE, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject/4711/realestateprojectentry/ext-test", RestClient.BuildUri(r).ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "" } } };
            }).ThenWith(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
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
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.DELETE, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestateproject/4711/realestateprojectentry", RestClient.BuildUri(r).ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "" } } };
            }).ThenWith(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await Client.RealEstateProjects.RemoveAsync(4711);
            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.RealEstateProjects.RemoveAsync(4712);
            });
        }
    }
}
