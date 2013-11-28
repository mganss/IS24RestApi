﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using IS24RestApi;

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
            var is24Client = new IS24Client
            {
                ConsumerKey = config.ConsumerKey,
                ConsumerSecret = config.ConsumerSecret,
                AccessToken = config.AccessToken,
                AccessTokenSecret = config.AccessSecret,
                BaseUrlPrefix = @"http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0"
            };

            var api = new ImportExportClient(is24Client);
            RealtorContactDetails contact = null;

            try
            {
                contact = await api.Contacts.GetAsync("Hans Meiser", isExternal: true);
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

                await api.Contacts.CreateAsync(contact);

                contact.address.houseNumber = "1a";
                await api.Contacts.UpdateAsync(contact);
            }

            ApartmentRent re = null;

            try
            {
                re = await api.RealEstates.GetAsync("Hauptstraße 1", isExternal: true) as ApartmentRent;
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

                await api.RealEstates.CreateAsync(re);

                re.baseRent += 100.0;
                await api.RealEstates.UpdateAsync(re);
            }

            var atts = await api.Attachments.GetAsync(re);
            if (atts == null || !atts.Any())
            {
                var att = new Picture
                {
                    floorplan = false,
                    titlePicture = true,
                    title = "Zimmer",
                };

                await api.Attachments.CreateAsync(re, att, @"..\..\test.jpg");

                att.title = "Zimmer 1";
                await api.Attachments.UpdateAsync(re, att);
            }

            var res = new List<RealEstate>();
            await api.RealEstates.GetAsync().ForEachAsync(res.Add);
        }
    }
}
