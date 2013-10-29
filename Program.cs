using IS24;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var contact = api.GetContact("Hans Meiser", isExternal: true);
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

                api.CreateContact(contact);

                contact.address.houseNumber = "1a";
                api.UpdateContact(contact);
            }

            var re = api.GetRealEstate("Hauptstraße 1", isExternal: true) as ApartmentRent;
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

                api.CreateRealEstate(re);

                re.baseRent += 100.0;
                api.UpdateRealEstate(re);
            }

            var atts = api.GetAttachments(re);
            if (atts == null || !atts.Any())
            {
                var att = new Picture
                {
                    floorplan = false,
                    titlePicture = true,
                    title = "Zimmer",
                };

                api.CreateAttachment(re, att, @"..\..\test.jpg");

                att.title = "Zimmer 1";
                api.UpdateAttachment(re, att);
            }
        }
    }
}
