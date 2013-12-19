using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.Common;

namespace IS24RestApi
{
    /// <summary>
    /// The resource responsible for publishing real estates
    /// </summary>
    public class PublishResource : IPublishResource
    {
        private IIS24Connection is24Connection;

        /// <summary>
        /// Creates a new <see cref="PublishResource"/> instance
        /// </summary>
        /// <param name="is24Connection"></param>
        public PublishResource(IIS24Connection is24Connection)
        {
            this.is24Connection = is24Connection;
        }

        /// <summary>
        /// Depublishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        public async Task UnpublishAsync(RealEstate realEstate)
        {
            var req = is24Connection.CreateRequest("publish");
            req.AddParameter("realestate", realEstate.Id);
            req.AddParameter("publishchannel", ImportExportClient.ImmobilienscoutPublishChannelId);
            var pos = await is24Connection.ExecuteAsync<PublishObjects>(req, "");

            if (pos.PublishObject != null && pos.PublishObject.Any())
            {
                req = is24Connection.CreateRequest("publish/{id}", Method.DELETE);
                req.AddParameter("id", pos.PublishObject[0].Id, ParameterType.UrlSegment);
                var pres = await is24Connection.ExecuteAsync<Messages>(req, "");
                if (!pres.IsSuccessful(MessageCode.MESSAGE_RESOURCE_DELETED))
                {
                    throw new IS24Exception(string.Format("Error depublishing RealEstate {0}: {1}", realEstate.ExternalId, pres.Message.ToMessage())) { Messages = pres };
                }
            }
        }

        /// <summary>
        /// Publishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        public async Task PublishAsync(RealEstate realEstate)
        {
            var req = is24Connection.CreateRequest("publish");
            req.AddParameter("realestate", realEstate.Id);
            req.AddParameter("publishchannel", ImportExportClient.ImmobilienscoutPublishChannelId);
            var pos = await is24Connection.ExecuteAsync<PublishObjects>(req, "");

            if (pos.PublishObject == null || !pos.PublishObject.Any())
            {
                req = is24Connection.CreateRequest("publish", Method.POST);
                var p = new PublishObject
                        {
                            PublishChannel = new PublishChannel { Id = ImportExportClient.ImmobilienscoutPublishChannelId, IdSpecified = true },
                            RealEstate = new PublishObjectRealEstate { Id = realEstate.Id, IdSpecified = true }
                        };
                req.AddBody(p);
                var pres = await is24Connection.ExecuteAsync<Messages>(req, "");
                if (!pres.IsSuccessful(MessageCode.MESSAGE_RESOURCE_CREATED)) throw new IS24Exception(string.Format("Error publishing RealEstate {0}: {1}", realEstate.ExternalId, pres.Message.ToMessage())) { Messages = pres };
            }
        }
    }
}