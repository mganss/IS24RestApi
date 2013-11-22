using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ImmobilienscoutDotNet;

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
            var api = new ImmobilienscoutApi
            {
                ConsumerKey = config.ConsumerKey,
                ConsumerSecret = config.ConsumerSecret,
                AccessToken = config.AccessToken,
                AccessTokenSecret = config.AccessSecret,
                BaseUrlPrefix = @"http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0"
            };

            RealtorContactDetails contact = null;

            try
            {
                contact = await api.GetContactAsync("Hans Meiser", isExternal: true);
            }
            catch (IS24Exception ex)
            {
                if (ex.Messages.message.First().messageCode != MessageCode.ERROR_RESOURCE_NOT_FOUND) throw;
            }

            if (contact == null)
            {
                contact = new RealtorContactDetails
                {
                    lastname = "Meiser",
                    firstname = "Hans",
                    address = new Address
                    {
                        street = "Hauptstraße",
                        houseNumber = "1",
                        postcode = "10827",
                        city = "Berlin",
                        internationalCountryRegion = new CountryRegion { country = CountryCode.DEU, region = "Berlin" }
                    },
                    externalId = "Hans Meiser"
                };

                await api.CreateContactAsync(contact);

                contact.address.houseNumber = "1a";
                await api.UpdateContactAsync(contact);
            }

            ApartmentRent re = null;

            try
            {
                re = await api.GetRealEstateAsync("Hauptstraße 1", isExternal: true) as ApartmentRent;
            }
            catch (IS24Exception ex)
            {
                if (ex.Messages.message.First().messageCode != MessageCode.ERROR_RESOURCE_NOT_FOUND) throw;
            }

            if (re == null)
            {
                re = new ApartmentRent
                {
                    contact = new RealEstateContact { id = contact.id, idSpecified = true },
                    externalId = "Hauptstraße 1",
                    title = "IS24RestApi Test",
                    address = new Wgs84Address { street = "Hauptstraße", houseNumber = "1", city = "Berlin", postcode = "10827" },
                    baseRent = 500.0,
                    livingSpace = 100.0,
                    numberOfRooms = 4.0,
                    showAddress = true,
                    courtage = new CourtageInfo { hasCourtage = YesNoNotApplicableType.NO }
                };

                await api.CreateRealEstateAsync(re);

                re.baseRent += 100.0;
                await api.UpdateRealEstateAsync(re);
            }

            var atts = await api.GetAttachmentsAsync(re);
            if (atts == null || !atts.Any())
            {
                var att = new Picture
                {
                    floorplan = false,
                    titlePicture = true,
                    title = "Zimmer",
                };

                await api.CreateAttachmentAsync(re, att, @"..\..\test.jpg");

                att.title = "Zimmer 1";
                await api.UpdateAttachmentAsync(re, att);
            }

            var res = new List<RealEstate>();
            await api.GetRealEstatesAsync().ForEachAsync(res.Add);
        }
    }
}
