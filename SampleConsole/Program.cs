﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using IS24RestApi;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.Common;
using IS24RestApi.Search;
using System;
using System.Web;
using Newtonsoft.Json;

namespace SampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var config =  JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));

            if (args.Contains("-a"))
                AuthorizeAsync(config).Wait();
            else
                TestAsync(config).Wait();
        }

        private static async Task AuthorizeAsync(Config config)
        {
            // see http://api.immobilienscout24.de/useful/authentication/authentication-detailed.html

            var connection = new IS24Connection
            {
                ConsumerKey = config.ConsumerKey,
                ConsumerSecret = config.ConsumerSecret,
                BaseUrlPrefix = @"https://rest.sandbox-immobilienscout24.de/restapi/security"
            };

            // step 1
            await connection.GetRequestToken(callbackUrl: "oob");

            // step 2
            try
            {
                var url = string.Format("{0}/oauth/confirm_access?oauth_token={1}", connection.BaseUrlPrefix,
                    HttpUtility.UrlEncode(connection.RequestToken));
                Console.Out.WriteLine($"Please open {url} to obtain verifier.");
            }
            catch
            {
            }

            Console.Out.Write("Enter verifier: ");
            var verifier = Console.In.ReadLine().Trim();

            // step 3
            await connection.GetAccessToken(verifier);

            Console.Out.WriteLine("Access Token: {0}", connection.AccessToken);
            Console.Out.WriteLine("Access Token Secret: {0}", connection.AccessTokenSecret);
        }

        private static async Task TestAsync(Config config)
        {
            var connection = new IS24Connection
            {
                ConsumerKey = config.ConsumerKey,
                ConsumerSecret = config.ConsumerSecret,
                AccessToken = config.AccessToken,
                AccessTokenSecret = config.AccessSecret,
                BaseUrlPrefix = @"https://rest.sandbox-immobilienscout24.de/restapi/api"
            };

            var searchResource = new SearchResource(connection);
            var query = new RadiusQuery
            {
                Latitude = 52.49023,
                Longitude =  13.35939,
                Radius = 1,
                RealEstateType = RealEstateType.APARTMENT_RENT,
                Parameters = new
                {
                    Price = new DecimalRange { Max = 1000 },
                    LivingSpace = new DecimalRange { Min = 100 },
                    NumberOfRooms = new DecimalRange { Min = 4 },
                    Equipment = "balcony"
                },
                Channel = new HomepageChannel("me")
            };
            var results = await searchResource.Search(query);

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
                    Email = "hans.meiser@example.com",
                    ExternalId = "Hans Meiser"
                };

                await api.Contacts.CreateAsync(contact);

                contact.Address.HouseNumber = "1a";
                await api.Contacts.UpdateAsync(contact);
            }

            IRealEstate realEstate = null;

            try
            {
                realEstate = await api.RealEstates.GetAsync("Hauptstraße 1", isExternal: true);
            }
            catch (IS24Exception ex)
            {
                if (ex.Messages.Message.First().MessageCode != MessageCode.ERROR_RESOURCE_NOT_FOUND) throw;
            }

            if (realEstate == null)
            {
                var re = new ApartmentRent
                {
                    Contact = new RealEstateContact { Id = contact.Id },
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

                realEstate = new RealEstateItem(re, connection);
            }
            else
            {
                var re = (ApartmentRent)realEstate.RealEstate;
                re.BaseRent += 100.0;
                await api.RealEstates.UpdateAsync(re);
                re.BaseRent -= 100.0;
                await api.RealEstates.UpdateAsync(re);
            }

            var projects = await api.RealEstateProjects.GetAllAsync();
            if (projects.RealEstateProject.Any())
            {
                var project = projects.RealEstateProject.First();
                var entries = await api.RealEstateProjects.GetAllAsync(project.Id.Value);
                if (!entries.RealEstateProjectEntry.Any(e => e.RealEstateId.Value == realEstate.RealEstate.Id.Value))
                    await api.RealEstateProjects.AddAsync(project.Id.Value, realEstate.RealEstate);
            }

            var placement = await realEstate.PremiumPlacements.GetAsync();

            var a1 = new KeyValuePair<Attachment, string>(new Picture { Title = "Zimmer 1", Floorplan = false, TitlePicture = true }, @"..\..\..\test.jpg");
            var a2 = new KeyValuePair<Attachment, string>(new Picture { Title = "Zimmer 2" }, @"..\..\..\test.jpg");
            var a3 = new KeyValuePair<Attachment, string>(new Picture { Title = "Zimmer 3" }, @"..\..\..\test.jpg");
            var pdf = new KeyValuePair<Attachment, string>(new PDFDocument { Title = "Test" }, @"..\..\..\test.pdf");
            var video = new KeyValuePair<Attachment, string>(new StreamingVideo { Title = "Video" }, @"..\..\..\test.avi");
            var link = new KeyValuePair<Attachment, string>(new Link { Title = "Test", Url = "http://www.example.com/" }, null);

            var atts = new[] { video, a1, pdf, a2, link, a3 };

            await realEstate.Attachments.UpdateAsync(atts);

            var res = new List<RealEstate>();
            await api.RealEstates.GetAsync().ForEachAsync(x => res.Add(x.RealEstate));
        }
    }
}
