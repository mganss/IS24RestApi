using IS24RestApi.Common;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using RestSharp.Extensions;
using RestSharp.Serialization.Xml;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace IS24RestApi
{
    /// <summary>
    /// The connection used for preparing and executing the rest requests to IS24
    /// </summary>
    public class IS24Connection : IIS24Connection
    {
        /// <summary>
        /// The XML deserializer
        /// </summary>
        private static readonly IXmlDeserializer xmlDeserializer = new BaseXmlDeserializer();

        /// <summary>
        /// The XML serializer
        /// </summary>
        private static readonly IXmlSerializer xmlSerializer = new BaseXmlSerializer();

        private string _baseUrlPrefix;

        /// <summary>
        /// The common URL prefix of all resources (e.g. "https://rest.sandbox-immobilienscout24.de/restapi/api").
        /// </summary>
        public string BaseUrlPrefix
        {
            get { return _baseUrlPrefix; }
            set
            {
                if (!value.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Non-HTTPS URL prefix not allowed");
                _baseUrlPrefix = value;
            }
        }

        /// <summary>
        /// The OAuth Consumer Secret
        /// </summary>
        public string ConsumerSecret { get; set; }

        /// <summary>
        /// The OAuth Consumer Key
        /// </summary>
        public string ConsumerKey { get; set; }

        /// <summary>
        /// The OAuth Request Token.
        /// </summary>
        public string RequestToken { get; set; }

        /// <summary>
        /// The OAuth Request Token Secret
        /// </summary>
        public string RequestTokenSecret { get; set; }

        /// <summary>
        /// The OAuth Access Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The OAuth Access Token Secret
        /// </summary>
        public string AccessTokenSecret { get; set; }

        /// <summary>
        /// The factory of IRestClient objects that is used to communicate with the service.
        /// Used mainly for testing purposes.
        /// </summary>
        public Func<string, IRestClient> RestClientFactory { get; set; } = baseUrl => new RestClient(baseUrl);

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
                response = raw.ToAsyncResponse<T>();
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

                        var msgs = handler.Deserialize<Messages>(raw);
                        var ex = new IS24Exception(MessagesExtensions.ToMessage(msgs.Message)) { Messages = msgs, StatusCode = raw.StatusCode };

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

        static readonly string AssemblyVersion = typeof(IS24Connection).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        /// <summary>
        /// Performs an API request as an asynchronous task.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="request">The request object.</param>
        /// <param name="baseUrl">The suffix added to <see cref="BaseUrlPrefix"/> to obtain the request URL.</param>
        /// <returns>The task representing the request.</returns>
        public async Task<T> ExecuteAsync<T>(IRestRequest request, string baseUrl = null) where T : new()
        {
            var url = string.Join("/", BaseUrlPrefix, baseUrl);
            var client = RestClientFactory(url);

            client.UserAgent = "IS24RestApi/" + AssemblyVersion;
            client.Authenticator =
                OAuth1Authenticator.ForProtectedResource(ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret);
            client.ClearHandlers();
            client.AddHandler("application/xml", () => xmlDeserializer);
            request.XmlSerializer = xmlSerializer;

            var response = Deserialize<T>(request, await client.ExecuteTaskAsync(request));

            if (response.ErrorException != null) throw response.ErrorException;

            return response.Data;
        }

        /// <summary>
        /// Gets an OAuth request token. If successful, the returned values will be in <see cref="RequestToken"/> and <see cref="RequestTokenSecret"/>.
        /// </summary>
        /// <param name="callbackUrl">The callback URL. Use "oob" when not calling from a web application.</param>
        /// <returns>The task representing the request.</returns>
        /// <exception cref="IS24Exception"></exception>
        public async Task GetRequestToken(string callbackUrl = "oob")
        {
            var client = RestClientFactory(BaseUrlPrefix);
            client.Authenticator = OAuth1Authenticator.ForRequestToken(ConsumerKey, ConsumerSecret, callbackUrl);
            var request = new RestRequest("oauth/request_token", Method.GET);
            var response = await client.ExecuteTaskAsync(request);

            if (response.ErrorException != null) throw response.ErrorException;
            if (response.StatusCode != HttpStatusCode.OK) throw new IS24Exception(string.Format("Error getting request token, status {0}: {1}",
                response.StatusCode, response.StatusDescription));

            var qs = HttpUtility.ParseQueryString(response.Content);

            RequestToken = qs["oauth_token"];
            RequestTokenSecret = qs["oauth_token_secret"];
        }

        /// <summary>
        /// Gets an OAuth access token. If successful, the returned values will be in <see cref="AccessToken"/> and <see cref="AccessTokenSecret"/>.
        /// </summary>
        /// <param name="verifier">The verifier.</param>
        /// <returns>The task representing the request.</returns>
        /// <exception cref="IS24Exception"></exception>
        public async Task GetAccessToken(string verifier)
        {
            var client = RestClientFactory(BaseUrlPrefix);
            client.Authenticator = OAuth1Authenticator.ForAccessToken(ConsumerKey, ConsumerSecret, RequestToken, RequestTokenSecret, verifier);
            var request = new RestRequest("oauth/access_token", Method.GET);
            var response = await client.ExecuteTaskAsync(request);

            if (response.ErrorException != null) throw response.ErrorException;
            if (response.StatusCode != HttpStatusCode.OK) throw new IS24Exception(string.Format("Error getting access token, status {0}: {1}",
                response.StatusCode, response.StatusDescription));

            var qs = HttpUtility.ParseQueryString(response.Content);

            AccessToken = qs["oauth_token"];
            AccessTokenSecret = qs["oauth_token_secret"];
        }
    }
}