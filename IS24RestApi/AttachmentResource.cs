using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RestSharp;
using IS24RestApi.Common;
using IS24RestApi.Offer.RealEstates;

namespace IS24RestApi
{
    /// <summary>
    /// The resource responsible for managing attachments for real estates.
    /// </summary>
    public class AttachmentResource : IAttachmentResource
    {
        private readonly IIS24Connection is24Connection;

        /// <summary>
        /// Creates a new <see cref="AttachmentResource"/> instance
        /// </summary>
        /// <param name="is24Connection"></param>
        public AttachmentResource(IIS24Connection is24Connection)
        {
            this.is24Connection = is24Connection;
        }

        /// <summary>
        /// Gets all attachments of a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <returns>The attachment.</returns>
        public async Task<IEnumerable<Attachment>> GetAsync(RealEstate realEstate)
        {
            var req = is24Connection.CreateRequest("realestate/{id}/attachment");
            req.AddParameter("id", realEstate.Id, ParameterType.UrlSegment);
            var atts = await is24Connection.ExecuteAsync<Attachments>(req);
            return atts.Attachment;
        }

        /// <summary>
        /// Gets a single attachment identified by the specified id.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        /// <param name="id">The attachment id.</param>
        /// <returns>The attachment or null.</returns>
        public Task<Attachment> GetAsync(RealEstate re, string id)
        {
            var req = is24Connection.CreateRequest("realestate/{realEstate}/attachment/{id}");
            req.AddParameter("realEstate", re.Id, ParameterType.UrlSegment);
            req.AddParameter("id", id, ParameterType.UrlSegment);
            return is24Connection.ExecuteAsync<Attachment>(req);
        }

        /// <summary>
        /// Deletes an attachment identified by the specified id.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        /// <param name="id">The attachment id.</param>
        public async Task DeleteAsync(RealEstate re, string id)
        {
            var req = is24Connection.CreateRequest("realestate/{realEstate}/attachment/{id}", Method.DELETE);
            req.AddParameter("realEstate", re.Id, ParameterType.UrlSegment);
            req.AddParameter("id", id, ParameterType.UrlSegment);
            var resp = await is24Connection.ExecuteAsync<Messages>(req);
            if (!resp.IsSuccessful(MessageCode.MESSAGE_RESOURCE_DELETED))
            {
                throw new IS24Exception(string.Format("Error deleting attachment {0}: {1}", id, resp.Message.ToMessage())) { Messages = resp };
            }
        }

        /// <summary>
        /// Creates an attachment.
        /// </summary>
        /// <param name="re">The RealEstate.</param>
        /// <param name="att">The attachment.</param>
        /// <param name="path">The path to the attachment file.</param>
        public async Task CreateAsync(RealEstate re, Attachment att, string path)
        {
            var req = is24Connection.CreateRequest("realestate/{id}/attachment", Method.POST);
            req.AddParameter("id", re.Id, ParameterType.UrlSegment);
            var fileName = Path.GetFileName(path);
            if (fileName != null)
                req.AddFile("attachment", File.ReadAllBytes(path), fileName, MimeMapping.GetMimeMapping(fileName));

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Attachment), "http://rest.immobilienscout24.de/schema/common/1.0");

            var sw = new Utf8StringWriter();
            serializer.Serialize(sw, att);
            var metaData = Encoding.UTF8.GetBytes(sw.ToString());
            req.AddFile("metadata", metaData, "body.xml", "application/xml");

            var resp = await is24Connection.ExecuteAsync<Messages>(req);
            var id = resp.ExtractCreatedResourceId();

            if (!id.HasValue)
            {
                throw new IS24Exception(string.Format("Error creating attachment {0}: {1}", path, resp.Message.ToMessage())) { Messages = resp };
            }
            att.Id = id.Value;
            att.IdSpecified = true;
        }

        /// <summary>
        /// Updates an attachment.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        /// <param name="att">The attachment.</param>
        public async Task UpdateAsync(RealEstate re, Attachment att)
        {
            var req = is24Connection.CreateRequest("realestate/{realEstate}/attachment/{id}", Method.PUT);
            req.AddParameter("realEstate", re.Id, ParameterType.UrlSegment);
            req.AddParameter("id", att.Id, ParameterType.UrlSegment);
            req.AddBody(att, typeof(Attachment));

            var resp = await is24Connection.ExecuteAsync<Messages>(req);
            if (!resp.IsSuccessful())
            {
                throw new IS24Exception(string.Format("Error updating attachment {0}: {1}", att.Title, resp.Message.ToMessage())) { Messages = resp };
            }
        }
    }
}