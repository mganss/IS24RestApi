using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using RestSharp.Extensions;
using RestSharp.Serializers;

namespace IS24RestApi
{
    /// <summary>
    /// The connection used for preparing and executing the rest requests to IS24 
    /// </summary>
    public class IS24Connection : IIS24Connection
    {
        /// <summary>
        /// Gets the default value for the current user
        /// </summary>
        public const string User = "me";

        /// <summary>
        /// The XML deserializer
        /// </summary>
        private  static readonly IDeserializer xmlDeserializer = new BaseXmlDeserializer();

        /// <summary>
        /// The XML serializer
        /// </summary>
        private static readonly ISerializer xmlSerializer = new BaseXmlSerializer();

        /// <summary>
        /// The URL prefix including the user part
        /// </summary>
        public string BaseUrl
        {
            get
            {
                return string.Format("{0}/user/{1}/", BaseUrlPrefix, Uri.EscapeDataString(User));
            }
        }

        /// <summary>
        /// The common URL prefix of all resources (e.g. "http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0").
        /// </summary>
        public string BaseUrlPrefix { get; set; }

        /// <summary>
        /// The OAuth ConsumerSecret
        /// </summary>
        public string ConsumerSecret { get; set; }

        /// <summary>
        /// The OAuth ConsumerKey
        /// </summary>
        public string ConsumerKey { get; set; }

        /// <summary>
        /// The OAuth AccessToken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The OAuth AccessTokenSecret
        /// </summary>
        public string AccessTokenSecret { get; set; }

        /// <summary>
        /// The factory of IHttp objects that the RestClient uses to communicate with the service.
        /// Used mainly for testing purposes.
        /// </summary>
        public IHttpFactory HttpFactory { get; set; }

        /// <summary>
        /// Creates a basic <see cref="IRestRequest"/> instance for the given resource
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public IRestRequest CreateRequest(string resource, Method method = Method.GET)
        {
            return new RestRequest(resource, method) { XmlSerializer = xmlSerializer };
        }

        private IRestResponse<T> Deserialize<T>(IRestRequest request, IRestResponse raw)
        {
            IRestResponse<T> response = new RestResponse<T>();

            try
            {
                response = raw.toAsyncResponse<T>();
                response.Request = request;

                // Only attempt to deserialize if the request has not errored due
                // to a transport or framework exception.  HTTP errors should attempt to 
                // be deserialized 

                if (response.ErrorException == null)
                {
                    var handler = xmlDeserializer;
                    handler.DateFormat = request.DateFormat;

                    if ((int)response.StatusCode < 400)
                    {
                        handler.RootElement = request.RootElement;
                        handler.Namespace = request.XmlNamespace;

                        response.Data = handler.Deserialize<T>(raw);
                    }
                    else
                    {
                        // An HTTP error occurred. Deserialize error messages.

                        var msgs = handler.Deserialize<messages>(raw);
                        var ex = new IS24Exception(msgs.message.ToMessage()) { Messages = msgs, StatusCode = raw.StatusCode };

                        response.ResponseStatus = ResponseStatus.Error;
                        response.ErrorMessage = ex.Message;
                        response.ErrorException = ex;
                    }
                }
            }
            catch (Exception ex)
            {
                response.ResponseStatus = ResponseStatus.Error;
                response.ErrorMessage = ex.Message;
                response.ErrorException = ex;
            }

            return response;
        }

        /// <summary>
        /// Performs an API request as an asynchronous task.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="request">The request object.</param>
        /// <param name="baseUrl">The suffix added to <see cref="BaseUrlPrefix"/> to obtain the request URL. If null, <see cref="BaseUrl"/> will be used.</param>
        /// <returns>The task representing the request.</returns>
        public async Task<T> ExecuteAsync<T>(IRestRequest request, string baseUrl = null) where T : new()
        {
            baseUrl = baseUrl == null ? BaseUrl : string.Join("/", BaseUrlPrefix, baseUrl);
            var client = new RestClient(baseUrl)
                         {
                             Authenticator =
                                 OAuth1Authenticator.ForProtectedResource(ConsumerKey,
                                     ConsumerSecret, AccessToken, AccessTokenSecret)
                         };
            if (HttpFactory != null) client.HttpFactory = HttpFactory;
            client.ClearHandlers();
            client.AddHandler("application/xml", xmlDeserializer);
            request.XmlSerializer = xmlSerializer;

            var response = Deserialize<T>(request, await client.ExecuteTaskAsync(request));

            if (response.ErrorException != null) throw response.ErrorException;

            return response.Data;
        }
    }
}