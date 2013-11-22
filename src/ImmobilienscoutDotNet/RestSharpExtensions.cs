using System;
using System.Threading.Tasks;
using RestSharp;

namespace ImmobilienscoutDotNet
{
    static class RestSharpExtensions
    {
        public static IRestRequest AddBody(this IRestRequest req, object o, Type t, string xmlNamespace = "")
        {
            req.XmlSerializer.Namespace = xmlNamespace;
            var serialized = ((BaseXmlSerializer)req.XmlSerializer).Serialize(o, t);
            return req.AddParameter(req.XmlSerializer.ContentType, serialized, ParameterType.RequestBody);
        }

        public static Task<IRestResponse> ExecuteAsync(this IRestClient client, IRestRequest request)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();

            try
            {
                client.ExecuteAsync(request, r => tcs.SetResult(r));
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }

            return tcs.Task;
        }
    }
}
