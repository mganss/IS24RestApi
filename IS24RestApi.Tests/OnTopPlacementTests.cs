using IS24RestApi.Common;
using IS24RestApi.Offer.PremiumPlacement;
using IS24RestApi.Offer.ShowcasePlacement;
using IS24RestApi.Offer.TopPlacement;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace IS24RestApi.Tests
{
    public class OnTopPlacementTests : ImportExportTestBase
    {
        public OnTopPlacementTests()
            : base(@"https://rest.sandbox-immobilienscout24.de/restapi/api")
        { }

        [Fact]
        public async Task OnTopPlacement_Create_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.POST, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/topplacement/list", RestClient.BuildUri(r).ToString());
                return new Topplacements();
            });

            var placements = await Client.TopPlacements.CreateAsync(new Topplacements { Topplacement = { new Topplacement { Realestateid = "1" } } });
        }

        [Fact]
        public async Task OnTopPlacement_Create_CallSucceeds()
        {
            RestClient.RespondWith(r =>
            {
                var e = r.Body() as Topplacements;
                Assert.IsType<Topplacements>(e);
                Assert.Single(e.Topplacement);
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
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.POST, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/1/topplacement", RestClient.BuildUri(r).ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource topplacement with id 4711 has been created." } } };
            }).ThenWith(r =>
            {
                Assert.Equal(Method.POST, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/ext-2/topplacement", RestClient.BuildUri(r).ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_CREATED, MessageProperty = "Resource topplacement with id 4712 has been created." } } };
            });

            await Client.TopPlacements.CreateAsync("1");
            await Client.TopPlacements.CreateAsync("2", isExternal: true);
        }

        [Fact]
        public async Task OnTopPlacement_CreateWithId_ErrorOccurs_ThrowsIS24Exception()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.POST, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/1/topplacement", RestClient.BuildUri(r).ToString());
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
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.GET, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/topplacement/all", RestClient.BuildUri(r).ToString());
                return new Topplacements();
            });

            await Client.TopPlacements.GetAllAsync();
        }

        [Fact]
        public async Task OnTopPlacement_GetAll_CallSucceeds()
        {
            RestClient.RespondWith(r =>
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
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.GET, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/1/topplacement", RestClient.BuildUri(r).ToString());
                return new Topplacements();
            }).ThenWith(r =>
            {
                Assert.Equal(Method.GET, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/ext-2/topplacement", RestClient.BuildUri(r).ToString());
                return new Topplacements();
            });

            await Client.TopPlacements.GetAsync("1");
            await Client.TopPlacements.GetAsync("2", isExternal: true);
        }

        [Fact]
        public async Task ShowcasePlacement_Get_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.GET, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/1/showcaseplacement", RestClient.BuildUri(r).ToString());
                return new Showcaseplacements
                {
                    Showcaseplacement =
                    {
                        new Showcaseplacement
                        {
                            Realestateid = "1",
                            Message = "toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 2, 1) },
                            ExternalId = "ext1",
                            MessageCode = "MESSAGE_OPERATION_SUCCESSFUL"
                        }
                    }
                };
            }).ThenWith(r =>
            {
                Assert.Equal(Method.GET, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/ext-2/showcaseplacement", RestClient.BuildUri(r).ToString());
                return new Showcaseplacements();
            });

            var result = await Client.ShowcasePlacements.GetAsync("1");
            var expected = new Showcaseplacement
            {
                Realestateid = "1",
                Message = "toplisted",
                ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 2, 1) },
                ExternalId = "ext1",
                MessageCode = "MESSAGE_OPERATION_SUCCESSFUL"
            };
            AssertEx.Equal(expected, result);
            await Client.ShowcasePlacements.GetAsync("2", isExternal: true);
        }

        [Fact]
        public async Task PremiumPlacement_Get_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.GET, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/1/premiumplacement", RestClient.BuildUri(r).ToString());
                return new Premiumplacements
                {
                    Premiumplacement =
                    {
                        new Premiumplacement
                        {
                            Realestateid = "1",
                            Message = "toplisted",
                            ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 2, 1) },
                            ExternalId = "ext1",
                            MessageCode = MessageCode.MESSAGE_OPERATION_SUCCESSFUL
                        }
                    }
                };
            }).ThenWith(r =>
            {
                Assert.Equal(Method.GET, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/ext-2/premiumplacement", RestClient.BuildUri(r).ToString());
                return new Premiumplacements();
            });

            var result = await Client.PremiumPlacements.GetAsync("1");
            var expected = new Premiumplacement
            {
                Realestateid = "1",
                Message = "toplisted",
                ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 2, 1) },
                ExternalId = "ext1",
                MessageCode = MessageCode.MESSAGE_OPERATION_SUCCESSFUL
            };
            AssertEx.Equal(expected, result);
            await Client.PremiumPlacements.GetAsync("2", isExternal: true);
        }

        [Fact]
        public async Task OnTopPlacement_Get_CallSucceeds()
        {
            RestClient.RespondWith(r =>
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
                        }
                    }
                };
            }).ThenWith(r =>
            {
                return new Topplacements
                {
                    Topplacement =
                    {
                        new Topplacement
                        {
                            Realestateid = "2",
                            MessageCode = MessageCode.ERROR_REQUESTED_DATA_NOT_FOUND,
                            Message = "not toplisted"
                        }
                    }
                };
            }).ThenWith(r =>
            {
                return new Topplacements
                {
                    Topplacement =
                    {
                        new Topplacement
                        {
                            Realestateid = "3",
                            MessageCode = MessageCode.ERROR_RESOURCE_NOT_FOUND,
                            Message = "resource not found"
                        }
                    }
                };
            });

            var result = await Client.TopPlacements.GetAsync("1");
            var expected = new Topplacement
            {
                Realestateid = "1",
                MessageCode = MessageCode.MESSAGE_OPERATION_SUCCESSFUL,
                Message = "toplisted",
                ServicePeriod = new DateRange { DateFrom = new DateTime(2014, 1, 1), DateTo = new DateTime(2014, 2, 1) },
                ExternalId = "ext1"
            };

            AssertEx.Equal(expected, result);

            result = await Client.TopPlacements.GetAsync("2");
            Assert.Null(result);

            await AssertEx.ThrowsAsync<IS24Exception>(async () =>
            {
                await Client.TopPlacements.GetAsync("3");
            });
        }

        [Fact]
        public async Task OnTopPlacement_RemoveAll_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.DELETE, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/topplacement/all", RestClient.BuildUri(r).ToString());
                return new Topplacements();
            });

            await Client.TopPlacements.RemoveAllAsync();
        }

        [Fact]
        public async Task OnTopPlacement_RemoveAll_CallSucceeds()
        {
            RestClient.RespondWith(r =>
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
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.DELETE, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/1/topplacement", RestClient.BuildUri(r).ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "Resource topplacement with id 4711 has been deleted." } } };
            }).ThenWith(r =>
            {
                Assert.Equal(Method.DELETE, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/ext-2/topplacement", RestClient.BuildUri(r).ToString());
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "Resource topplacement with id 4712 has been deleted." } } };
            });

            await Client.TopPlacements.RemoveAsync("1");
            await Client.TopPlacements.RemoveAsync("2", isExternal: true);
        }

        [Fact]
        public async Task OnTopPlacement_Remove_CallSucceeds()
        {
            RestClient.RespondWith(r =>
            {
                return new Messages { Message = { new Message { MessageCode = MessageCode.MESSAGE_RESOURCE_DELETED, MessageProperty = "Resource topplacement with id 4711 has been deleted." } } };
            });

            await Client.TopPlacements.RemoveAsync("1");
        }

        [Fact]
        public async Task OnTopPlacement_Remove_ErrorOccurs_ThrowsIS24Exception()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.DELETE, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/realestate/1/topplacement", RestClient.BuildUri(r).ToString());
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
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.DELETE, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/topplacement/list?realestateids=" + HttpUtility.UrlEncode("1,2"), RestClient.BuildUri(r).ToString());
                return new Topplacements();
            }).RespondWith(r =>
            {
                Assert.Equal(Method.DELETE, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0/user/me/topplacement/list?realestateids=" + HttpUtility.UrlEncode("ext-3,ext-4"), RestClient.BuildUri(r).ToString());
                return new Topplacements();
            });

            await Client.TopPlacements.RemoveAsync(new[] { "1", "2" });
            await Client.TopPlacements.RemoveAsync(new[] { "3", "4" }, isExternal: true);
        }

        [Fact]
        public async Task OnTopPlacement_RemoveList_CallSucceeds()
        {
            RestClient.RespondWith(r =>
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
