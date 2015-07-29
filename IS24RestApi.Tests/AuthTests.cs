using IS24RestApi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IS24RestApi.Tests
{
    public class AuthTests
    {
        public HttpStub Http { get; set; }

        public IS24Connection Connection { get; set; }

        public AuthTests()
        {
            Http = new HttpStub();

            Connection = new IS24Connection
            {
                HttpFactory = new HttpFactory(Http),
                BaseUrlPrefix = @"http://rest.sandbox-immobilienscout24.de/restapi/api",
                ConsumerKey = "ConsumerKey",
                ConsumerSecret = "ConsumerSecret"
            };
        }

        [Fact]
        public async Task Auth_GetRequestToken_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/oauth/request_token", Http.Url.ToString());
                return new HttpStubResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Raw = true,
                    ResponseObject = Encoding.UTF8.GetBytes("?oauth_token=token&oauth_token_secret=secret"),
                    ContentType = "text/plain"
                };
            });

            await Connection.GetRequestToken();

            Assert.Equal("token", Connection.RequestToken);
            Assert.Equal("secret", Connection.RequestTokenSecret);
        }

        [Fact]
        public async Task Auth_GetRequestToken_ErrorThrowsException()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.NotFound, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () => await Connection.GetRequestToken());
        }

        [Fact]
        public async Task Auth_GetAccessToken_RequestsCorrectResource()
        {
            Http.RespondWith(m =>
            {
                Assert.Equal("GET", m);
                Assert.Equal("http://rest.sandbox-immobilienscout24.de/restapi/api/oauth/access_token", Http.Url.ToString());
                return new HttpStubResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Raw = true,
                    ResponseObject = Encoding.UTF8.GetBytes("?oauth_token=token&oauth_token_secret=secret"),
                    ContentType = "text/plain"
                };
            });

            Connection.RequestToken = "request_token";
            Connection.RequestTokenSecret = "request_secret";

            await Connection.GetAccessToken("verifier");

            Assert.Equal("token", Connection.AccessToken);
            Assert.Equal("secret", Connection.AccessTokenSecret);
        }

        [Fact]
        public async Task Auth_GetAccessToken_ErrorThrowsException()
        {
            Http.RespondWith(m =>
            {
                return new HttpStubResponse { StatusCode = HttpStatusCode.NotFound, ResponseObject = new Messages() };
            });

            Connection.RequestToken = "request_token";
            Connection.RequestTokenSecret = "request_secret";

            await AssertEx.ThrowsAsync<IS24Exception>(async () => await Connection.GetAccessToken("verifier"));
        }
    }
}
