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
    public class RealEstateTests: ImportExportTestBase
    {
        public RealEstateTests()
            : base(@"http://rest.sandbox-immobilienscout24.de/restapi/api")
        {
        }

        [Fact]
        public async Task RealEstate_Get_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711", Http.Url.ToString());
                return new ApartmentRent { Title = "Test" };
            });

            var re = await Client.RealEstates.GetAsync("4711");
        }

        [Fact]
        public async Task RealEstate_Get_RequestsCorrectExternalIdPrefix()
        {
            Http.RespondWith(m =>
            {
                Assert.True(Http.Url.ToString().EndsWith("/ext-test"));
                return new ApartmentRent { Title = "test" };
            });

            var re = await Client.RealEstates.GetAsync("test", isExternal: true);
        }

        [Fact]
        public async Task RealEstate_Get_ResourceDoesNotExist_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.NotFound, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                var re = await Client.RealEstates.GetAsync("test");
            });
        }

        [Fact]
        public async Task RealEstate_Get_CanDeserializeResponse()
        {
            Http.RespondWith(m =>
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
            Http.RespondWith(m =>
            {
                Assert.Equal("POST", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "RealEstate with id [4711] has been created.", Id = "4711" } } };
            });

            var re = new ApartmentRent { Title = "Test" };

            await Client.RealEstates.CreateAsync(re);
        }

        [Fact]
        public async Task RealEstate_Create_CallSucceeds_RealEstateObjectHasNewId()
        {
            Http.RespondWith(m =>
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
            Http.RespondWith(m =>
            {
                var re = new BaseXmlDeserializer().Deserialize<RealEstate>(new RestResponse { Content = Http.RequestBody });
                Assert.IsType<ApartmentRent>(re);
                Assert.Equal("Test", re.Title);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "RealEstate with id [4711] has been created.", Id = "4711" } } };
            });

            var r = new ApartmentRent { Title = "Test" };

            await Client.RealEstates.CreateAsync(r);
        }

        [Fact]
        public async Task RealEstate_Create_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.RealEstates.CreateAsync(new ApartmentRent());
            });
        }

        [Fact]
        public async Task RealEstate_Update_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("PUT", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var re = new ApartmentRent { Id = 4711, Title = "Test" };

            await Client.RealEstates.UpdateAsync(re);
        }

        [Fact]
        public async Task RealEstate_Create_CallSucceeds_NoExceptionThrown()
        {
            Http.RespondWith(m =>
            {
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var re = new ApartmentRent { Id = 4711, Title = "Test" };

            await Client.RealEstates.UpdateAsync(re);
        }

        [Fact]
        public async Task RealEstate_Update_PostsRealEstateObject()
        {
            Http.RespondWith(m =>
            {
                var re = new BaseXmlDeserializer().Deserialize<RealEstate>(new RestResponse { Content = Http.RequestBody });
                Assert.IsType<ApartmentRent>(re);
                Assert.Equal(4711, re.Id);
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, MessageProperty = "" } } };
            });

            var r = new ApartmentRent { Id = 4711, Title = "Test" };

            await Client.RealEstates.UpdateAsync(r);
        }

        [Fact]
        public async Task RealEstate_Update_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.RealEstates.UpdateAsync(new ApartmentRent { Id = 1 });
            });
        }

        [Fact]
        public async Task RealEstate_Delete_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "" } } };
            });

            await Client.RealEstates.DeleteAsync("4711");
        }

        [Fact]
        public void RealEstate_GetAll_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal(1, int.Parse(Http.Parameters.Single(p => p.Name == "pagenumber").Value));
                Assert.InRange(int.Parse(Http.Parameters.Single(p => p.Name == "pagesize").Value), 1, 100);
                var url = Http.Url.GetLeftPart(UriPartial.Path);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate", url);
                return new RealEstates { RealEstateList = { }, Paging = new Paging { NumberOfPages = 1 } };
            });

            var res = Client.RealEstates.GetAsync().ToEnumerable().ToList();
        }

        [Fact]
        public void RealEstate_GetAll_RequestsCorrectPages()
        {
            Http.RespondWith(m =>
            {
                return new RealEstates
                {
                    RealEstateList = { new OfferApartmentRent { Id = 4711 } },
                    Paging = new Paging { NumberOfPages = 2 }
                };
            }).ThenWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711", Http.Url.ToString());
                return new ApartmentRent { Id = 4711, Title = "Test 1" };
            }).ThenWith(m =>
            {
                Assert.Equal(2, int.Parse(Http.Parameters.Single(p => p.Name == "pagenumber").Value));
                return new RealEstates
                {
                    RealEstateList = { new OfferApartmentRent { Id = 4712 } },
                    Paging = new Paging { NumberOfPages = 2 }
                };
            }).ThenWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4712", Http.Url.ToString());
                return new ApartmentRent { Id = 4712, Title = "Test 2" };
            }).ThenWith(m =>
            {
                Assert.True(false, "Must not request more pages than available.");
                return new Messages { Message = { new Message { MessageProperty = "fail", MessageCode = MessageCode.ERROR_COMMON_RESOURCE_NOT_FOUND } } };
            });

            var res = Client.RealEstates.GetAsync().ToEnumerable().ToList();

            Assert.Equal(2, res.Count);
            Assert.Equal(4711, res[0].RealEstate.Id);
            Assert.Equal(4712, res[1].RealEstate.Id);
        }
    }
}
