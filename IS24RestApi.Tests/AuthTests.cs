using IS24RestApi.Common;
using RestSharp;
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
        public RestClientStub RestClient { get; set; }

        public IS24Connection Connection { get; set; }

        public AuthTests()
        {
            RestClient = new RestClientStub();

            Connection = new IS24Connection
            {
                RestClientFactory = baseUrl =>
                {
                    RestClient.BaseUrl = new Uri(baseUrl);
                    return RestClient;
                },
                BaseUrlPrefix = @"https://rest.sandbox-immobilienscout24.de/restapi/api",
                ConsumerKey = "ConsumerKey",
                ConsumerSecret = "ConsumerSecret"
            };
        }

        [Fact]
        public async Task Auth_GetRequestToken_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.Get, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/oauth/request_token", RestClient.BuildUri(r).ToString());
                return new RestResponseStub
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
            RestClient.RespondWith(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.NotFound, ResponseObject = new Messages() };
            });

            await AssertEx.ThrowsAsync<IS24Exception>(async () => await Connection.GetRequestToken());
        }

        [Fact]
        public async Task Auth_GetAccessToken_RequestsCorrectResource()
        {
            RestClient.RespondWith(r =>
            {
                Assert.Equal(Method.Get, r.Method);
                Assert.Equal("https://rest.sandbox-immobilienscout24.de/restapi/api/oauth/access_token", RestClient.BuildUri(r).ToString());
                return new RestResponseStub
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
            RestClient.RespondWith(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.NotFound, ResponseObject = new Messages() };
            });

            Connection.RequestToken = "request_token";
            Connection.RequestTokenSecret = "request_secret";

            await AssertEx.ThrowsAsync<IS24Exception>(async () => await Connection.GetAccessToken("verifier"));
        }
    }
}
