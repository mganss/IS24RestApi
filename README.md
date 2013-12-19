IS24RestApi
===========

A small library that accesses the [Immobilienscout24 Import/Export REST API](http://developerwiki.immobilienscout24.de/wiki/Import-Export-API)
using [RestSharp](https://github.com/restsharp/RestSharp).

Getting Started
---------------

IS24RestApi is available as a [NuGet package](https://www.nuget.org/packages/IS24RestApi/).

The `IS24Client` class has CRUD methods for all resource types and the `SampleConsole` project shows a few uses. You need to acquire OAuth credentials beforehand (e.g. by carrying out [these steps](http://developerwiki.immobilienscout24.de/wiki/Customer-website_Tutorial#oAuth_by_our_playground))
and put them in the config.json file that's read from the current working directory at runtime (see the included config.example.json file).

All API calls are carried out asynchronously and the paging call `GetRealEstatesAsync()` uses [Reactive Extensions](http://rx.codeplex.com/) to return the real estate objects both lazily and asynchronously.

Regenerating API Types
----------------------

The classes in the Types.generated.cs file were generated from the XSD files provided by IS24. There are two PowerShell scripts in the xsd folder to automate this process. 
Open a Visual Studio command prompt and execute them with `powershell ...`. 
If you get a permissions error, open a PowerShell and type `Set-ExecutionPolicy Unrestricted` (more about this [here](http://technet.microsoft.com/en-us/library/ee176949.aspx)).

1. `DownloadSchemaFiles.ps1` downloads all .xsd files from [here](http://rest.immobilienscout24.de/restapi/api/offer/v1.0/?_wadl&_schema) and moves `messages*.xsd` to the includes folder.
2. `GenerateClasses.ps1` generates .cs files into the `generated` folder.

Contributing
------------

Pull requests to improve are welcome :)
