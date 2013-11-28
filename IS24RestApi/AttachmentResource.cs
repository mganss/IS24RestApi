using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RestSharp;

namespace IS24RestApi
{
    public class AttachmentResource
    {
        private IS24Client is24Client;

        public AttachmentResource(IS24Client is24Client)
        {
            this.is24Client = is24Client;
        }

        /// <summary>
        /// Gets all attachments of a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <returns>The attachment.</returns>
        public async Task<IEnumerable<Attachment>> GetAsync(RealEstate realEstate)
        {
            var req = is24Client.Is24RestClient.Request("realestate/{id}/attachment");
            req.AddParameter("id", realEstate.id, ParameterType.UrlSegment);
            var atts = await is24Client.Is24RestClient.ExecuteAsync<Attachments>(req);
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
            var req = is24Client.Is24RestClient.Request("realestate/{realEstate}/attachment/{id}");
            req.AddParameter("realEstate", re.id, ParameterType.UrlSegment);
            req.AddParameter("id", id, ParameterType.UrlSegment);
            return is24Client.Is24RestClient.ExecuteAsync<Attachment>(req);
        }

        /// <summary>
        /// Deletes an attachment identified by the specified id.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        /// <param name="id">The attachment id.</param>
        public async Task DeleteAsync(RealEstate re, string id)
        {
            var req = is24Client.Is24RestClient.Request("realestate/{realEstate}/attachment/{id}", Method.DELETE);
            req.AddParameter("realEstate", re.id, ParameterType.UrlSegment);
            req.AddParameter("id", id, ParameterType.UrlSegment);
            var resp = await is24Client.Is24RestClient.ExecuteAsync<messages>(req);
            if (!is24Client.Ok(resp, MessageCode.MESSAGE_RESOURCE_DELETED)) throw new IS24Exception(string.Format("Error deleting attachment {0}: {1}", id, resp.message.Msg())) { Messages = resp };
        }

        /// <summary>
        /// Creates an attachment.
        /// </summary>
        /// <param name="re">The RealEstate.</param>
        /// <param name="att">The attachment.</param>
        /// <param name="path">The path to the attachment file.</param>
        public async Task CreateAsync(RealEstate re, Attachment att, string path)
        {
            var req = is24Client.Is24RestClient.Request("realestate/{id}/attachment", Method.POST);
            req.AddParameter("id", re.id, ParameterType.UrlSegment);
            var fileName = Path.GetFileName(path);
            if (fileName != null)
                req.AddFile("attachment", File.ReadAllBytes(path), fileName, MimeMapping.GetMimeMapping(fileName));

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Attachment), "http://rest.immobilienscout24.de/schema/common/1.0");

            var sw = new Utf8StringWriter();
            serializer.Serialize(sw, att);
            var metaData = Encoding.UTF8.GetBytes(sw.ToString());
            req.AddFile("metadata", metaData, "body.xml", "application/xml");

            var resp = await is24Client.Is24RestClient.ExecuteAsync<messages>(req);
            var id = is24Client.ExtractNewId(resp);

            if (!id.HasValue) throw new IS24Exception(string.Format("Error creating attachment {0}: {1}", path, resp.message.Msg())) { Messages = resp };
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
            var req = is24Client.Is24RestClient.Request("realestate/{realEstate}/attachment/{id}", Method.PUT);
            req.AddParameter("realEstate", re.id, ParameterType.UrlSegment);
            req.AddParameter("id", att.id, ParameterType.UrlSegment);
            req.AddBody(att, typeof(Attachment));

            var resp = await is24Client.Is24RestClient.ExecuteAsync<messages>(req);
            if (!is24Client.Ok(resp)) throw new IS24Exception(string.Format("Error updating attachment {0}: {1}", att.title, resp.message.Msg())) { Messages = resp };
        }
    }
}