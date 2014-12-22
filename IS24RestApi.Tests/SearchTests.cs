using IS24RestApi.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Xunit;
using IS24RestApi.Search.Common;
using IS24RestApi.Search.ResultList;

namespace IS24RestApi.Tests
{
    public class SearchTests : TestBase<SearchResource>
    {
        public SearchTests()
            : base(@"http://rest.sandbox-immobilienscout24.de/restapi/api", c => new SearchResource(c))
        { }

        [Fact]
        public async Task Search_Get_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                var url = "http://rest.sandbox-immobilienscout24.de/restapi/api/search/v1.0/search/radius";
                Assert.Equal(url, Http.Url.GetLeftPart(UriPartial.Path));
                var parms = Http.Url.ParseQueryString();
                Assert.Equal("apartmentrent", parms["realestatetype"]);
                Assert.Equal("1.00;2.00;10", parms["geocoordinates"]);
                Assert.Equal("full text", parms["fulltext"]);
                Assert.Equal("2000-01-02T00:00:00", parms["lastmodification"]);
                Assert.Equal("2001-03-04T00:00:00", parms["publishedafter"]);
                Assert.Equal("2002-05-06T00:00:00", parms["firstactivation"]);
                Assert.Equal("1", parms["apisearchfield1"]);
                Assert.Equal("2", parms["apisearchfield2"]);
                Assert.Equal("3", parms["apisearchfield3"]);
                Assert.Equal("-1000.00", parms["price"]);
                Assert.Equal("rentpermonth", parms["pricetype"]);
                Assert.Equal("true", parms["freeofcourtageonly"]);
                Assert.Equal("-distance", parms["sort"]);
                return new IS24RestApi.Search.ResultList.Resultlist
                {
                    ResultlistEntries =
                    {
                        new ResultlistEntries 
                        {
                            ResultlistEntry =
                            {
                                new ResultlistEntry { RealEstate = new ApartmentRent { Id = 4711 } },
                                new ResultlistEntry { RealEstate = new ApartmentRent { Id = 4712 } },
                            }
                        }
                    }
                };
            });

            var query = new RadiusQuery
            {
                RealEstateType = Common.RealEstateType.APARTMENT_RENT,
                Latitude = 1.0,
                Longitude = 2.0,
                Radius = 10,
                FullText = "full text",
                LastModification = new DateTime(2000, 1, 2),
                PublishedAfter = new DateTime(2001, 3, 4),
                FirstActivation = new DateTime(2002, 5, 6),
                ApiSearchField1 = "1",
                ApiSearchField2 = "2",
                ApiSearchField3 = "3",
                Channel = new HomepageChannel("user"),
                Parameters = new 
                {
                    Price = new DecimalRange { Max = 1000 },
                    PriceType = "rentpermonth",
                    FreeOfCourtageOnly = true,
                },
                Sort = Sorting.Distance,
                SortDirection = ListSortDirection.Descending
            };

            var res = await Client.Search(query);

            Assert.Equal(2, res.ResultlistEntries[0].ResultlistEntry.Count);
            Assert.Equal(4711, res.ResultlistEntries[0].ResultlistEntry[0].RealEstate.Id);
            Assert.Equal(4712, res.ResultlistEntries[0].ResultlistEntry[1].RealEstate.Id);
        }

        [Fact]
        public void Search_CanSerializeRanges()
        {
            var intRange = new IntRange { Min = 3, Max = 9 };
            Assert.Equal("3-9", intRange.ToString());
            intRange = new IntRange { Min = 2 };
            Assert.Equal("2-", intRange.ToString());
            intRange = new IntRange { Max = 4 };
            Assert.Equal("-4", intRange.ToString());

            var doubleRange = new DoubleRange { Min = 1.23, Max = 4.56 };
            Assert.Equal("1.23-4.56", doubleRange.ToString());

            var decimalRange = new DecimalRange { Min = 1.23m, Max = 4.56m };
            Assert.Equal("1.23-4.56", decimalRange.ToString());
        }

        [Fact]
        public async Task Search_Get_CanPerformRegionSearch()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                var url = "http://rest.sandbox-immobilienscout24.de/restapi/api/search/v1.0/search/region";
                Assert.Equal(url, Http.Url.GetLeftPart(UriPartial.Path));
                var parms = Http.Url.ParseQueryString();
                Assert.Equal("grouping,matchcount", parms["features"]);
                Assert.Equal("apartmentrent", parms["realestatetype"]);
                Assert.Equal("1002003004005", parms["geocodes"]);
                Assert.Equal("-1000.00", parms["price"]);
                Assert.Equal("rentpermonth", parms["pricetype"]);
                Assert.Equal("true", parms["freeofcourtageonly"]);
                Assert.Equal("-distance", parms["sort"]);
                Assert.Equal("4", parms["pagenumber"]);
                Assert.Equal("10", parms["pagesize"]);
                Assert.Equal("balcony,garden,lift", parms["equipment"]);
                return new IS24RestApi.Search.ResultList.Resultlist
                {
                    ResultlistEntries =
                    {
                        new ResultlistEntries 
                        {
                            ResultlistEntry =
                            {
                                new ResultlistEntry { RealEstate = new ApartmentRent { Id = 4711 } },
                                new ResultlistEntry { RealEstate = new ApartmentRent { Id = 4712 } },
                            }
                        }
                    }
                };
            });

            var query = new RegionQuery
            {
                RealEstateType = Common.RealEstateType.APARTMENT_RENT,
                GeoCodeId = new GeoCodeId { Continent = 1, Country = 2, Region = 3, City = 4, Quarter = 5 },
                Features = { Feature.Grouping, Feature.MatchCount },
                Parameters = new Dictionary<string, object>
                {
                    { "Price",  new DecimalRange { Max = 1000 } },
                    { "PriceType", "rentpermonth" },
                    { "FreeOfCourtageOnly",  true },
                    { "Equipment", new[] { "balcony", "garden", "lift" } }
                },
                Sort = Sorting.Distance,
                SortDirection = ListSortDirection.Descending
            };

            var res = await Client.Search(query, 4, 10);

            Assert.Equal(2, res.ResultlistEntries[0].ResultlistEntry.Count);
            Assert.Equal(4711, res.ResultlistEntries[0].ResultlistEntry[0].RealEstate.Id);
            Assert.Equal(4712, res.ResultlistEntries[0].ResultlistEntry[1].RealEstate.Id);
        }
    }
}
