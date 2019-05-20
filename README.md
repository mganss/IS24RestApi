IS24RestApi
===========

[![NuGet version](https://badge.fury.io/nu/IS24RestApi.svg)](http://badge.fury.io/nu/IS24RestApi)
[![Build status](https://ci.appveyor.com/api/projects/status/xdpv5jbm3vtr993s/branch/master?svg=true)](https://ci.appveyor.com/project/mganss/is24restapi/branch/master)
[![codecov.io](https://codecov.io/github/mganss/IS24RestApi/coverage.svg?branch=master)](https://codecov.io/github/mganss/IS24RestApi?branch=master)
[![Sonarcloud Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=mganss_IS24RestApi2&metric=alert_status)](https://sonarcloud.io/dashboard?id=mganss_IS24RestApi2)

[![netstandard2.0](https://img.shields.io/badge/netstandard-2.0-brightgreen.svg)](https://img.shields.io/badge/netstandard-2.0-brightgreen.svg)

A small library to access the [Immobilienscout24 Import/Export REST API](http://api.immobilienscout24.de/our-apis/import-export.html)
using [RestSharp](https://github.com/restsharp/RestSharp).

Getting Started
---------------

IS24RestApi is available as a [NuGet package](https://www.nuget.org/packages/IS24RestApi/).

The `ImportExportClient` class has CRUD methods for all resource types and the `SampleConsole` project shows a few uses. 
You need to acquire OAuth credentials beforehand 
(e.g. by carrying out [these steps](http://api.immobilienscout24.de/useful/tutorials-sdks-plugins/tutorial-customer-website.html))
and put them in the config.json file that's read from the current working directory at runtime (see the included config.example.json file).

The `AuthorizeAsync` method shows how to perform the OAuth authorization steps programmatically.

All API calls are carried out asynchronously and the paging call `GetAsync()` for `RealEstate` resources
uses [Reactive Extensions](http://rx.codeplex.com/) to return the real estate objects both lazily and asynchronously.

Regenerating API Types
----------------------

The classes in the Types.generated.cs file were generated from the XSD files provided by IS24
using [XmlSchemaClassGenerator](https://github.com/mganss/XmlSchemaClassGenerator). 
There are two [dotnet-script](https://github.com/filipw/dotnet-script) scripts in the xsd folder to automate this process. 

1. `DownloadSchemaFiles.csx` downloads all .xsd files from [here](http://rest.immobilienscout24.de/restapi/api/offer/v1.0/?_wadl&_schema) 
and moves `messages*.xsd` to the includes folder.
2. `GenerateClasses.csx` generates .cs files into the `generated` folder.

Contributing
------------

Pull requests to improve are welcome :)
