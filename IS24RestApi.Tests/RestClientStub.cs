using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

    public class RestClientStub: RestClient
    {
        public List<Func<IRestRequest, RestResponseStub>> GetResponses { get; set; }
        public int CurrentCallNumber { get; set; }

        public RestClientStub()
        {
            GetResponses = new List<Func<IRestRequest, RestResponseStub>>();
            CurrentCallNumber = 0;
        }

        public RestClientStub RespondWith(Func<IRestRequest, RestResponseStub> getResponse)
        {
            GetResponses.Add(getResponse);
            return this;
        }

        public RestClientStub ThenWith(Func<IRestRequest, RestResponseStub> getResponse)
        {
            return RespondWith(getResponse);
        }

        public RestClientStub RespondWith(Func<IRestRequest, object> getObject)
        {
            GetResponses.Add(r =>
            {
                return new RestResponseStub { StatusCode = HttpStatusCode.OK, ResponseObject = getObject(r) };
            });

            return this;
        }

        public RestClientStub ThenWith(Func<IRestRequest, object> getObject)
        {
            return RespondWith(getObject);
        }

        private IRestResponse PerformGetResponse(IRestRequest request)
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

            return response;
        }

        public override Task<IRestResponse> ExecuteTaskAsync(IRestRequest request)
        {
            var response = PerformGetResponse(request);
            return Task.FromResult(response);
        }
    }
}
