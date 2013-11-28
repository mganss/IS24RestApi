using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace IS24RestApi
{
    public class PublishResource : IPublishResource
    {
        private IIS24Client importExportClient;

        public PublishResource(IIS24Client importExportClient)
        {
            this.importExportClient = importExportClient;
        }

        /// <summary>
        /// Depublishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        public async Task UnpublishAsync(RealEstate realEstate)
        {
            var req = importExportClient.Request("publish");
            req.AddParameter("realestate", realEstate.id);
            req.AddParameter("publishchannel", ImportExportClient.ImmobilienscoutPublishChannelId);
            var pos = await importExportClient.ExecuteAsync<publishObjects>(req, "");

            if (pos.publishObject != null && pos.publishObject.Any())
            {
                req = importExportClient.Request("publish/{id}", Method.DELETE);
                req.AddParameter("id", pos.publishObject[0].id, ParameterType.UrlSegment);
                var pres = await importExportClient.ExecuteAsync<messages>(req, "");
                if (!pres.IsSuccessful(MessageCode.MESSAGE_RESOURCE_DELETED))
                {
                    throw new IS24Exception(string.Format("Error depublishing RealEstate {0}: {1}", realEstate.externalId, pres.message.ToMessage())) { Messages = pres };
                }
            }
        }

        /// <summary>
        /// Publishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        public async Task PublishAsync(RealEstate realEstate)
        {
            var req = importExportClient.Request("publish");
            req.AddParameter("realestate", realEstate.id);
            req.AddParameter("publishchannel", ImportExportClient.ImmobilienscoutPublishChannelId);
            var pos = await importExportClient.ExecuteAsync<publishObjects>(req, "");

            if (pos.publishObject == null || !pos.publishObject.Any())
            {
                req = importExportClient.Request("publish", Method.POST);
                var p = new PublishObject
                        {
                            publishChannel = new PublishChannel { id = ImportExportClient.ImmobilienscoutPublishChannelId, idSpecified = true },
                            realEstate = new PublishObjectRealEstate { id = realEstate.id, idSpecified = true }
                        };
                req.AddBody(p);
                var pres = await importExportClient.ExecuteAsync<messages>(req, "");
                if (!pres.IsSuccessful(MessageCode.MESSAGE_RESOURCE_CREATED)) throw new IS24Exception(string.Format("Error publishing RealEstate {0}: {1}", realEstate.externalId, pres.message.ToMessage())) { Messages = pres };
            }
        }
    }
}