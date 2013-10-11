IS24RestApi
===========

A small console application that accesses the [Immobilienscout24 Import/Export REST API](http://developerwiki.immobilienscout24.de/wiki/Import-Export-API)
using [RestSharp](https://github.com/restsharp/RestSharp).

The `RestApi` class has CRUD methods for all resource types and the `Main()` methods shows a few uses. You need to acquire OAuth credentials beforehand
and put them in the `config.json` file that's read from the current working directory at runtime (see the included `config.example.json` file).

The classes in the IS24.cs file were generated from the XSD files provided by IS24. Unfortunately to regenerate them there is currently some massaging necessary:

1. Generate file: `xsd alterationdate-1.0.xsd attachmentsorder-1.0.xsd ... /c /n:IS24` from a Visual Studio command prompt (`xsd *.xsd` doesn't work :(
2. Replace `Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Namespace="http://rest.immobilienscout24.de/schema/common/1.0"` with `Form=System.Xml.Schema.XmlSchemaForm.Unqualified`
3. Remove `abstract` from `RealEstate`, `Attachment` class (RestSharp limitation)
4. Add `#pragma warning disable 1591` and `#pragma warning restore 1591` at the top and bottom resp.

Pull requests to improve are welcome :)
