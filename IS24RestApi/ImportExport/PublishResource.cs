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
    public class PublishResource : ResourceBase, IPublishResource
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
        /// The URL path segment identifying the resource, e.g. "offer/v1.0/user/{username}"
        /// </summary>
        public override string UrlPath
        {
            get { return "offer/v1.0"; }
        }

        /// <summary>
        /// Depublishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <param name="channelId">The channelId of the channel to depublish from.</param>
        /// <exception cref="IS24Exception"></exception>
        public async Task UnpublishAsync(RealEstate realEstate, int channelId)
        {
            var req = is24Connection.CreateRequest("publish");
            req.AddParameter("realestate", realEstate.Id);
            req.AddParameter("publishchannel", channelId);
            var pos = await ExecuteAsync<PublishObjects>(is24Connection, req);

            if (pos.PublishObject != null && pos.PublishObject.Any())
            {
                req = is24Connection.CreateRequest("publish/{id}", Method.DELETE);
                req.AddParameter("id", pos.PublishObject[0].Id, ParameterType.UrlSegment);
                var pres = await ExecuteAsync<Messages>(is24Connection, req);
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
        /// <param name="channelId">The channelId of the channel to publish to.</param>
        /// <exception cref="IS24Exception"></exception>
        public async Task PublishAsync(RealEstate realEstate, int channelId)
        {
            var req = is24Connection.CreateRequest("publish");
            req.AddParameter("realestate", realEstate.Id);
            req.AddParameter("publishchannel", channelId);
            var pos = await ExecuteAsync<PublishObjects>(is24Connection, req);

            if (pos.PublishObject == null || !pos.PublishObject.Any())
            {
                req = is24Connection.CreateRequest("publish", Method.POST);
                var p = new PublishObject
                        {
                            PublishChannel = new PublishChannel { Id = channelId },
                            RealEstate = new PublishObjectRealEstate { Id = realEstate.Id }
                        };
                req.AddBody(p);
                var pres = await ExecuteAsync<Messages>(is24Connection, req);
                if (!pres.IsSuccessful(MessageCode.MESSAGE_RESOURCE_CREATED)) throw new IS24Exception(string.Format("Error publishing RealEstate {0}: {1}", realEstate.ExternalId, pres.Message.ToMessage())) { Messages = pres };
            }
        }

        /// <summary>
        /// Depublishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <returns></returns>
        public Task UnpublishAsync(RealEstate realEstate)
        {
            return UnpublishAsync(realEstate, ImportExportClient.ImmobilienscoutPublishChannelId);
        }

        /// <summary>
        /// Publishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <returns></returns>
        public Task PublishAsync(RealEstate realEstate)
        {
            return PublishAsync(realEstate, ImportExportClient.ImmobilienscoutPublishChannelId);
        }

        /// <summary>
        /// Depublishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <param name="publishChannel">The channel to depublish from.</param>
        /// <returns></returns>
        public Task UnpublishAsync(RealEstate realEstate, PublishChannel publishChannel)
        {
            return UnpublishAsync(realEstate, (int)publishChannel.Id);
        }

        /// <summary>
        /// Publishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <param name="publishChannel">The channel to publish to.</param>
        /// <returns></returns>
        public Task PublishAsync(RealEstate realEstate, PublishChannel publishChannel)
        {
            return PublishAsync(realEstate, (int)publishChannel.Id);
        }
    }
}