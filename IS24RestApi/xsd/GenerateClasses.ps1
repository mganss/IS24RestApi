$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath

cd $dir

# Powershell doesn't automatically set the .NET cwd >:(
[Environment]::CurrentDirectory = (Get-Location -PSProvider FileSystem).ProviderPath

$dll = ls ..\..\packages\XmlSchemaClassGenerator*\*\XmlSchemaClassGenerator.dll | Sort-Object LastModificationTime -Descending | Select-Object -First 1
[System.Reflection.Assembly]::LoadFrom($dll.FullName) | Out-Null
$generator = New-Object XmlSchemaClassGenerator.Generator
$generator.OutputFolder = '..\generated'
$namespaceMapping = New-Object 'System.Collections.Generic.Dictionary[string,string]'
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/common/1.0", "IS24RestApi.Common")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/offer/alterationdate/1.0", "IS24RestApi.Offer.AlterationDate")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/attachmentsorder/1.0", "IS24RestApi.AttachmentsOrder")
$namespaceMapping.Add("ttp://rest.immobilienscout24.de/schema/offer/productbookingoverview/1.0", "IS24RestApi.Offer.ProductBookingOverview")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/offer/listelement/1.0", "IS24RestApi.Offer.ListElement")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/offer/premiumplacement/1.0", "IS24RestApi.Offer.PremiumPlacement")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/offer/user/1.0", "IS24RestApi.Offer.User")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/offer/productrecommondation/1.0", "IS24RestApi.Offer.ProductRecommendation")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/offer/toplisting/1.0", "IS24RestApi.Offer.TopListing")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/realestate/counts/1.0", "IS24RestApi.Realestate.Counts")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/offer/realestates/1.0", "IS24RestApi.Offer.RealEstates")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/offer/realestateproject/1.0", "IS24RestApi.Offer.RealEstateProject")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/offer/realestatestock/1.0", "IS24RestApi.Offer.RealEstateStock")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/offer/realtor/1.0", "IS24RestApi.Offer.Realtor")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/offer/realtorbadges/1.0", "IS24RestApi.Offer.RealtorBadges")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/offer/showcaseplacement/1.0", "IS24RestApi.Offer.ShowcasePlacement")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/offer/topplacement/1.0", "IS24RestApi.Offer.TopPlacement")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/videoupload/1.0", "IS24RestApi.VideoUpload")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/offer/zipandlocationtoregion/1.0", "IS24RestApi.Offer.ZipAndLocationToRegion")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/platform/gis/1.0", "IS24RestApi.Platform.Gis")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/user/1.0", "IS24RestApi.User")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/search/common/1.0", "IS24RestApi.Search.Common")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/search/expose/1.0", "IS24RestApi.Search.Expose")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/search/region/1.0", "IS24RestApi.Search.Region")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/search/savedSearch/1.0", "IS24RestApi.Search.SavedSearch")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/search/resultlist/1.0", "IS24RestApi.Search.ResultList")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/search/searcher/1.0", "IS24RestApi.Search.Searcher")
$namespaceMapping.Add("http://rest.immobilienscout24.de/schema/search/shortlist/1.0", "IS24RestApi.Search.ShortList")
$generator.NamespaceMapping = $namespaceMapping
$generator.GenerateNullables = $true
[XmlSchemaClassGenerator.SimpleModel]::IntegerDataType = [System.Type]::GetType("System.Int32")

[System.String[]]$files = ls */*.xsd | %{ $_.FullName }

echo "Generating classes from:"
echo $files
echo ""
echo "Generating files:"

$generator.Log = [System.Action[System.String]]{ param($s) [System.Console]::Out.WriteLine($s); }

$generator.Generate($files)
