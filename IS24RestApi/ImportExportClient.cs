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
        /// Gets the underlying <see cref="Is24Connection"/> which manages the RESTful calls
        /// </summary>
        public IIS24Connection Is24Connection { get; private set; }

        /// <summary>
        /// Gets the <see cref="IRealEstateResource"/> to manage real estates
        /// </summary>
        public IRealEstateResource RealEstates { get; private set; }

        /// <summary>
        /// Gets the <see cref="IContactResource"/> managing the contacts
        /// </summary>
        public IContactResource Contacts { get; private set; }

        ///// <summary>
        ///// Gets the <see cref="IAttachmentResource"/> managing attachments
        ///// </summary>
        //public IAttachmentResource Attachments { get; private set; }

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
            Is24Connection = connection;
            RealEstates = new RealEstateResource(Is24Connection);
            Contacts = new ContactResource(Is24Connection);
            //Attachments = new AttachmentResource(Is24Connection);
            Publish = new PublishResource(Is24Connection);
            PublishChannels = new PublishChannelResource(Is24Connection);
        }
    }
}
