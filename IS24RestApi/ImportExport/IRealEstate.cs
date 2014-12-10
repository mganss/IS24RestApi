using IS24RestApi.Common;
using IS24RestApi.Offer.RealEstates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// Describes a real estate that was retrieved from the IS24 REST API.
    /// </summary>
    public interface IRealEstate
    {
        /// <summary>
        /// Get the <see cref="RealEstate"/> instance with data delivered from IS24
        /// </summary>
        RealEstate RealEstate { get; }

        /// <summary>
        /// Gets the <see cref="IAttachmentResource"/> for the real estate retrieved
        /// </summary>
        IAttachmentResource Attachments { get; }

        /// <summary>
        /// Gets the <see cref="IShowcasePlacementResource"/> for the real estate retrieved
        /// </summary>
        IShowcasePlacementResource ShowcasePlacements { get; }

        /// <summary>
        /// Gets the <see cref="IPremiumPlacementResource"/> for the real estate retrieved
        /// </summary>
        IPremiumPlacementResource PremiumPlacements { get; }
        
        /// <summary>
        /// Gets the <see cref="ITopPlacementResource"/> for the real estate retrieved
        /// </summary>
        ITopPlacementResource TopPlacements { get; }

        /// <summary>
        /// Publishes a RealEstate object.
        /// </summary>
        /// <param name="publishChannel">The channel to publish to.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task PublishAsync(PublishChannel publishChannel);

        /// <summary>
        /// Publishes a RealEstate object to the IS24 channel.
        /// </summary>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task PublishAsync();

        /// <summary>
        /// Depublishes a RealEstate object.
        /// </summary>
        /// <param name="publishChannel">The channel to depublish from.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task UnpublishAsync(PublishChannel publishChannel);

        /// <summary>
        /// Depublishes a RealEstate object from the IS24 channel.
        /// </summary>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task UnpublishAsync();
    }
}
