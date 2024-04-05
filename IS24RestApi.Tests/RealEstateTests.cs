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
    public class RealEstateTests : ImportExportTestBase
    {
        public RealEstateTests()
            : base(@"https://rest.sandbox-immobilienscout24.de/restapi/api")
        {
        }

        [Fact]
        public async Task RealEstate_Get_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.Get, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711", RestClient.BuildUri(r).GetLeftPart(UriPartial.Path));
                Assert.Equal("?usenewenergysourceenev2014values=true", RestClient.BuildUri(r).Query);
                return new ApartmentRent { Title = "Test" };
            });

            var re = await Client.RealEstates.GetAsync("4711");
        }

        [Fact]
        public async Task RealEstate_Get_RequestsCorrectExternalIdPrefix()
        {
            RestClient.RespondWith(r =>
            {
                Assert.EndsWith("/ext-test", RestClient.BuildUri(r).AbsolutePath);
                return new ApartmentRent { Title = "test" };
            });

            var re = await Client.RealEstates.GetAsync("test", isExternal: true);
        }

        [Fact]
        public async Task RealEstate_Get_ResourceDoesNotExist_ThrowsIS24Exception()
        {
            RestClient.RespondWith(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.NotFound, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var re = await Client.RealEstates.GetAsync("test");
            });
        }

        [Fact]
        public async Task RealEstate_Get_CanDeserializeResponse()
        {
            RestClient.RespondWith(r =>
            {
                return new ApartmentRent { Title = "test" };
            });

            var re = await Client.RealEstates.GetAsync("test");

            Assert.IsType<ApartmentRent>(re.RealEstate);
            Assert.Equal("test", re.RealEstate.Title);
        }

        [Fact]
        public async Task RealEstate_Create_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.Post, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate", RestClient.BuildUri(r).GetLeftPart(UriPartial.Path));
                Assert.Equal("?usenewenergysourceenev2014values=true", RestClient.BuildUri(r).Query);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "RealEstate with id [4711] has been created.", Id = "4711" } } };
            });

            var re = new ApartmentRent { Title = "Test" };

            await Client.RealEstates.CreateAsync(re);
        }

        [Fact]
        public async Task RealEstate_Create_CallSucceeds_RealEstateObjectHasNewId()
        {
            RestClient.RespondWith(r =>
            {
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "RealEstate with id [4711] has been created.", Id = "4711" } } };
            });

            var re = new ApartmentRent { Title = "Test" };

            await Client.RealEstates.CreateAsync(re);

            Assert.Equal(4711, re.Id);
        }

        [Fact]
        public async Task RealEstate_Create_PostsRealEstateObject()
        {
            RestClient.RespondWith(r =>
            {
                var re = r.Parameters.Single(p => p.Type == ParameterType.RequestBody).Value as RealEstate;
                Assert.IsType<ApartmentRent>(re);
                Assert.Equal("Test", re.Title);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "RealEstate with id [4711] has been created.", Id = "4711" } } };
            });

            var a = new ApartmentRent { Title = "Test" };

            await Client.RealEstates.CreateAsync(a);
        }

        [Fact]
        public async Task RealEstate_Create_ErrorOccurs_ThrowsIS24Exception()
        {
            RestClient.RespondWith(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.RealEstates.CreateAsync(new ApartmentRent());
            });
        }

        [Fact]
        public async Task RealEstate_Update_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.Put, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711", RestClient.BuildUri(r).GetLeftPart(UriPartial.Path));
                Assert.Equal("?usenewenergysourceenev2014values=true", RestClient.BuildUri(r).Query);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var re = new ApartmentRent { Id = 4711, Title = "Test" };

            await Client.RealEstates.UpdateAsync(re);
        }

        [Fact]
        public async Task RealEstate_Create_CallSucceeds_NoExceptionThrown()
        {
            RestClient.RespondWith(r =>
            {
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var re = new ApartmentRent { Id = 4711, Title = "Test" };

            await Client.RealEstates.UpdateAsync(re);
        }

        [Fact]
        public async Task RealEstate_Update_PostsRealEstateObject()
        {
            RestClient.RespondWith(r =>
            {
                var re = r.Body() as RealEstate;
                Assert.IsType<ApartmentRent>(re);
                Assert.Equal(4711, re.Id);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var a = new ApartmentRent { Id = 4711, Title = "Test" };

            await Client.RealEstates.UpdateAsync(a);
        }

        [Fact]
        public async Task RealEstate_Update_ErrorOccurs_ThrowsIS24Exception()
        {
            RestClient.RespondWith(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.RealEstates.UpdateAsync(new ApartmentRent { Id = 1 });
            });
        }

        [Fact]
        public async Task RealEstate_Delete_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.Delete, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711", RestClient.BuildUri(r).ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "" } } };
            });

            await Client.RealEstates.DeleteAsync("4711");
        }

        [Fact]
        public void RealEstate_GetAll_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.Get, r.Method);
                Assert.Equal(1, int.Parse(r.Parameters.Single(p => p.Name == "pagenumber").Value.ToString()));
                Assert.InRange(int.Parse(r.Parameters.Single(p => p.Name == "pagesize").Value.ToString()), 1, 100);
                var url = RestClient.BuildUri(r).GetLeftPart(UriPartial.Path);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate", url);
                return new RealEstates { RealEstateList = { }, Paging = new Paging { NumberOfPages = 1 } };
            });

            var res = Client.RealEstates.GetAsync().ToEnumerable().ToList();
        }

        [Fact]
        public void RealEstate_GetAll_RequestsCorrectPages()
        {
            RestClient.RespondWith(r =>
            {
                return new RealEstates
                {
                    RealEstateList = { new OfferApartmentRent { Id = 4711 } },
                    Paging = new Paging { NumberOfPages = 2 }
                };
            }).ThenWith(r =>
            {
                Assert.Equal(Method.Get, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711", RestClient.BuildUri(r).GetLeftPart(UriPartial.Path));
                Assert.Equal("?usenewenergysourceenev2014values=true", RestClient.BuildUri(r).Query);
                return new ApartmentRent { Id = 4711, Title = "Test 1" };
            }).ThenWith(r =>
            {
                Assert.Equal(2, int.Parse(r.Parameters.Single(p => p.Name == "pagenumber").Value.ToString()));
                return new RealEstates
                {
                    RealEstateList = { new OfferApartmentRent { Id = 4712 } },
                    Paging = new Paging { NumberOfPages = 2 }
                };
            }).ThenWith(r =>
            {
                Assert.Equal(Method.Get, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4712", RestClient.BuildUri(r).GetLeftPart(UriPartial.Path));
                Assert.Equal("?usenewenergysourceenev2014values=true", RestClient.BuildUri(r).Query);
                return new ApartmentRent { Id = 4712, Title = "Test 2" };
            }).ThenWith(r =>
            {
                Assert.True(false, "Must not request more pages than available.");
                return new Messages { Message = { new Message { MessageProperty = "fail", MessageCode = MessageCode.ERROR_COMMON_RESOURCE_NOT_FOUND } } };
            });

            var res = Client.RealEstates.GetAsync().ToEnumerable().ToList();

            Assert.Equal(2, res.Count);
            Assert.Equal(4711, res[0].RealEstate.Id);
            Assert.Equal(4712, res[1].RealEstate.Id);
        }

        [Fact]
        public void RealEstate_GetSummaries_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.Get, r.Method);
                Assert.Equal(1, int.Parse(r.Parameters.Single(p => p.Name == "pagenumber").Value.ToString()));
                Assert.InRange(int.Parse(r.Parameters.Single(p => p.Name == "pagesize").Value.ToString()), 1, 100);
                var url = RestClient.BuildUri(r).GetLeftPart(UriPartial.Path);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate", url);
                return new RealEstates { RealEstateList = { }, Paging = new Paging { NumberOfPages = 1 } };
            });

            var res = Client.RealEstates.GetSummariesAsync().ToEnumerable().ToList();
        }

        [Fact]
        public void RealEstate_GetSummaries_RequestsCorrectPages()
        {
            RestClient.RespondWith(r =>
            {
                return new RealEstates
                {
                    RealEstateList = { new OfferApartmentRent { Id = 4711 } },
                    Paging = new Paging { NumberOfPages = 2 }
                };
            }).ThenWith(r =>
            {
                Assert.Equal(2, int.Parse(r.Parameters.Single(p => p.Name == "pagenumber").Value.ToString()));
                return new RealEstates
                {
                    RealEstateList = { new OfferApartmentRent { Id = 4712 } },
                    Paging = new Paging { NumberOfPages = 2 }
                };
            }).ThenWith(r =>
            {
                Assert.True(false, "Must not request more pages than available.");
                return new Messages { Message = { new Message { MessageProperty = "fail", MessageCode = MessageCode.ERROR_COMMON_RESOURCE_NOT_FOUND } } };
            });

            var res = Client.RealEstates.GetSummariesAsync().ToEnumerable().ToList();

            Assert.Equal(2, res.Count);
            Assert.Equal(4711, res[0].Id);
            Assert.Equal(4712, res[1].Id);
        }
    }
}
