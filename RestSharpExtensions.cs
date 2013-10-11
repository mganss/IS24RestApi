using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi
{
    static class RestSharpExtensions
    {
        public static IRestRequest AddBody(this IRestRequest req, object o, Type t, string xmlNamespace = "")
        {
            req.XmlSerializer.Namespace = xmlNamespace;
            var serialized = ((BaseXmlSerializer)req.XmlSerializer).Serialize(o, t);
            return req.AddParameter(req.XmlSerializer.ContentType, serialized, ParameterType.RequestBody);
        }
    }
}
