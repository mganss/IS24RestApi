using System.Threading.Tasks;
using IS24RestApi.Offer;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.Common;

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

        /// <summary>
        /// Depublishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <param name="channelId">The channelId of the channel to depublish from.</param>
        Task UnpublishAsync(RealEstate realEstate, int channelId);

        /// <summary>
        /// Publishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <param name="channelId">The channelId of the channel to publish to.</param>
        Task PublishAsync(RealEstate realEstate, int channelId);

        /// <summary>
        /// Depublishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <param name="publishChannel">The channel to depublish from.</param>
        Task UnpublishAsync(RealEstate realEstate, PublishChannel publishChannel);

        /// <summary>
        /// Publishes a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <param name="publishChannel">The channel to publish to.</param>
        Task PublishAsync(RealEstate realEstate, PublishChannel publishChannel);
    }
}