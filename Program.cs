using IS24;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace IS24RestApi
{
    class Program
    {
        class Config
        {
            public string ConsumerKey { get; set; }
            public string ConsumerSecret { get; set; }
            public string AccessToken { get; set; }
            public string AccessSecret { get; set; }
        }

        static void Main(string[] args)
        {
            var config = RestSharp.SimpleJson.DeserializeObject<Config>(File.ReadAllText("config.json"));
            var api = new RestApi
            {
                ConsumerKey = config.ConsumerKey,
                ConsumerSecret = config.ConsumerSecret,
                AccessToken = config.AccessToken,
                AccessTokenSecret = config.AccessSecret,
                BaseUrlPrefix = @"http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0"
            };

            try
            {
                var nre = new RealEstate { id = 4711 };
                var natts = api.GetAttachmentsAsync(nre).Result;
            }
            catch (AggregateException ex)
            {
                var iex = ex.InnerException as IS24Exception;
                if (iex != null)
                {
                    var msgs = iex.Messages;
                }
                else throw;
            }

            var contact = api.GetContactAsync("Hans Meiser", isExternal: true).Result;
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

                api.CreateContactAsync(contact).Wait();

                contact.address.houseNumber = "1a";
                api.UpdateContactAsync(contact).Wait();
            }

            var re = api.GetRealEstateAsync("Hauptstraße 1", isExternal: true).Result as ApartmentRent;
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
                    showAddress = true
                };

                api.CreateRealEstateAsync(re).Wait();

                re.baseRent += 100.0;
                api.UpdateRealEstateAsync(re).Wait();
            }

            var atts = api.GetAttachmentsAsync(re).Result;
            if (atts == null || !atts.Any())
            {
                var att = new Picture
                {
                    floorplan = false,
                    titlePicture = true,
                    title = "Zimmer",
                };

                api.CreateAttachmentAsync(re, att, @"..\..\test.jpg").Wait();

                att.title = "Zimmer 1";
                api.UpdateAttachmentAsync(re, att).Wait();
            }

            List<RealEstate> res = new List<RealEstate>();
            foreach (var r in api.GetRealEstatesAsync().ToEnumerable())
            {
                res.Add(r);
            }
        }
    }
}
