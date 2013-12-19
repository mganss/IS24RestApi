using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using IS24RestApi;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.Common;

namespace SampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TestAsync().Wait();
        }

        private static async Task TestAsync()
        {
            var config = RestSharp.SimpleJson.DeserializeObject<Config>(File.ReadAllText("config.json"));
            var connection = new IS24Connection
            {
                ConsumerKey = config.ConsumerKey,
                ConsumerSecret = config.ConsumerSecret,
                AccessToken = config.AccessToken,
                AccessTokenSecret = config.AccessSecret,
                BaseUrlPrefix = @"http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0"
            };

            var api = new ImportExportClient(connection);
            RealtorContactDetails contact = null;

            try
            {
                contact = await api.Contacts.GetAsync("Hans Meiser", isExternal: true);
            }
            catch (IS24Exception ex)
            {
                if (ex.Messages.Message.First().MessageCode != MessageCode.ERROR_RESOURCE_NOT_FOUND) throw;
            }

            if (contact == null)
            {
                contact = new RealtorContactDetails
                {
                    Lastname = "Meiser",
                    Firstname = "Hans",
                    Address = new Address
                    {
                        Street = "Hauptstraße",
                        HouseNumber = "1",
                        Postcode = "10827",
                        City = "Berlin",
                        InternationalCountryRegion = new CountryRegion { Country = CountryCode.DEU, Region = "Berlin" }
                    },
                    ExternalId = "Hans Meiser"
                };

                await api.Contacts.CreateAsync(contact);

                contact.Address.HouseNumber = "1a";
                await api.Contacts.UpdateAsync(contact);
            }

            ApartmentRent re = null;

            try
            {
                re = await api.RealEstates.GetAsync("Hauptstraße 1", isExternal: true) as ApartmentRent;
            }
            catch (IS24Exception ex)
            {
                if (ex.Messages.Message.First().MessageCode != MessageCode.ERROR_RESOURCE_NOT_FOUND) throw;
            }
            
            if (re == null)
            {
                re = new ApartmentRent
                {
                    Contact = new RealEstateContact { Id = contact.Id, IdSpecified = true },
                    ExternalId = "Hauptstraße 1",
                    Title = "IS24RestApi Test",
                    Address = new Wgs84Address { Street = "Hauptstraße", HouseNumber = "1", City = "Berlin", Postcode = "10827" },
                    BaseRent = 500.0,
                    LivingSpace = 100.0,
                    NumberOfRooms = 4.0,
                    ShowAddress = true,
                    Courtage = new CourtageInfo { HasCourtage = YesNoNotApplicableType.NO }
                };

                await api.RealEstates.CreateAsync(re);

                re.BaseRent += 100.0;
                await api.RealEstates.UpdateAsync(re);
            }

            var atts = await api.Attachments.GetAsync(re);
            if (atts == null || !atts.Any())
            {
                var att = new Picture
                {
                    Floorplan = false,
                    TitlePicture = true,
                    Title = "Zimmer",
                };

                await api.Attachments.CreateAsync(re, att, @"..\..\test.jpg");

                att.Title = "Zimmer 1";
                await api.Attachments.UpdateAsync(re, att);
            }

            var res = new List<RealEstate>();
            await api.RealEstates.GetAsync().ForEachAsync(res.Add);
        }
    }
}
