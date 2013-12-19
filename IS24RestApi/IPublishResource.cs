using System.Threading.Tasks;
using IS24RestApi.Offer;
using IS24RestApi.Offer.RealEstates;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the resource responsible for publishing real estates
    /// </summary>
    public interface IPublishResource
    {
        /// <summary>
        /// Depublishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        Task UnpublishAsync(RealEstate realEstate);

        /// <summary>
        /// Publishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        Task PublishAsync(RealEstate realEstate);
    }
}