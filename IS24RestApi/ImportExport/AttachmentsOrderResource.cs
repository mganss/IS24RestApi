using System.Threading.Tasks;
using RestSharp;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.AttachmentsOrder;
using IS24RestApi.Common;

namespace IS24RestApi
{
    /// <summary>
    /// The resource responsible for managing the order of attachments displayed on IS24
    /// </summary>
    public class AttachmentsOrderResource : ImportExportResourceBase, IAttachmentsOrderResource
    {
        /// <summary>
        /// Gets the underlying <see cref="IIS24Connection"/> for executing the requests
        /// </summary>
        public IIS24Connection Connection { get; private set; }

        /// <summary>
        /// Gets the <see cref="RealEstate"/> data
        /// </summary>
        public RealEstate RealEstate { get; private set; }

        /// <summary>
        /// Creates a new <see cref="AttachmentsOrderResource"/> instance
        /// </summary>
        /// <param name="realEstate"></param>
        /// <param name="connection"></param>
        public AttachmentsOrderResource(RealEstate realEstate, IIS24Connection connection)
        {
            RealEstate = realEstate;
            Connection = connection;
        }

        /// <summary>
        /// Get the current order of uploaded <see cref="Attachment"/>s
        /// </summary>
        /// <returns></returns>
        public async Task<List> GetAsync()
        {
            var request = Connection.CreateRequest("realestate/{realEstateId}/attachment/attachmentsorder");
            request.AddParameter("realEstateId", RealEstate.Id, ParameterType.UrlSegment);
            return await ExecuteAsync<List>(Connection, request);
        }

        /// <summary>
        /// Update the order of uploaded <see cref="Attachment"/>s
        /// </summary>
        /// <param name="attachmentsOrder"></param>
        /// <returns></returns>
        public async Task UpdateAsync(List attachmentsOrder)
        {
            var request = Connection.CreateRequest("realestate/{realEstateId}/attachment/attachmentsorder", Method.PUT);
            request.AddParameter("realEstateId", RealEstate.Id, ParameterType.UrlSegment);
            request.AddBody(attachmentsOrder);
            var resp = await ExecuteAsync<Messages>(Connection, request);
            if (!resp.IsSuccessful())
            {
                throw new IS24Exception(string.Format("Error updating attachmentsorder for real estate {0}: {1}",
                    RealEstate.Id, resp.Message.ToMessage())) {Messages = resp};
            }
        }
    }
}