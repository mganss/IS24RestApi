using System;
using System.Threading.Tasks;
using RestSharp;

namespace IS24RestApi
{
    static class RestSharpExtensions
    {
        // public static RestRequest AddBody(this RestRequest req, object o, Type t, string xmlNamespace = "")
        // {
        //     req.XmlSerializer.Namespace = xmlNamespace;
        //     var serialized = ((BaseXmlSerializer)req.XmlSerializer).Serialize(o, t);
        //     return req.AddParameter(req.XmlSerializer.ContentType, serialized, ParameterType.RequestBody);
        // }
    }
}
