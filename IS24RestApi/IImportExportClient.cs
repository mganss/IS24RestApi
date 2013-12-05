namespace IS24RestApi
{
    /// <summary>
    /// Describes an endpoint of the Immobilienscout24-REST-API for importing and exporting real estate data
    /// See http://developerwiki.immobilienscout24.de/wiki/ImmobilienScout24_API
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
        PublishResource Publish { get; }
    }
}