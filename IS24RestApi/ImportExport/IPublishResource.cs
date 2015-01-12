using System.Threading.Tasks;
using IS24RestApi.Offer;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.Common;
using System.Collections.Generic;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the resource responsible for publishing real estates
    /// </summary>
    public interface IPublishResource : IResource
    {
        /// <summary>
        /// Gets all <see cref="PublishObject"/>s the user has access to matching the given parameters
        /// </summary>
        /// <param name="channelId">The channelId of the channel to depublish from.</param>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <exception cref="IS24Exception"></exception>
        Task<PublishObjects> GetAsync(RealEstate realEstate, int? channelId = null);

        /// <summary>
        /// Depublishes a RealEstate object from the IS24 channel.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        Task UnpublishAsync(RealEstate realEstate);

        /// <summary>
        /// Publishes a RealEstate object to the IS24 channel.
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

        /// <summary>
        /// Publishes a list of RealEstate objects.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="channelId">The channelId of the channel to publish to.</param>
        /// <exception cref="IS24Exception"></exception>
        Task<PublishObjects> PublishAsync(IEnumerable<RealEstate> realEstates, int channelId);

        /// <summary>
        /// Publishes a list of RealEstate objects.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="publishChannel">The channel to publish to.</param>  
        Task<PublishObjects> PublishAsync(IEnumerable<RealEstate> realEstates, PublishChannel publishChannel);

        /// <summary>
        /// Publishes a list of RealEstate objects to the specified channels.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="channelIds">The channelIds of the channels to publish to.</param>
        Task<PublishObjects> PublishAsync(IEnumerable<RealEstate> realEstates, IEnumerable<int> channelIds);

        /// <summary>
        /// Publishes a list of RealEstate objects to the specified channels.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="publishChannels">The channels to publish to.</param>  
        Task<PublishObjects> PublishAsync(IEnumerable<RealEstate> realEstates, IEnumerable<PublishChannel> publishChannels);

        /// <summary>
        /// Publishes a list of RealEstate objects to the IS24 channel.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        Task<PublishObjects> PublishAsync(IEnumerable<RealEstate> realEstates);

        /// <summary>
        /// Depublishes a list of RealEstate objects.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="channelId">The channelId of the channel to depublish from.</param>
        Task<PublishObjects> UnpublishAsync(IEnumerable<RealEstate> realEstates, int channelId);

        /// <summary>
        /// Depublishes a list of RealEstate objects.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="publishChannel">The channel to depublish from.</param>
        Task<PublishObjects> UnpublishAsync(IEnumerable<RealEstate> realEstates, PublishChannel publishChannel);

        /// <summary>
        /// Depublishes a list of RealEstate objects from the specified channels.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="channelIds">The channelIds of the channels to depublish from.</param>
        Task<PublishObjects> UnpublishAsync(IEnumerable<RealEstate> realEstates, IEnumerable<int> channelIds);

        /// <summary>
        /// Depublishes a list of RealEstate objects from the specified channels.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        /// <param name="publishChannels">The channels to depublish from.</param>  
        Task<PublishObjects> UnpublishAsync(IEnumerable<RealEstate> realEstates, IEnumerable<PublishChannel> publishChannels);

        /// <summary>
        /// Depublishes a list of RealEstate objects from the IS24 channel.
        /// </summary>
        /// <param name="realEstates">The RealEstate objects.</param>
        Task<PublishObjects> UnpublishAsync(IEnumerable<RealEstate> realEstates);
    }
}