namespace IS24RestApi
{
    /// <summary>
    /// Represents an endpoint of the Immobilienscout24-REST-API for importing and exporting real estate data
    /// See http://developerwiki.immobilienscout24.de/wiki/ImmobilienScout24_API
    /// </summary>
    public class ImportExportClient : IImportExportClient
    {
        /// <summary>
        /// Gets the default value for PublishChannel IS24
        /// </summary>
        public const int ImmobilienscoutPublishChannelId = 10000;

        /// <summary>
        /// Gets the underlying <see cref="Connection"/> which manages the RESTful calls
        /// </summary>
        public IIS24Connection Connection { get; private set; }

        /// <summary>
        /// Gets the <see cref="IRealEstateResource"/> to manage real estates
        /// </summary>
        public IRealEstateResource RealEstates { get; private set; }

        /// <summary>
        /// Gets the <see cref="IContactResource"/> managing the contacts
        /// </summary>
        public IContactResource Contacts { get; private set; }

        /// <summary>
        /// Gets the <see cref="IPublishChannelResource"/> accessing the user's publish channels
        /// </summary>
        public IPublishChannelResource PublishChannels { get; private set; }

        /// <summary>
        /// Gets the <see cref="PublishResource"/> for publishing real estates
        /// </summary>
        public PublishResource Publish { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ImportExportClient"/> instance
        /// </summary>
        /// <param name="connection"></param>
        public ImportExportClient(IIS24Connection connection)
        {
            Connection = connection;
            RealEstates = new RealEstateResource(Connection);
            Contacts = new ContactResource(Connection);
            Publish = new PublishResource(Connection);
            PublishChannels = new PublishChannelResource(Connection);
        }
    }
}
