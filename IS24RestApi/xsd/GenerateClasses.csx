#! "netcoreapp2.2"
#r "nuget: XmlSchemaClassGenerator-beta, 2.0.214"
#r "nuget: Glob.cs, 2.1.21"

using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using XmlSchemaClassGenerator;

var namespaceMapping = new NamespaceProvider
{
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/common/1.0"), "IS24RestApi.Common" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/offer/alterationdate/1.0"), "IS24RestApi.Offer.AlterationDate" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/attachmentsorder/1.0"), "IS24RestApi.AttachmentsOrder" },
    { new NamespaceKey("ttp://rest.immobilienscout24.de/schema/offer/productbookingoverview/1.0"), "IS24RestApi.Offer.ProductBookingOverview" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/offer/listelement/1.0"), "IS24RestApi.Offer.ListElement" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/offer/premiumplacement/1.0"), "IS24RestApi.Offer.PremiumPlacement" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/offer/user/1.0"), "IS24RestApi.Offer.User" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/offer/productrecommondation/1.0"), "IS24RestApi.Offer.ProductRecommendation" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/offer/toplisting/1.0"), "IS24RestApi.Offer.TopListing" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/realestate/counts/1.0"), "IS24RestApi.Realestate.Counts" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/offer/realestates/1.0"), "IS24RestApi.Offer.RealEstates" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/offer/realestateproject/1.0"), "IS24RestApi.Offer.RealEstateProject" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/offer/realestatestock/1.0"), "IS24RestApi.Offer.RealEstateStock" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/offer/realtor/1.0"), "IS24RestApi.Offer.Realtor" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/offer/realtorbadges/1.0"), "IS24RestApi.Offer.RealtorBadges" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/offer/showcaseplacement/1.0"), "IS24RestApi.Offer.ShowcasePlacement" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/offer/topplacement/1.0"), "IS24RestApi.Offer.TopPlacement" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/videoupload/1.0"), "IS24RestApi.VideoUpload" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/offer/zipandlocationtoregion/1.0"), "IS24RestApi.Offer.ZipAndLocationToRegion" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/platform/gis/1.0"), "IS24RestApi.Platform.Gis" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/user/1.0"), "IS24RestApi.User" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/search/common/1.0"), "IS24RestApi.Search.Common" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/search/expose/1.0"), "IS24RestApi.Search.Expose" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/search/region/1.0"), "IS24RestApi.Search.Region" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/search/savedSearch/1.0"), "IS24RestApi.Search.SavedSearch" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/search/resultlist/1.0"), "IS24RestApi.Search.ResultList" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/search/searcher/1.0"), "IS24RestApi.Search.Searcher" },
    { new NamespaceKey("http://rest.immobilienscout24.de/schema/search/shortlist/1.0"), "IS24RestApi.Search.ShortList" },
};

var generator = new Generator
{
    OutputFolder = @"..\generated",
    GenerateNullables = true,
    GenerateInterfaces = true,
    DataAnnotationMode = DataAnnotationMode.Partial,
    IntegerDataType = typeof(int),
    Log = s => Console.Out.WriteLine(s),
    NamespaceProvider = namespaceMapping,
};

var files = Ganss.IO.Glob.ExpandNames("*/*.xsd");

Console.Out.WriteLine("Generating classes from:\n" + string.Join("\n", files));
Console.Out.WriteLine("\nGenerating files...");

generator.Generate(files);
