using IS24RestApi.Offer.PremiumPlacement;
using IS24RestApi.Offer.ShowcasePlacement;
using IS24RestApi.Offer.TopPlacement;
namespace IS24RestApi
{
    /// <summary>
    /// Describes an endpoint of the Immobilienscout24-REST-API for importing and exporting real estate data
    /// See http://api.immobilienscout24.de/our-apis/import-export.html
    /// </summary>
    public interface IImportExportClient
    {
        /// <summary>
        /// Gets the underlying <see cref="Connection"/> which manages the RESTful calls
        /// </summary>
        IIS24Connection Connection { get; }

        /// <summary>
        /// Gets the <see cref="RealEstateResource"/> to manage real estates
        /// </summary>
        IRealEstateResource RealEstates { get; }

        /// <summary>
        /// Gets the <see cref="ContactResource"/> managing the contacts
        /// </summary>
        IContactResource Contacts { get; }

        /// <summary>
        /// Gets the <see cref="IPublishChannelResource"/> accessing the user's publish channels
        /// </summary>
        IPublishChannelResource PublishChannels { get; }

        /// <summary>
        /// Gets the <see cref="PublishResource"/> for publishing real estates
        /// </summary>
        IPublishResource Publish { get; }

        /// <summary>
        /// Gets the <see cref="RealEstateProjectResource"/> for managing real estate projects.
        /// </summary>
        /// <value>
        /// The real estate projects.
        /// </value>
        IRealEstateProjectResource RealEstateProjects { get; }

        /// <summary>
        /// Gets the showcase placements.
        /// </summary>
        /// <value>
        /// The showcase placements.
        /// </value>
        IShowcasePlacementResource ShowcasePlacements { get; }

        /// <summary>
        /// Gets the premium placements.
        /// </summary>
        /// <value>
        /// The premium placements.
        /// </value>
        IPremiumPlacementResource PremiumPlacements { get; }

        /// <summary>
        /// Gets the top placements.
        /// </summary>
        /// <value>
        /// The top placements.
        /// </value>
        ITopPlacementResource TopPlacements { get; }
    }
}