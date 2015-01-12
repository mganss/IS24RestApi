using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.Common;
using System.Collections.Generic;

namespace IS24RestApi
{
    /// <summary>
    /// The resource responsible for publishing real estates
    /// </summary>
    public class PublishResource : ResourceBase, IPublishResource
    {
        /// <summary>
        /// Gets the underlying <see cref="IIS24Connection"/> for executing the requests
        /// </summary>
        public IIS24Connection Connection { get; private set; }

        /// <summary>
        /// Creates a new <see cref="PublishResource"/> instance
        /// </summary>
        /// <param name="connection"></param>
        public PublishResource(IIS24Connection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// The URL path segment identifying the resource, e.g. "offer/v1.0/user/{username}"
        /// </summary>
        public override string UrlPath
        {
            get { return "offer/v1.0"; }
        }

        /// <summary>
        /// Gets all <see cref="PublishObject"/>s the user has access to matching the given parameters
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <param name="channelId">The channelId of the channel to depublish from.</param>
        /// <exception cref="IS24Exception"></exception>
        public async Task<PublishObjects> GetAsync(RealEstate realEstate, int? channelId = null)
        {
            var req = Connection.CreateRequest("publish");
            req.AddParameter("realestate", realEstate.Id);
            if (channelId != null) req.AddParameter("publishchannel", channelId.Value);
            var pos = await ExecuteAsync<PublishObjects>(Connection, req);
            return pos;
        }

        /// <summary>
        /// Depublishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <param name="channelId">The channelId of the channel to depublish from.</param>
        /// <exception cref="IS24Exception"></exception>
        public async Task UnpublishAsync(RealEstate realEstate, int channelId)
        {
            var pos = await GetAsync(realEstate, channelId);

            if (pos.PublishObject != null && pos.PublishObject.Any())
            {
                var req = Connection.CreateRequest("publish/{id}", Method.DELETE);
                req.AddParameter("id", pos.PublishObject[0].Id, ParameterType.UrlSegment);
                var pres = await ExecuteAsync<Messages>(Connection, req);
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
            var pos = await GetAsync(realEstate, channelId);

            if (pos.PublishObject == null || !pos.PublishObject.Any())
            {
                var req = Connection.CreateRequest("publish", Method.POST);
                var p = new PublishObject
                        {
                            PublishChannel = new PublishChannel { Id = channelId },
                            RealEstate = new PublishObjectRealEstate { Id = realEstate.Id }
                        };
                req.AddBody(p);
                var pres = await ExecuteAsync<Messages>(Connection, req);
                if (!pres.IsSuccessful(MessageCode.MESSAGE_RESOURCE_CREATED)) throw new IS24Exception(string.Format("Error publishing RealEstate {0}: {1}", realEstate.ExternalId, pres.Message.ToMessage())) { Messages = pres };
            }
        }

        /// <summary>
        /// Depublishes a RealEstate object from the IS24 channel.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <returns></returns>
        public Task UnpublishAsync(RealEstate realEstate)
        {
            return UnpublishAsync(realEstate, ImportExportClient.ImmobilienscoutPublishChannelId);
        }

        /// <summary>
        /// Publishes a RealEstate object to the IS24 channel.
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

        /// <summary>
        /// Publishes a list of RealEstate objects.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="channelId">The channelId of the channel to publish to.</param>
        /// <exception cref="IS24Exception"></exception>
        public Task<PublishObjects> PublishAsync(IEnumerable<RealEstate> realEstates, int channelId)
        {
            return PublishAsync(realEstates, new[] { channelId });
        }

        /// <summary>
        /// Publishes a list of RealEstate objects.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="publishChannel">The channel to publish to.</param>  
        public Task<PublishObjects> PublishAsync(IEnumerable<RealEstate> realEstates, PublishChannel publishChannel)
        {
            return PublishAsync(realEstates, (int)publishChannel.Id.Value);
        }

        /// <summary>
        /// Publishes a list of RealEstate objects to the specified channels.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="channelIds">The channelIds of the channels to publish to.</param>
        /// <exception cref="IS24Exception"></exception>
        public async Task<PublishObjects> PublishAsync(IEnumerable<RealEstate> realEstates, IEnumerable<int> channelIds)
        {
            var req = Connection.CreateRequest("publish/list", Method.POST);
            req.AddParameter("publishids",
                string.Join(",", realEstates.SelectMany(r => channelIds.Select(c => r.Id.Value.ToString() + "_" + c))));
            var pos = await ExecuteAsync<PublishObjects>(Connection, req);
            return pos;
        }

        /// <summary>
        /// Publishes a list of RealEstate objects to the specified channels.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="publishChannels">The channels to publish to.</param>  
        public Task<PublishObjects> PublishAsync(IEnumerable<RealEstate> realEstates, IEnumerable<PublishChannel> publishChannels)
        {
            return PublishAsync(realEstates, publishChannels.Select(c => (int)c.Id.Value));
        }

        /// <summary>
        /// Publishes a list of RealEstate objects to the IS24 channel.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        public Task<PublishObjects> PublishAsync(IEnumerable<RealEstate> realEstates)
        {
            return PublishAsync(realEstates, ImportExportClient.ImmobilienscoutPublishChannelId);
        }

        /// <summary>
        /// Depublishes a list of RealEstate objects.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="channelId">The channelId of the channel to depublish from.</param>
        /// <exception cref="IS24Exception"></exception>
        public Task<PublishObjects> UnpublishAsync(IEnumerable<RealEstate> realEstates, int channelId)
        {
            return UnpublishAsync(realEstates, new[] { channelId });
        }

        /// <summary>
        /// Depublishes a list of RealEstate objects.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="publishChannel">The channel to depublish from.</param>  
        public Task<PublishObjects> UnpublishAsync(IEnumerable<RealEstate> realEstates, PublishChannel publishChannel)
        {
            return UnpublishAsync(realEstates, (int)publishChannel.Id.Value);
        }

        /// <summary>
        /// Depublishes a list of RealEstate objects from the specified channels.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="channelIds">The channelIds of the channels to depublish from.</param>
        /// <exception cref="IS24Exception"></exception>
        public async Task<PublishObjects> UnpublishAsync(IEnumerable<RealEstate> realEstates, IEnumerable<int> channelIds)
        {
            var req = Connection.CreateRequest("publish/list", Method.DELETE);
            req.AddParameter("publishids",
                string.Join(",", realEstates.SelectMany(r => channelIds.Select(c => r.Id.Value.ToString() + "_" + c))));
            var pos = await ExecuteAsync<PublishObjects>(Connection, req);
            return pos;
        }

        /// <summary>
        /// Depublishes a list of RealEstate objects from the specified channels.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="publishChannels">The channels to depublish from.</param>  
        public Task<PublishObjects> UnpublishAsync(IEnumerable<RealEstate> realEstates, IEnumerable<PublishChannel> publishChannels)
        {
            return UnpublishAsync(realEstates, publishChannels.Select(c => (int)c.Id.Value));
        }

        /// <summary>
        /// Depublishes a list of RealEstate objects from the IS24 channel.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        public Task<PublishObjects> UnpublishAsync(IEnumerable<RealEstate> realEstates)
        {
            return UnpublishAsync(realEstates, ImportExportClient.ImmobilienscoutPublishChannelId);
        }
    }
}