using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestSharp.Serializers;
using RestSharp.Serializers.Xml;
using Xunit;

namespace IS24RestApi.Tests
{
    public class RestResponseStub
    {
        public RestResponseStub()
        {
            StatusCode = HttpStatusCode.OK;
        }

        public HttpStatusCode StatusCode { get; set; }
        public object ResponseObject { get; set; }
        public bool Raw { get; set; }
        public string ContentType { get; set; }
    }

    public class RestClientStub: IRestClient
    {
        public List<Func<RestRequest, RestResponseStub>> GetResponses { get; set; }
        public int CurrentCallNumber { get; set; }

        public RestClientStub()
        {
            GetResponses = new List<Func<RestRequest, RestResponseStub>>();
            CurrentCallNumber = 0;
        }

        public RestClientStub(string baseUrl, RestClientStub restClientStub)
        {
            GetResponses = restClientStub?.GetResponses ?? new List<Func<RestRequest, RestResponseStub>>();
            CurrentCallNumber = restClientStub?.CurrentCallNumber ?? 0;
            Options = new ReadOnlyRestClientOptions(new RestClientOptions(baseUrl));
            DefaultParameters = new DefaultParameters(Options);
            Serializers = new RestSerializers(new Dictionary<DataFormat, SerializerRecord>()
            {
                {
                    DataFormat.Xml,
                    new SerializerRecord(
                        DataFormat.Xml,
                        new[] { "text/xml", "application/xml" },
                        type => true,
                        () => new XmlRestSerializer()
                            .WithXmlSerializer(new BaseXmlSerializer())
                            .WithXmlDeserializer(new BaseXmlDeserializer()))
                }
            });                       
        }

        public RestClientStub RespondWith(Func<RestRequest, RestResponseStub> getResponse)
        {
            GetResponses.Add(getResponse);
            return this;
        }

        public RestClientStub ThenWith(Func<RestRequest, RestResponseStub> getResponse)
        {
            return RespondWith(getResponse);
        }

        public RestClientStub RespondWith(Func<RestRequest, object> getObject)
        {
            GetResponses.Add(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.OK, ResponseObject = getObject(r) };
            });

            return this;
        }

        public RestClientStub ThenWith(Func<RestRequest, object> getObject)
        {
            return RespondWith(getObject);
        }

        private RestResponse PerformGetResponse(RestRequest request)
        {
            var response = new RestResponse
            {
                ResponseStatus = ResponseStatus.None
            };

            try
            {
                Assert.InRange(CurrentCallNumber, 0, GetResponses.Count - 1);
                var r = GetResponses[CurrentCallNumber](request);
                CurrentCallNumber++;

                var serializedResponse =
                    r.Raw
                        ? Encoding.UTF8.GetString((byte[])r.ResponseObject)
                        : new BaseXmlSerializer().Serialize(r.ResponseObject);
                
                var bytes = r.Raw ? (byte[])r.ResponseObject : Encoding.UTF8.GetBytes(serializedResponse);
                response.Content = serializedResponse;
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

            return response;
        }

        public Task<RestResponse> ExecuteAsync(RestRequest request, CancellationToken cancellationToken = default)
        {
            var response = PerformGetResponse(request);
            return Task.FromResult(response);
        }

        public async Task<Stream> DownloadStreamAsync(RestRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ReadOnlyRestClientOptions Options { get; }
        public RestSerializers Serializers { get; }
        public DefaultParameters DefaultParameters { get; }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}
