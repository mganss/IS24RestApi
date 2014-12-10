using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using IS24RestApi;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.Common;
using IS24RestApi.Search;
using System.Diagnostics;
using System;
using RestSharp.Contrib;
using IS24RestApi.Offer.TopPlacement;
using IS24RestApi.Offer.RealEstateProject;

namespace SampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = RestSharp.SimpleJson.DeserializeObject<Config>(File.ReadAllText("config.json"));

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
                BaseUrlPrefix = @"http://rest.sandbox-immobilienscout24.de/restapi/security"
            };

            // step 1
            await connection.GetRequestToken(callbackUrl: "oob");

            // step 2
            try
            {
                var url = string.Format("{0}/oauth/confirm_access?oauth_token={1}", connection.BaseUrlPrefix,
                    HttpUtility.UrlEncode(connection.RequestToken));
                Process.Start(url);
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
                BaseUrlPrefix = @"http://rest.sandbox-immobilienscout24.de/restapi/api"
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

            await realEstate.PublishAsync();

            var placements = await realEstate.PremiumPlacements.GetAsync();

            if (placements.Premiumplacement.Any(p => p.MessageCode == MessageCode.MESSAGE_OPERATION_SUCCESSFUL))
                await realEstate.PremiumPlacements.RemoveAsync();

            await realEstate.PremiumPlacements.CreateAsync();

            var atts = await realEstate.Attachments.GetAsync();
            if (atts == null || !atts.Any())
            {
                var att = new Picture
                {
                    Floorplan = false,
                    TitlePicture = true,
                    Title = "Zimmer",
                };

                await realEstate.Attachments.CreateAsync(att, @"..\..\test.jpg");

                att.Title = "Zimmer 1";
                await realEstate.Attachments.UpdateAsync(att);

                var link = new Link { Title = "Test", Url = "http://www.example.com/" };

                await realEstate.Attachments.CreateAsync(link);

                var video = new StreamingVideo { Title = "Video" };
                await realEstate.Attachments.CreateStreamingVideoAsync(video, @"..\..\test.avi");
            }

            var res = new List<RealEstate>();
            await api.RealEstates.GetAsync().ForEachAsync(x => res.Add(x.RealEstate));
        }
    }
}
