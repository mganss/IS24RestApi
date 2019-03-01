using IS24RestApi.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Net.Cache;

namespace IS24RestApi.Tests
{
    public class HttpStubResponse
    {
        public HttpStubResponse()
        {
            StatusCode = HttpStatusCode.OK;
        }

        public HttpStatusCode StatusCode { get; set; }
        public object ResponseObject { get; set; }
        public bool Raw { get; set; }
        public string ContentType { get; set; }
    }

    public class HttpStub: IHttp
    {
        public List<Func<string, HttpStubResponse>> GetResponses { get; set; }
        public int CurrentCallNumber { get; set; }

        public HttpStub()
        {
            Reset();
            GetResponses = new List<Func<string, HttpStubResponse>>();
            CurrentCallNumber = 0;
        }

        public void Reset()
        {
            Headers = new List<HttpHeader>();
            Files = new List<HttpFile>();
            Parameters = new List<HttpParameter>();
            Cookies = new List<HttpCookie>();
        }

        public HttpStub RespondWith(Func<string, HttpStubResponse> getResponse)
        {
            GetResponses.Add(getResponse);
            return this;
        }

        public HttpStub ThenWith(Func<string, HttpStubResponse> getResponse)
        {
            return RespondWith(getResponse);
        }

        public HttpStub RespondWith(Func<string, object> getObject)
        {
            GetResponses.Add(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.OK, ResponseObject = getObject(m) };
            });

            return this;
        }

        public HttpStub ThenWith(Func<string, object> getObject)
        {
            return RespondWith(getObject);
        }

        private HttpResponse GetStyleMethodInternal(string p)
        {
            return PerformGetResponse(p);
        }

        private HttpResponse PostPutInternal(string p)
        {
            return GetStyleMethodInternal(p);
        }

        private HttpWebRequest GetStyleMethodInternalAsync(string p, Action<HttpResponse> action)
        {
            var response = GetStyleMethodInternal(p);
            action(response);

            return HttpWebRequest.CreateHttp(Url);
        }

        private HttpWebRequest PutPostInternalAsync(string p, Action<HttpResponse> action)
        {
            return GetStyleMethodInternalAsync(p, action);
        }

        private HttpResponse PerformGetResponse(string method)
        {
            var response = new HttpResponse();
            response.ResponseStatus = ResponseStatus.None;

            try
            {
                Assert.InRange(CurrentCallNumber, 0, GetResponses.Count - 1);
                var r = GetResponses[CurrentCallNumber](method);
                CurrentCallNumber++;

                var bytes = r.Raw ? (byte[])r.ResponseObject : Encoding.UTF8.GetBytes(new BaseXmlSerializer().Serialize(r.ResponseObject));
                response.ResponseStatus = ResponseStatus.Completed;
                response.StatusCode = r.StatusCode;
                response.ContentType = r.ContentType ?? "text/xml";
                response.ContentLength = bytes.Length;
                response.RawBytes = bytes;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.ErrorException = ex;
                response.ResponseStatus = ResponseStatus.Error;
            }

            Reset();

            return response;
        }

        /// <summary>
        /// True if this HTTP request has any HTTP parameters
        /// </summary>
        protected bool HasParameters
        {
            get
            {
                return Parameters.Any();
            }
        }

        /// <summary>
        /// True if this HTTP request has any HTTP cookies
        /// </summary>
        protected bool HasCookies
        {
            get
            {
                return Cookies.Any();
            }
        }

        /// <summary>
        /// True if a request body has been specified
        /// </summary>
        protected bool HasBody
        {
            get
            {
                return RequestBodyBytes != null || !string.IsNullOrEmpty(RequestBody);
            }
        }

        /// <summary>
        /// True if files have been set to be uploaded
        /// </summary>
        protected bool HasFiles
        {
            get
            {
                return Files.Any();
            }
        }

        /// <summary>
        /// Always send a multipart/form-data request - even when no Files are present.
        /// </summary>
        public bool AlwaysMultipartFormData { get; set; }

        /// <summary>
        /// UserAgent to be sent with request
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// Timeout in milliseconds to be used for the request
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// The number of milliseconds before the writing or reading times out.
        /// </summary>
        public int ReadWriteTimeout { get; set; }
        /// <summary>
        /// System.Net.ICredentials to be sent with request
        /// </summary>
        public ICredentials Credentials { get; set; }
        /// <summary>
        /// The System.Net.CookieContainer to be used for the request
        /// </summary>
        public CookieContainer CookieContainer { get; set; }
        /// <summary>
        /// The method to use to write the response instead of reading into RawBytes
        /// </summary>
        public Action<Stream> ResponseWriter { get; set; }
        /// <summary>
        /// Collection of files to be sent with request
        /// </summary>
        public IList<HttpFile> Files { get; private set; }
        /// <summary>
        /// Whether or not HTTP 3xx response redirects should be automatically followed
        /// </summary>
        public bool FollowRedirects { get; set; }
        /// <summary>
        /// X509CertificateCollection to be sent with request
        /// </summary>
        public X509CertificateCollection ClientCertificates { get; set; }
        /// <summary>
        /// Maximum number of automatic redirects to follow if FollowRedirects is true
        /// </summary>
        public int? MaxRedirects { get; set; }
        /// <summary>
        /// Determine whether or not the "default credentials" (e.g. the user account under which the current process is running)
        /// will be sent along to the server.
        /// </summary>
        public bool UseDefaultCredentials { get; set; }

        /// <summary>
        /// HTTP headers to be sent with request
        /// </summary>
        public IList<HttpHeader> Headers { get; private set; }
        /// <summary>
        /// HTTP parameters (QueryString or Form values) to be sent with request
        /// </summary>
        public IList<HttpParameter> Parameters { get; private set; }
        /// <summary>
        /// HTTP cookies to be sent with request
        /// </summary>
        public IList<HttpCookie> Cookies { get; private set; }
        /// <summary>
        /// Request body to be sent with request
        /// </summary>
        public string RequestBody { get; set; }
        /// <summary>
        /// Content type of the request body.
        /// </summary>
        public string RequestContentType { get; set; }
        /// <summary>
        /// An alternative to RequestBody, for when the caller already has the byte array.
        /// </summary>
        public byte[] RequestBodyBytes { get; set; }
        /// <summary>
        /// URL to call for this request
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Proxy info to be sent with request
        /// </summary>
        public IWebProxy Proxy { get; set; }

        public HttpWebRequest DeleteAsync(Action<HttpResponse> action)
        {
            return GetStyleMethodInternalAsync("DELETE", action);
        }

        public HttpWebRequest GetAsync(Action<HttpResponse> action)
        {
            return GetStyleMethodInternalAsync("GET", action);
        }

        public HttpWebRequest HeadAsync(Action<HttpResponse> action)
        {
            return GetStyleMethodInternalAsync("HEAD", action);
        }

        public HttpWebRequest OptionsAsync(Action<HttpResponse> action)
        {
            return GetStyleMethodInternalAsync("OPTIONS", action);
        }

        public HttpWebRequest PostAsync(Action<HttpResponse> action)
        {
            return PutPostInternalAsync("POST", action);
        }

        public HttpWebRequest PutAsync(Action<HttpResponse> action)
        {
            return PutPostInternalAsync("PUT", action);
        }

        public HttpWebRequest PatchAsync(Action<HttpResponse> action)
        {
            return PutPostInternalAsync("PATCH", action);
        }

        public HttpWebRequest MergeAsync(Action<HttpResponse> action)
        {
            return PutPostInternalAsync("MERGE", action);
        }

        /// <summary>
        /// Execute an async POST-style request with the specified HTTP Method.
        /// </summary>
        /// <param name="httpMethod">The HTTP method to execute.</param>
        /// <returns></returns>
        public HttpWebRequest AsPostAsync(Action<HttpResponse> action, string httpMethod)
        {
            return PutPostInternalAsync(httpMethod.ToUpperInvariant(), action);
        }

        /// <summary>
        /// Execute an async GET-style request with the specified HTTP Method.
        /// </summary>
        /// <param name="httpMethod">The HTTP method to execute.</param>
        /// <returns></returns>
        public HttpWebRequest AsGetAsync(Action<HttpResponse> action, string httpMethod)
        {
            return GetStyleMethodInternalAsync(httpMethod.ToUpperInvariant(), action);
        }

        /// <summary>
        /// Execute a POST request
        /// </summary>
        public HttpResponse Post()
        {
            return PostPutInternal("POST");
        }

        /// <summary>
        /// Execute a PUT request
        /// </summary>
        public HttpResponse Put()
        {
            return PostPutInternal("PUT");
        }

        /// <summary>
        /// Execute a GET request
        /// </summary>
        public HttpResponse Get()
        {
            return GetStyleMethodInternal("GET");
        }

        /// <summary>
        /// Execute a HEAD request
        /// </summary>
        public HttpResponse Head()
        {
            return GetStyleMethodInternal("HEAD");
        }

        /// <summary>
        /// Execute an OPTIONS request
        /// </summary>
        public HttpResponse Options()
        {
            return GetStyleMethodInternal("OPTIONS");
        }

        /// <summary>
        /// Execute a DELETE request
        /// </summary>
        public HttpResponse Delete()
        {
            return GetStyleMethodInternal("DELETE");
        }

        /// <summary>
        /// Execute a PATCH request
        /// </summary>
        public HttpResponse Patch()
        {
            return PostPutInternal("PATCH");
        }

        /// <summary>
        /// Execute a MERGE request
        /// </summary>
        public HttpResponse Merge()
        {
            return PostPutInternal("MERGE");
        }

        /// <summary>
        /// Execute a GET-style request with the specified HTTP Method.
        /// </summary>
        /// <param name="httpMethod">The HTTP method to execute.</param>
        /// <returns></returns>
        public HttpResponse AsGet(string httpMethod)
        {
            return GetStyleMethodInternal(httpMethod.ToUpperInvariant());
        }

        /// <summary>
        /// Execute a POST-style request with the specified HTTP Method.
        /// </summary>
        /// <param name="httpMethod">The HTTP method to execute.</param>
        /// <returns></returns>
        public HttpResponse AsPost(string httpMethod)
        {
            return PostPutInternal(httpMethod.ToUpperInvariant());
        }

        public bool PreAuthenticate { get; set; }

        private Encoding encoding = Encoding.UTF8;

        public Encoding Encoding { get { return this.encoding; } set { this.encoding = value; } }

        public RequestCachePolicy CachePolicy { get; set; }
    }
}
