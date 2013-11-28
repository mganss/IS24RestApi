using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace IS24RestApi
{
    /// <summary>
    /// Represents an endpoint of the Immobilienscout24-REST-API for importing and exporting real estate data
    /// See http://developerwiki.immobilienscout24.de/wiki/ImmobilienScout24_API
    /// </summary>
    public class ImportExportClient
    {
        public const int ImmobilienscoutPublishChannelId = 10000;

        /// <summary>
        /// Gets the underlying <see cref="Is24Client"/> which manages the RESTful calls
        /// </summary>
        public IIS24Client Is24Client { get; private set; }

        /// <summary>
        /// Gets the <see cref="RealEstateResource"/> to manage real estates
        /// </summary>
        public IRealEstateResource RealEstates { get; private set; }

        /// <summary>
        /// Gets the <see cref="ContactResource"/> managing the contacts
        /// </summary>
        public IContactResource Contacts { get; private set; }

        /// <summary>
        /// Gets the <see cref="AttachmentResource"/> managing attachments
        /// </summary>
        public IAttachmentResource Attachments { get; private set; }

        /// <summary>
        /// Gets the <see cref="publishResource"/> for publishing real estates
        /// </summary>
        public PublishResource Publish { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ImportExportClient"/> instance
        /// </summary>
        /// <param name="client"></param>
        public ImportExportClient(IS24Client client)
        {
            Is24Client = client;
            RealEstates = new RealEstateResource(Is24Client);
            Contacts = new ContactResource(Is24Client);
            Attachments = new AttachmentResource(Is24Client);
            Publish = new PublishResource(Is24Client);
        }
    }
}
