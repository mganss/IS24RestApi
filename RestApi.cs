using IS24;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace IS24RestApi
{
    /// <summary>
    /// Represents an endpoint of the Immobilienscout24-REST-API.
    /// See http://developerwiki.immobilienscout24.de/wiki/ImmobilienScout24_API
    /// </summary>
    public class RestApi
    {
        const string User = "me";

        /// <summary>
        /// The OAuth ConsumerSecret
        /// </summary>
        public string ConsumerSecret { get; set; }
        /// <summary>
        /// The OAuth ConsumerKey
        /// </summary>
        public string ConsumerKey { get; set; }
        /// <summary>
        /// The OAuth AccessToken
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// The OAuth AccessTokenSecret
        /// </summary>
        public string AccessTokenSecret { get; set; }
        /// <summary>
        /// The common URL prefix of all resources (e.g. "http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0").
        /// </summary>
        public string BaseUrlPrefix { get; set; }

        /// <summary>
        /// The XML deserializer
        /// </summary>
        public static IDeserializer XmlDeserializer = new BaseXmlDeserializer();
        /// <summary>
        /// The XML serializer
        /// </summary>
        public static ISerializer XmlSerializer = new BaseXmlSerializer();

        /// <summary>
        /// The URL prefix including the user part
        /// </summary>
        public string BaseUrl
        {
            get
            {
                return string.Format("{0}/user/{1}/", BaseUrlPrefix, Uri.EscapeDataString(User));
            }
        }

        private RestRequest Request(string resource, Method method = Method.GET)
        {
            return new RestRequest(resource, method) { XmlSerializer = XmlSerializer };
        }

        /// <summary>
        /// Performs an API request.
        /// </summary>
        /// <typeparam name="T">The type of the response object</typeparam>
        /// <param name="request">The request</param>
        /// <param name="baseUrl">The suffix added to <see cref="BaseUrlPrefix"/> to obtain the request URL. If null, <see cref="BaseUrl"/> will be used.</param>
        /// <returns></returns>
        private T Execute<T>(RestRequest request, string baseUrl = null) where T: new()
        {
            baseUrl = baseUrl == null ? BaseUrl : string.Join("/", BaseUrlPrefix, baseUrl);
            var client = new RestClient(baseUrl);
            client.Authenticator = OAuth1Authenticator.ForProtectedResource(ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret);
            client.AddHandler("application/xml", XmlDeserializer);
            request.XmlSerializer = XmlSerializer;

            var response = client.Execute<T>(request);

            if (response.ErrorException != null) throw response.ErrorException;

            return response.Data;
        }

        /// <summary>
        /// A <see cref="StringWriter"/> that uses <see cref="System.Text.Encoding.UTF8"/>.
        /// </summary>
        public sealed class Utf8StringWriter : StringWriter
        {
            /// <summary>
            /// The encoding.
            /// </summary>
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }

        /// <summary>
        /// Get all RealEstate objects.
        /// </summary>
        /// <returns>The RealEstate objects.</returns>
        public IEnumerable<RealEstate> GetRealEstates()
        {
            var page = 1;

            while (true)
            {
                var req = Request("realestate");
                req.AddParameter("pagesize", 100);
                req.AddParameter("pagenumber", page);
                var rel = Execute<realEstates>(req);

                foreach (var ore in rel.realEstateList)
                {
                    var oreq = Request("realestate/{id}");
                    oreq.AddParameter("id", ore.id, ParameterType.UrlSegment);
                    var re = Execute<RealEstate>(oreq);
                    yield return re;
                }

                if (page >= rel.Paging.numberOfPages) break;
                page++;
            }
        }

        /// <summary>
        /// Gets a single RealEstate object identified by the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="isExternal">true if the id is an external id.</param>
        /// <returns>The RealEstate object or null.</returns>
        public RealEstate GetRealEstate(string id, bool isExternal = false)
        {
            var req = Request("realestate/{id}");
            req.AddParameter("id", isExternal ? "ext-" + id : id, ParameterType.UrlSegment);
            return Execute<RealEstate>(req);
        }

        /// <summary>
        /// Creates a RealEstate object.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        public void CreateRealEstate(RealEstate re)
        {
            var req = Request("realestate", Method.POST);
            req.AddBody(re);
            var resp = Execute<messages>(req);
            var id = ExtractNewId(resp);
            if (!id.HasValue) throw new Exception(string.Format("Error creating RealEstate {0}: {1}", re.externalId, resp.message.Msg()));
            re.id = id.Value;
            re.idSpecified = true;
        }

        /// <summary>
        /// Updates a RealEstate object.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        public void UpdateRealEstate(RealEstate re)
        {
            var req = Request("realestate/{id}", Method.PUT);
            req.AddParameter("id", re.id, ParameterType.UrlSegment);
            req.AddBody(re);
            var resp = Execute<messages>(req);
            if (!Ok(resp)) throw new Exception(string.Format("Error updating RealEstate {0}: {1}", re.externalId, resp.message.Msg()));
        }

        /// <summary>
        /// Gets all contacts.
        /// </summary>
        /// <returns>The contacts.</returns>
        public IEnumerable<RealtorContactDetails> GetContacts()
        {
            var contacts = Execute<realtorContactDetailsList>(Request("contact"));
            return contacts.realtorContactDetails;
        }

        /// <summary>
        /// Gets a single contact identified by the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="isExternal">true if the id is an external id.</param>
        /// <returns>The contact or null.</returns>
        public RealtorContactDetails GetContact(string id, bool isExternal = false)
        {
            var req = Request("contact/{id}");
            req.AddParameter("id", isExternal ? "ext-" + id : id, ParameterType.UrlSegment);
            return Execute<RealtorContactDetails>(req);
        }

        /// <summary>
        /// Creates a contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public void CreateContact(RealtorContactDetails contact)
        {
            var req = Request("contact", Method.POST);
            req.AddBody(contact);

            var resp = Execute<messages>(req);
            var id = ExtractNewId(resp);
            if (!id.HasValue) throw new Exception(string.Format("Error creating contact {0}: {1}", contact.lastname, resp.message.Msg()));
            contact.id = id.Value;
            contact.idSpecified = true;
        }

        /// <summary>
        /// Updates a contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public void UpdateContact(RealtorContactDetails contact)
        {
            var req = Request("contact/{id}", Method.PUT);
            req.AddParameter("id", contact.id, ParameterType.UrlSegment);
            req.AddBody(contact);

            var resp = Execute<messages>(req);
            if (!Ok(resp)) throw new Exception(string.Format("Error updating contact {0}: {1}", contact.lastname, resp.message.Msg()));
        }

        /// <summary>
        /// Gets all attachments of a RealEstate object.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        /// <returns>The attachment.</returns>
        public IEnumerable<Attachment> GetAttachments(RealEstate re)
        {
            var req = Request("realestate/{id}/attachment");
            req.AddParameter("id", re.id, ParameterType.UrlSegment);
            var atts = Execute<Attachments>(req);
            return atts.attachment;
        }

        /// <summary>
        /// Gets a single attachment identified by the specified id.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        /// <param name="id">The attachment id.</param>
        /// <returns>The attachment or null.</returns>
        public Attachment GetAttachment(RealEstate re, string id)
        {
            var req = Request("realestate/{re}/attachment/{id}");
            req.AddParameter("re", re.id, ParameterType.UrlSegment);
            req.AddParameter("id", id, ParameterType.UrlSegment);
            var a = Execute<Attachment>(req);
            return a;
        }

        /// <summary>
        /// Deletes an attachment identified by the specified id.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        /// <param name="id">The attachment id.</param>
        public void DeleteAttachment(RealEstate re, string id)
        {
            var req = Request("realestate/{re}/attachment/{id}", Method.DELETE);
            req.AddParameter("re", re.id, ParameterType.UrlSegment);
            req.AddParameter("id", id, ParameterType.UrlSegment);
            var resp = Execute<messages>(req);
            if (!Ok(resp, MessageCode.MESSAGE_RESOURCE_DELETED)) throw new Exception(string.Format("Error deleting attachment {0}: {1}", id, resp.message.Msg()));
        }

        /// <summary>
        /// Creates an attachment.
        /// </summary>
        /// <param name="re">The RealEstate.</param>
        /// <param name="att">The attachment.</param>
        /// <param name="path">The path to the attachment file.</param>
        public void CreateAttachment(RealEstate re, Attachment att, string path)
        {
            var req = Request("realestate/{id}/attachment", Method.POST);
            req.AddParameter("id", re.id, ParameterType.UrlSegment);
            var fileName = Path.GetFileName(path);
            req.AddFile("attachment", File.ReadAllBytes(path), fileName, MimeMapping.GetMimeMapping(fileName));

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Attachment), "http://rest.immobilienscout24.de/schema/common/1.0");
            var sw = new Utf8StringWriter();
            serializer.Serialize(sw, att);
            var metaData = Encoding.UTF8.GetBytes(sw.ToString());
            req.AddFile("metadata", metaData, "body.xml", "application/xml");

            var resp = Execute<messages>(req);
            var id = ExtractNewId(resp);

            if (!id.HasValue) throw new Exception(string.Format("Error creating attachment {0}: {1}", path, resp.message.Msg()));
            att.id = id.Value;
            att.idSpecified = true;
        }

        /// <summary>
        /// Updates an attachment.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        /// <param name="att">The attachment.</param>
        public void UpdateAttachment(RealEstate re, Attachment att)
        {
            var req = Request("realestate/{re}/attachment/{id}", Method.PUT);
            req.AddParameter("re", re.id, ParameterType.UrlSegment);
            req.AddParameter("id", att.id, ParameterType.UrlSegment);
            req.AddBody(att, typeof(Attachment));

            var resp = Execute<messages>(req);
            if (!Ok(resp)) throw new Exception(string.Format("Error updating attachment {0}: {1}", att.title, resp.message.Msg()));
        }

        const int IS24Channel = 10000;

        /// <summary>
        /// Depublishes a RealEstate object.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        public void Depublish(RealEstate re)
        {
            var req = Request("publish");
            req.AddParameter("realestate", re.id);
            req.AddParameter("publishchannel", IS24Channel);
            var pos = Execute<publishObjects>(req, "");

            if (pos.publishObject != null && pos.publishObject.Any())
            {
                req = Request("publish/{id}", Method.DELETE);
                req.AddParameter("id", pos.publishObject[0].id, ParameterType.UrlSegment);
                var pres = Execute<messages>(req, "");
                if (!Ok(pres, MessageCode.MESSAGE_RESOURCE_DELETED)) throw new Exception(string.Format("Error depublishing RealEstate {0}: {1}", re.externalId, pres.message.Msg()));
            }
        }

        /// <summary>
        /// Publishes a RealEstate object.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        public void Publish(RealEstate re)
        {
            var req = Request("publish");
            req.AddParameter("realestate", re.id);
            req.AddParameter("publishchannel", IS24Channel);
            var pos = Execute<publishObjects>(req, "");

            if (pos.publishObject == null || !pos.publishObject.Any())
            {
                req = Request("publish", Method.POST);
                var p = new PublishObject
                {
                    publishChannel = new PublishChannel { id = IS24Channel, idSpecified = true },
                    realEstate = new PublishObjectRealEstate { id = re.id, idSpecified = true }
                };
                req.AddBody(p);
                var pres = Execute<messages>(req, "");
                if (!Ok(pres, MessageCode.MESSAGE_RESOURCE_CREATED)) throw new Exception(string.Format("Error publishing RealEstate {0}: {1}", re.externalId, pres.message.Msg()));
            }
        }

        long? ExtractNewId(messages resp)
        {
            if (resp.message != null && resp.message.Count() > 0 && resp.message[0].messageCode == MessageCode.MESSAGE_RESOURCE_CREATED)
            {
                var m = Regex.Match(resp.message[0].message, @"with id \[(\d+)\] has been created");
                if (m.Success) return long.Parse(m.Groups[1].Value);
            }

            return null;
        }

        bool Ok(messages resp, MessageCode code = MessageCode.MESSAGE_RESOURCE_UPDATED)
        {
            return (resp.message != null && resp.message.Count() > 0 && resp.message[0].messageCode == code);
        }
    }
}
