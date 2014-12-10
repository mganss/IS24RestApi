using IS24RestApi.Common;
using IS24RestApi.Offer.RealEstates;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// The <see cref="RealEstateItem"/> wraps the <see cref="RealEstate"/> data item together with the resource 
    /// responsible for the attachments for this real estate
    /// </summary>
    public class RealEstateItem : IRealEstate
    {
        /// <summary>
        /// Get the <see cref="IRealEstate.RealEstate"/> instance with data delivered from IS24
        /// </summary>
        public RealEstate RealEstate { get; private set; }

        /// <summary>
        /// Gets the <see cref="IAttachmentResource"/> for the real estate retrieved
        /// </summary>
        public IAttachmentResource Attachments { get; private set; }

        /// <summary>
        /// Gets the <see cref="IPremiumPlacementResource"/> for the real estate retrieved
        /// </summary>
        public IPremiumPlacementResource PremiumPlacements { get; private set; }

        /// <summary>
        /// Gets the <see cref="ITopPlacementResource"/> for the real estate retrieved
        /// </summary>
        public ITopPlacementResource TopPlacements { get; private set; }

        /// <summary>
        /// Gets the <see cref="IShowcasePlacementResource"/> for the real estate retrieved
        /// </summary>
        public IShowcasePlacementResource ShowcasePlacements { get; private set; }

        private IPublishResource Publish { get; set; }

        /// <summary>
        /// Publishes a RealEstate object.
        /// </summary>
        /// <param name="publishChannel">The channel to publish to.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task PublishAsync(PublishChannel publishChannel)
        {
            return Publish.PublishAsync(RealEstate, publishChannel);
        }

        /// <summary>
        /// Publishes a RealEstate object to the IS24 channel.
        /// </summary>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task PublishAsync()
        {
            return Publish.PublishAsync(RealEstate);
        }

        /// <summary>
        /// Depublishes a RealEstate object.
        /// </summary>
        /// <param name="publishChannel">The channel to depublish from.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task UnpublishAsync(PublishChannel publishChannel)
        {
            return Publish.UnpublishAsync(RealEstate, publishChannel);
        }

        /// <summary>
        /// Depublishes a RealEstate object from the IS24 channel.
        /// </summary>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task UnpublishAsync()
        {
            return Publish.UnpublishAsync(RealEstate);
        }

        /// <summary>
        /// Creates a new <see cref="RealEstateItem"/> instance
        /// </summary>
        /// <param name="realEstate"><see cref="RealEstate"/> data item</param>
        /// <param name="connection">The <see cref="IIS24Connection"/> used for querying the API</param>
        public RealEstateItem(RealEstate realEstate, IIS24Connection connection)
        {
            RealEstate = realEstate;
            Attachments = new AttachmentResource(realEstate, connection);
            Publish = new PublishResource(connection);
            PremiumPlacements = new PremiumPlacementResource(connection, realEstate);
            TopPlacements = new TopPlacementResource(connection, realEstate);
            ShowcasePlacements = new ShowcasePlacementResource(connection, realEstate);
        }
    }
}