using System.Reactive.Linq;
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
    public class RealEstateTests: TestBase
    {
        public RealEstateTests()
            : base(@"http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0")
        {
        }

        [Fact]
        public async Task RealEstate_Get_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711", Http.Url.ToString());
                return new ApartmentRent { title = "Test" };
            });

            var re = await Client.RealEstates.GetAsync("4711");
        }

        [Fact]
        public async Task RealEstate_Get_RequestsCorrectExternalIdPrefix()
        {
            Http.RespondWith(m =>
            {
                Assert.True(Http.Url.ToString().EndsWith("/ext-test"));
                return new ApartmentRent { title = "test" };
            });

            var re = await Client.RealEstates.GetAsync("test", isExternal: true);
        }

        [Fact]
        public async Task RealEstate_Get_ResourceDoesNotExist_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.NotFound, ResponseObject = new messages() };
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
                return new ApartmentRent { title = "test" };
            });

            var re = await Client.RealEstates.GetAsync("test");

            Assert.IsType<ApartmentRent>(re.RealEstate);
            Assert.Equal("test", re.RealEstate.title);
        }

        [Fact]
        public async Task RealEstate_Create_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("POST", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate", Http.Url.ToString());
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_CREATED, message = "RealEstate with id [4711] has been created." } } };
            });

            var re = new ApartmentRent { title = "Test" };

            await Client.RealEstates.CreateAsync(re);
        }

        [Fact]
        public async Task RealEstate_Create_CallSucceeds_RealEstateObjectHasNewId()
        {
            Http.RespondWith(m =>
            {
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_CREATED, message = "RealEstate with id [4711] has been created." } } };
            });

            var re = new ApartmentRent { title = "Test" };

            await Client.RealEstates.CreateAsync(re);

            Assert.Equal(4711, re.id);
        }

        [Fact]
        public async Task RealEstate_Create_PostsRealEstateObject()
        {
            Http.RespondWith(m =>
            {
                var re = new BaseXmlDeserializer().Deserialize<RealEstate>(new RestResponse { Content = Http.RequestBody });
                Assert.IsType<ApartmentRent>(re);
                Assert.Equal("Test", re.title);
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_CREATED, message = "RealEstate with id [4711] has been created." } } };
            });

            var r = new ApartmentRent { title = "Test" };

            await Client.RealEstates.CreateAsync(r);
        }

        [Fact]
        public async Task RealEstate_Create_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new messages() };
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
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, message = "" } } };
            });

            var re = new ApartmentRent { id = 4711, title = "Test" };

            await Client.RealEstates.UpdateAsync(re);
        }

        [Fact]
        public async Task RealEstate_Create_CallSucceeds_NoExceptionThrown()
        {
            Http.RespondWith(m =>
            {
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, message = "" } } };
            });

            var re = new ApartmentRent { id = 4711, title = "Test" };

            await Client.RealEstates.UpdateAsync(re);
        }

        [Fact]
        public async Task RealEstate_Update_PostsRealEstateObject()
        {
            Http.RespondWith(m =>
            {
                var re = new BaseXmlDeserializer().Deserialize<RealEstate>(new RestResponse { Content = Http.RequestBody });
                Assert.IsType<ApartmentRent>(re);
                Assert.Equal(4711, re.id);
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_UPDATED, message = "" } } };
            });

            var r = new ApartmentRent { id = 4711, idSpecified = true, title = "Test" };

            await Client.RealEstates.UpdateAsync(r);
        }

        [Fact]
        public async Task RealEstate_Update_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.PreconditionFailed, ResponseObject = new messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.RealEstates.UpdateAsync(new ApartmentRent());
            });
        }

        [Fact]
        public async Task RealEstate_Delete_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711", Http.Url.ToString());
                return new messages { message = new[] { new Message { messageCode = MessageCode.MESSAGE_RESOURCE_DELETED, message = "" } } };
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
                return new realEstates { realEstateList = new OfferRealEstateForList[] { }, Paging = new Paging { numberOfPages = 1, numberOfPagesSpecified = true } };
            });

            var res = Client.RealEstates.GetAsync().ToEnumerable().ToList();
        }

        [Fact]
        public void RealEstate_GetAll_RequestsCorrectPages()
        {
            Http.RespondWith(m =>
            {
                return new realEstates { realEstateList = new OfferRealEstateForList[] { new OfferApartmentRent { id = 4711, idSpecified = true } }, 
                    Paging = new Paging { numberOfPages = 2, numberOfPagesSpecified = true } };
            }).ThenWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4711", Http.Url.ToString());
                return new ApartmentRent { id = 4711, idSpecified = true, title = "Test 1" };
            }).ThenWith(m =>
            {
                Assert.Equal(2, int.Parse(Http.Parameters.Single(p => p.Name == "pagenumber").Value));
                return new realEstates { realEstateList = new OfferRealEstateForList[] { new OfferApartmentRent { id = 4712, idSpecified = true } }, 
                    Paging = new Paging { numberOfPages = 2, numberOfPagesSpecified = true } };
            }).ThenWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/4712", Http.Url.ToString());
                return new ApartmentRent { id = 4712, idSpecified = true, title = "Test 2" };
            }).ThenWith(m =>
            {
                Assert.True(false, "Must not request more pages than available.");
                return new messages { message = new[] { new Message { message = "fail", messageCode = MessageCode.ERROR_COMMON_RESOURCE_NOT_FOUND } } };
            });

            var res = Client.RealEstates.GetAsync().ToEnumerable().ToList();

            Assert.Equal(2, res.Count);
            Assert.Equal(4711, res[0].RealEstate.id);
            Assert.Equal(4712, res[1].RealEstate.id);
        }
    }
}
