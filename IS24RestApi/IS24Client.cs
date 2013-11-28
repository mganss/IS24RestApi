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
    /// Represents an endpoint of the Immobilienscout24-REST-API.
    /// See http://developerwiki.immobilienscout24.de/wiki/ImmobilienScout24_API
    /// </summary>
    public class IS24Client
    {
        private readonly RealEstateResource realEstates;
        private readonly ContactResource contacts;
        private readonly AttachmentResource attachments;
        private readonly PublishResource publishResource;
        public const int ImmobilienscoutPublishChannelId = 10000;

        public IS24RestClient Is24RestClient { get; set; }

        public RealEstateResource RealEstates
        {
            get { return realEstates; }
        }

        public ContactResource Contacts
        {
            get { return contacts; }
        }

        public AttachmentResource Attachments
        {
            get { return attachments; }
        }

        public PublishResource Publish
        {
            get { return publishResource; }
        }

        /// <summary>
        /// Creates a new <see cref="IS24Client"/> instance
        /// </summary>
        /// <param name="restClient"></param>
        public IS24Client(IS24RestClient restClient)
        {
            this.Is24RestClient = restClient;
            realEstates = new RealEstateResource(this);
            contacts = new ContactResource(this);
            attachments = new AttachmentResource(this);
            publishResource = new PublishResource(this);
        }


        public long? ExtractNewId(messages resp)
        {
            if (resp.message != null && resp.message.Count() > 0 && resp.message[0].messageCode == MessageCode.MESSAGE_RESOURCE_CREATED)
            {
                var m = Regex.Match(resp.message[0].message, @"with id \[(\d+)\] has been created");
                if (m.Success) return long.Parse(m.Groups[1].Value);
            }

            return null;
        }

        public bool Ok(messages resp, MessageCode code = MessageCode.MESSAGE_RESOURCE_UPDATED)
        {
            return (resp.message != null && resp.message.Count() > 0 && resp.message[0].messageCode == code);
        }
    }
}
