using IS24RestApi.Common;
using IS24RestApi.Offer.TopPlacement;
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
    public class OnTopPlacementTests : ImportExportTestBase
    {
        public OnTopPlacementTests()
            : base(@"http://rest.sandbox-immobilienscout24.de/restapi/api")
        { }

        [Fact]
        public async Task OnTopPlacement_Create_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("POST", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/topplacement/list", Http.Url.ToString());
                return new Topplacements();
            });

            var placements = await Client.TopPlacements.CreateAsync(new Topplacements { Topplacement = { new Topplacement { Realestateid = "1" } } });
        }

        [Fact]
        public async Task OnTopPlacement_Create_CallSucceeds()
        {
            Http.RespondWith(m =>
            {
                var e = new BaseXmlDeserializer().Deserialize<Topplacements>(new RestResponse { Content = Http.RequestBody });
                Assert.IsType<Topplacements>(e);
                Assert.Equal(1, e.Topplacement.Count);
                Assert.Equal("1", e.Topplacement.Single().Realestateid);

                return new Topplacements
                {
                    Topplacement = { new Topplacement { Realestateid = "1",
                        MessageCode = MessageCode.MESSAGE_OPERATION_SUCCESSFUL, Message = "top placed"  } }
                };
            });

            var placements = new Topplacements { Topplacement = { new Topplacement { Realestateid = "1" } } };
            var result = await Client.TopPlacements.CreateAsync(placements);

            Assert.Equal(MessageCode.MESSAGE_OPERATION_SUCCESSFUL, result.Topplacement.Single().MessageObject.MessageCode);
            Assert.Equal("1", result.Topplacement.Single().Realestateid);
        }

        [Fact]
        public async Task OnTopPlacement_CreateWithId_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("POST", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/1/topplacement", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource topplacement with id 4711 has been created." } } };
            }).ThenWith(m =>
            {
                Assert.Equal("POST", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/ext-2/topplacement", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource topplacement with id 4712 has been created." } } };
            });

            await Client.TopPlacements.CreateAsync("1");
            await Client.TopPlacements.CreateAsync("2", isExternal: true);
        }

        [Fact]
        public async Task OnTopPlacement_CreateWithId_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("POST", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/1/topplacement", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.ERROR_RESOURCE_VALIDATION, MessageProperty = "Error while validating input for the resource. [MESSAGE: topplacement for this real estate is not possible:Not allowed to perform ontop for unpublished realestate (#1).]" } } };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.TopPlacements.CreateAsync("1");
            });
        }

        [Fact]
        public async Task OnTopPlacement_GetAll_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/topplacement/all", Http.Url.ToString());
                return new Topplacements();
            });

            await Client.TopPlacements.GetAllAsync();
        }

        [Fact]
        public async Task OnTopPlacement_GetAll_CallSucceeds()
        {
            Http.RespondWith(m =>
            {
                return new Topplacements
                {
                    Topplacement = 
                    { 
                        new Topplacement 
                        { 
                            Realestateid = "1", 
                            MessageCode = MessageCode.MESSAGE_OPERATION_SUCCESSFUL,
                            Message = "toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 12, 31) },
                            ExternalId = "ext1"
                        },
                        new Topplacement 
                        { 
                            Realestateid = "2", 
                            MessageCode = MessageCode.MESSAGE_OPERATION_SUCCESSFUL,
                            Message = "toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 12, 31) },
                            ExternalId = "ext2"
                        }
                    }
                };
            });

            var result = await Client.TopPlacements.GetAllAsync();
            var expected = new Topplacements
            {
                Topplacement = 
                    { 
                        new Topplacement 
                        { 
                            Realestateid = "1", 
                            MessageCode = MessageCode.MESSAGE_OPERATION_SUCCESSFUL,
                            Message = "toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 12, 31) },
                            ExternalId = "ext1"
                        },
                        new Topplacement 
                        { 
                            Realestateid = "2", 
                            MessageCode = MessageCode.MESSAGE_OPERATION_SUCCESSFUL,
                            Message = "toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 12, 31) },
                            ExternalId = "ext2"
                        }
                    }
            };

            AssertEx.Equal(expected, result);
        }

        [Fact]
        public async Task OnTopPlacement_Get_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/1/topplacement", Http.Url.ToString());
                return new Topplacements();
            }).ThenWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/ext-2/topplacement", Http.Url.ToString());
                return new Topplacements();
            });

            await Client.TopPlacements.GetAsync("1");
            await Client.TopPlacements.GetAsync("2", isExternal: true);
        }

        [Fact]
        public async Task OnTopPlacement_Get_CallSucceeds()
        {
            Http.RespondWith(m =>
            {
                return new Topplacements
                {
                    Topplacement = 
                    { 
                        new Topplacement 
                        { 
                            Realestateid = "1", 
                            MessageCode = MessageCode.MESSAGE_OPERATION_SUCCESSFUL,
                            Message = "toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 2, 1) },
                            ExternalId = "ext1"
                        },
                        new Topplacement 
                        { 
                            Realestateid = "1", 
                            MessageCode = MessageCode.MESSAGE_OPERATION_SUCCESSFUL,
                            Message = "toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 3, 1), DateTo = new DateTime(2014, 4, 1) },
                            ExternalId = "ext1"
                        }
                    }
                };
            });

            var result = await Client.TopPlacements.GetAsync("1");
            var expected = new Topplacements
            {
                Topplacement = 
                    { 
                        new Topplacement 
                        { 
                            Realestateid = "1", 
                            MessageCode = MessageCode.MESSAGE_OPERATION_SUCCESSFUL,
                            Message = "toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 2, 1) },
                            ExternalId = "ext1"
                        },
                        new Topplacement 
                        { 
                            Realestateid = "1", 
                            MessageCode = MessageCode.MESSAGE_OPERATION_SUCCESSFUL,
                            Message = "toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 3, 1), DateTo = new DateTime(2014, 4, 1) },
                            ExternalId = "ext1"
                        }
                    }
            };

            AssertEx.Equal(expected, result);
        }

        [Fact]
        public async Task OnTopPlacement_RemoveAll_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/topplacement/all", Http.Url.ToString());
                return new Topplacements();
            });

            await Client.TopPlacements.RemoveAllAsync();
        }

        [Fact]
        public async Task OnTopPlacement_RemoveAll_CallSucceeds()
        {
            Http.RespondWith(m =>
            {
                return new Topplacements
                {
                    Topplacement = 
                    { 
                        new Topplacement 
                        { 
                            Realestateid = "1", 
                            MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED,
                            Message = "de-toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 12, 31) },
                            ExternalId = "ext1"
                        },
                        new Topplacement 
                        { 
                            Realestateid = "2", 
                            MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED,
                            Message = "de-toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 12, 31) },
                            ExternalId = "ext2"
                        }
                    }
                };
            });

            var result = await Client.TopPlacements.RemoveAllAsync();
            var expected = new Topplacements
            {
                Topplacement = 
                    { 
                        new Topplacement 
                        { 
                            Realestateid = "1", 
                            MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED,
                            Message = "de-toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 12, 31) },
                            ExternalId = "ext1"
                        },
                        new Topplacement 
                        { 
                            Realestateid = "2", 
                            MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED,
                            Message = "de-toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 12, 31) },
                            ExternalId = "ext2"
                        }
                    }
            };

            AssertEx.Equal(expected, result);
        }

        [Fact]
        public async Task OnTopPlacement_Remove_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/1/topplacement", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "Resource topplacement with id 4711 has been deleted." } } };
            }).ThenWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/ext-2/topplacement", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "Resource topplacement with id 4712 has been deleted." } } };
            });

            await Client.TopPlacements.RemoveAsync("1");
            await Client.TopPlacements.RemoveAsync("2", isExternal: true);
        }

        [Fact]
        public async Task OnTopPlacement_Remove_CallSucceeds()
        {
            Http.RespondWith(m =>
            {
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "Resource topplacement with id 4711 has been deleted." } } };
            });

            await Client.TopPlacements.RemoveAsync("1");
        }

        [Fact]
        public async Task OnTopPlacement_Remove_ErrorOccurs_ThrowsIS24Exception()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/1/topplacement", Http.Url.ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.ERROR_RESOURCE_VALIDATION, MessageProperty = "Error while validating input for the resource. [MESSAGE: topplacement for realestate with id='1' can not be deleted before 2014-01-29T13:17:10.000+01:00]" } } };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.TopPlacements.RemoveAsync("1");
            });
        }


        [Fact]
        public async Task OnTopPlacement_RemoveList_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/topplacement/list?realestateids=" + WebUtility.UrlEncode("1,2"), Http.Url.ToString());
                return new Topplacements();
            }).RespondWith(m =>
            {
                Assert.Equal("DELETE", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/topplacement/list?realestateids=" + WebUtility.UrlEncode("ext-3,ext-4"), Http.Url.ToString());
                return new Topplacements();
            });

            await Client.TopPlacements.RemoveAsync(new[] { "1", "2" });
            await Client.TopPlacements.RemoveAsync(new[] { "3", "4" }, isExternal: true);
        }

        [Fact]
        public async Task OnTopPlacement_RemoveList_CallSucceeds()
        {
            Http.RespondWith(m =>
            {
                return new Topplacements
                {
                    Topplacement = 
                    { 
                        new Topplacement 
                        { 
                            Realestateid = "1", 
                            MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED,
                            Message = "de-toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 12, 31) },
                            ExternalId = "ext1"
                        },
                        new Topplacement 
                        { 
                            Realestateid = "2", 
                            MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED,
                            Message = "de-toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 12, 31) },
                            ExternalId = "ext2"
                        }
                    }
                };
            });

            var result = await Client.TopPlacements.RemoveAsync(new[] { "1", "2" });
            var expected = new Topplacements
            {
                Topplacement = 
                    { 
                        new Topplacement 
                        { 
                            Realestateid = "1", 
                            MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED,
                            Message = "de-toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 12, 31) },
                            ExternalId = "ext1"
                        },
                        new Topplacement 
                        { 
                            Realestateid = "2", 
                            MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED,
                            Message = "de-toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 12, 31) },
                            ExternalId = "ext2"
                        }
                    }
            };

            AssertEx.Equal(expected, result);
        }
    }
}
