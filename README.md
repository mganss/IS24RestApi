IS24RestApi
===========

A small library that accesses the [Immobilienscout24 Import/Export REST API](http://developerwiki.immobilienscout24.de/wiki/Import-Export-API)
using [RestSharp](https://github.com/restsharp/RestSharp).

The `IS24Client` class has CRUD methods for all resource types and the `SampleConsole` project shows a few uses. You need to acquire OAuth credentials beforehand (e.g. by carrying out [these steps](http://developerwiki.immobilienscout24.de/wiki/Customer-website_Tutorial#oAuth_by_our_playground))
and put them in the config.json file that's read from the current working directory at runtime (see the included config.example.json file).

All API calls are carried out asynchronously and the paging call `GetRealEstatesAsync()` uses [Reactive Extensions](http://rx.codeplex.com/) to return the real estate objects both lazily and asynchronously.

The classes in the Types.generated.cs file were generated from the XSD files provided by IS24. Unfortunately to regenerate them there is currently some massaging necessary:

1. Generate file: `xsd alterationdate-1.0.xsd attachmentsorder-1.0.xsd ... /c /n:IS24RestApi` from a Visual Studio command prompt (`xsd *.xsd` doesn't work :(
2. Replace `Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Namespace="http://rest.immobilienscout24.de/schema/common/1.0"` with `Form=System.Xml.Schema.XmlSchemaForm.Unqualified`
3. Remove `abstract` from `RealEstate`, `Attachment` classes (RestSharp limitation)
4. Add `#pragma warning disable 1591` and `#pragma warning restore 1591` at the top and bottom resp.

There are two PowerShell scripts in the xsd folder to automate this process. Open a Visual Studio command prompt and execute them with `powershell ...`. If you get a permissions error, open a PowerShell and type `Set-ExecutionPolicy Unrestricted` (more about this [here](http://technet.microsoft.com/en-us/library/ee176949.aspx)).

1. `DownloadSchemaFiles.ps1` downloads all .xsd files from [here](http://rest.immobilienscout24.de/restapi/api/offer/v1.0/?_wadl&_schema) and moves `messages*.xsd` to the includes folder.
2. `GenerateClasses.ps1` calls xsd.exe and modifies the resulting .cs file as described above, resulting in a file called IS24.cs. Move it to the project folder if everything went well.

Pull requests to improve are welcome :)
