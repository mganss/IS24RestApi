using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace IS24RestApi
{

    public class PublishResource
    {
        private IS24Client is24Client;

        public PublishResource(IS24Client is24Client)
        {
            this.is24Client = is24Client;
        }

        /// <summary>
        /// Depublishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        public async Task UnpublishAsync(RealEstate realEstate)
        {
            var req = is24Client.Is24RestClient.Request("publish");
            req.AddParameter("realestate", realEstate.id);
            req.AddParameter("publishchannel", IS24Client.ImmobilienscoutPublishChannelId);
            var pos = await is24Client.Is24RestClient.ExecuteAsync<publishObjects>(req, "");

            if (pos.publishObject != null && pos.publishObject.Any())
            {
                req = is24Client.Is24RestClient.Request("publish/{id}", Method.DELETE);
                req.AddParameter("id", pos.publishObject[0].id, ParameterType.UrlSegment);
                var pres = await is24Client.Is24RestClient.ExecuteAsync<messages>(req, "");
                if (!is24Client.Ok(pres, MessageCode.MESSAGE_RESOURCE_DELETED)) throw new IS24Exception(string.Format("Error depublishing RealEstate {0}: {1}", realEstate.externalId, pres.message.Msg())) { Messages = pres };
            }
        }

        /// <summary>
        /// Publishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        public async Task PublishAsync(RealEstate realEstate)
        {
            var req = is24Client.Is24RestClient.Request("publish");
            req.AddParameter("realestate", realEstate.id);
            req.AddParameter("publishchannel", IS24Client.ImmobilienscoutPublishChannelId);
            var pos = await is24Client.Is24RestClient.ExecuteAsync<publishObjects>(req, "");

            if (pos.publishObject == null || !pos.publishObject.Any())
            {
                req = is24Client.Is24RestClient.Request("publish", Method.POST);
                var p = new PublishObject
                        {
                            publishChannel = new PublishChannel { id = IS24Client.ImmobilienscoutPublishChannelId, idSpecified = true },
                            realEstate = new PublishObjectRealEstate { id = realEstate.id, idSpecified = true }
                        };
                req.AddBody(p);
                var pres = await is24Client.Is24RestClient.ExecuteAsync<messages>(req, "");
                if (!is24Client.Ok(pres, MessageCode.MESSAGE_RESOURCE_CREATED)) throw new IS24Exception(string.Format("Error publishing RealEstate {0}: {1}", realEstate.externalId, pres.message.Msg())) { Messages = pres };
            }
        }
    }
}