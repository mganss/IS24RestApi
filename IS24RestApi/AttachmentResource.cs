using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RestSharp;

namespace IS24RestApi
{
    public class AttachmentResource : IAttachmentResource
    {
        private IIS24Client importExportClient;

        public AttachmentResource(IIS24Client importExportClient)
        {
            this.importExportClient = importExportClient;
        }

        /// <summary>
        /// Gets all attachments of a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <returns>The attachment.</returns>
        public async Task<IEnumerable<Attachment>> GetAsync(RealEstate realEstate)
        {
            var req = importExportClient.Request("realestate/{id}/attachment");
            req.AddParameter("id", realEstate.id, ParameterType.UrlSegment);
            var atts = await importExportClient.ExecuteAsync<Attachments>(req);
            return atts.attachment;
        }

        /// <summary>
        /// Gets a single attachment identified by the specified id.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        /// <param name="id">The attachment id.</param>
        /// <returns>The attachment or null.</returns>
        public Task<Attachment> GetAsync(RealEstate re, string id)
        {
            var req = importExportClient.Request("realestate/{realEstate}/attachment/{id}");
            req.AddParameter("realEstate", re.id, ParameterType.UrlSegment);
            req.AddParameter("id", id, ParameterType.UrlSegment);
            return importExportClient.ExecuteAsync<Attachment>(req);
        }

        /// <summary>
        /// Deletes an attachment identified by the specified id.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        /// <param name="id">The attachment id.</param>
        public async Task DeleteAsync(RealEstate re, string id)
        {
            var req = importExportClient.Request("realestate/{realEstate}/attachment/{id}", Method.DELETE);
            req.AddParameter("realEstate", re.id, ParameterType.UrlSegment);
            req.AddParameter("id", id, ParameterType.UrlSegment);
            var resp = await importExportClient.ExecuteAsync<messages>(req);
            if (!resp.IsSuccessful(MessageCode.MESSAGE_RESOURCE_DELETED))
            {
                throw new IS24Exception(string.Format("Error deleting attachment {0}: {1}", id, resp.message.ToMessage())) { Messages = resp };
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
            var req = importExportClient.Request("realestate/{id}/attachment", Method.POST);
            req.AddParameter("id", re.id, ParameterType.UrlSegment);
            var fileName = Path.GetFileName(path);
            if (fileName != null)
                req.AddFile("attachment", File.ReadAllBytes(path), fileName, MimeMapping.GetMimeMapping(fileName));

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Attachment), "http://rest.immobilienscout24.de/schema/common/1.0");

            var sw = new Utf8StringWriter();
            serializer.Serialize(sw, att);
            var metaData = Encoding.UTF8.GetBytes(sw.ToString());
            req.AddFile("metadata", metaData, "body.xml", "application/xml");

            var resp = await importExportClient.ExecuteAsync<messages>(req);
            var id = resp.ExtractCreatedResourceId();

            if (!id.HasValue)
            {
                throw new IS24Exception(string.Format("Error creating attachment {0}: {1}", path, resp.message.ToMessage())) { Messages = resp };
            }
            att.id = id.Value;
            att.idSpecified = true;
        }

        /// <summary>
        /// Updates an attachment.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        /// <param name="att">The attachment.</param>
        public async Task UpdateAsync(RealEstate re, Attachment att)
        {
            var req = importExportClient.Request("realestate/{realEstate}/attachment/{id}", Method.PUT);
            req.AddParameter("realEstate", re.id, ParameterType.UrlSegment);
            req.AddParameter("id", att.id, ParameterType.UrlSegment);
            req.AddBody(att, typeof(Attachment));

            var resp = await importExportClient.ExecuteAsync<messages>(req);
            if (!resp.IsSuccessful())
            {
                throw new IS24Exception(string.Format("Error updating attachment {0}: {1}", att.title, resp.message.ToMessage())) { Messages = resp };
            }
        }
    }
}