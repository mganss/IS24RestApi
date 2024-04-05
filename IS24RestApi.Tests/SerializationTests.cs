﻿using IS24RestApi.Offer.RealEstates;
using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace IS24RestApi.Tests
{
    public class SerializationTests
    {
        [Fact]
        public void CanSerializeAndDeserializeAllExampleXmlFiles()
        {
            Test<RealEstate>("apartmentBuy");
            Test<RealEstate>("apartmentRent");
            Test<RealEstate>("assistedLiving");
            Test<RealEstate>("compulsoryAuction");
            Test<RealEstate>("garageBuy");
            Test<RealEstate>("garageRent");
            Test<RealEstate>("gastronomy");
            Test<RealEstate>("houseBuy");
            Test<RealEstate>("houseRent");
            Test<RealEstate>("houseType");
            Test<RealEstate>("industry");
            Test<RealEstate>("investment");
            Test<RealEstate>("livingBuySite");
            Test<RealEstate>("livingRentSite");
            Test<RealEstate>("office");
            Test<RealEstate>("seniorCare");
            Test<RealEstate>("shortTermAccommodation");
            Test<RealEstate>("specialPurpose");
            Test<RealEstate>("store");
            Test<RealEstate>("tradeSite");
            Test<RealEstate>("flatShareRoom");
        }

        void Test<T>(string file)
        {
            var xmlSerializer = new BaseXmlSerializer();
            var xmlDeserializer = new BaseXmlDeserializer();

            foreach (var suffix in new[] { "min", "max" })
            {
                var xml = File.ReadAllText( Path.Combine(Directory.GetCurrentDirectory(),"..","..","..","xml",$"{file}_{suffix}.xml"));

                var deserializedObject = xmlDeserializer.Deserialize<T>(new RestResponse { Content = xml });

                var serializedXml = xmlSerializer.Serialize(deserializedObject);
                var deserializedXml = xmlDeserializer.Deserialize<T>(new RestResponse { Content = serializedXml });
                AssertEx.Equal(deserializedObject, deserializedXml);
            }
        }

        [Fact]
        public void BaseXmlSerializer_CanSerializeToBaseClass()
        {
            var re = new ApartmentRent { Title = "Test" };
            var xml = new BaseXmlSerializer().Serialize(re);
            var xElement = XElement.Parse(xml);

            Assert.Equal("apartmentRent", xElement.Name.LocalName);
        }
    }
}
