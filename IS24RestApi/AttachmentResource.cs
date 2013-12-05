using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RestSharp;

namespace IS24RestApi
{
    /// <summary>
    /// The resource responsible for managing attachments for real estates.
    /// </summary>
    public class AttachmentResource : IAttachmentResource
    {
        private readonly IIS24Connection connection;

        /// <summary>
        /// Creates a new <see cref="AttachmentResource"/> instance
        /// </summary>
        /// <param name="realEstate"></param>
        /// <param name="connection"></param>
        public AttachmentResource(RealEstate realEstate, IIS24Connection connection)
        {
            this.RealEstate = realEstate;
            this.connection = connection;
        }

        /// <summary>
        /// Gets the <see cref="IAttachmentResource.RealEstate"/> instance the attachments belong to
        /// </summary>
        public RealEstate RealEstate { get; private set; }

        /// <summary>
        /// Gets all attachments of a RealEstate object.
        /// </summary>
        /// <returns>The attachment.</returns>
        public async Task<IEnumerable<Attachment>> GetAsync()
        {
            var req = connection.CreateRequest("realestate/{id}/attachment");
            req.AddParameter("id", RealEstate.id, ParameterType.UrlSegment);
            var atts = await connection.ExecuteAsync<Attachments>(req);
            return atts.attachment;
        }

        /// <summary>
        /// Gets a single attachment identified by the specified id.
        /// </summary>
        /// <param name="id">The attachment id.</param>
        /// <returns>The attachment or null.</returns>
        public Task<Attachment> GetAsync(string id)
        {
            var req = connection.CreateRequest("realestate/{realEstate}/attachment/{id}");
            req.AddParameter("realEstate", RealEstate.id, ParameterType.UrlSegment);
            req.AddParameter("id", id, ParameterType.UrlSegment);
            return connection.ExecuteAsync<Attachment>(req);
        }

        /// <summary>
        /// Deletes an attachment identified by the specified id.
        /// </summary>
        /// <param name="id">The attachment id.</param>
        public async Task DeleteAsync(string id)
        {
            var req = connection.CreateRequest("realestate/{realEstate}/attachment/{id}", Method.DELETE);
            req.AddParameter("realEstate", RealEstate.id, ParameterType.UrlSegment);
            req.AddParameter("id", id, ParameterType.UrlSegment);
            var resp = await connection.ExecuteAsync<messages>(req);
            if (!resp.IsSuccessful(MessageCode.MESSAGE_RESOURCE_DELETED))
            {
                throw new IS24Exception(string.Format("Error deleting attachment {0}: {1}", id, resp.message.ToMessage())) { Messages = resp };
            }
        }

        /// <summary>
        /// Creates an attachment.
        /// </summary>
        /// <param name="att">The attachment.</param>
        /// <param name="path">The path to the attachment file.</param>
        public async Task CreateAsync(Attachment att, string path)
        {
            var req = connection.CreateRequest("realestate/{id}/attachment", Method.POST);
            req.AddParameter("id", RealEstate.id, ParameterType.UrlSegment);
            var fileName = Path.GetFileName(path);
            if (fileName != null)
                req.AddFile("attachment", File.ReadAllBytes(path), fileName, MimeMapping.GetMimeMapping(fileName));

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Attachment), "http://rest.immobilienscout24.de/schema/common/1.0");

            var sw = new Utf8StringWriter();
            serializer.Serialize(sw, att);
            var metaData = Encoding.UTF8.GetBytes(sw.ToString());
            req.AddFile("metadata", metaData, "body.xml", "application/xml");

            var resp = await connection.ExecuteAsync<messages>(req);
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
        /// <param name="att">The attachment.</param>
        public async Task UpdateAsync(Attachment att)
        {
            var req = connection.CreateRequest("realestate/{realEstate}/attachment/{id}", Method.PUT);
            req.AddParameter("realEstate", RealEstate.id, ParameterType.UrlSegment);
            req.AddParameter("id", att.id, ParameterType.UrlSegment);
            req.AddBody(att, typeof(Attachment));

            var resp = await connection.ExecuteAsync<messages>(req);
            if (!resp.IsSuccessful())
            {
                throw new IS24Exception(string.Format("Error updating attachment {0}: {1}", att.title, resp.message.ToMessage())) { Messages = resp };
            }
        }
    }
}