using System;
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
        /// <summary>
        /// Gets the underlying <see cref="IIS24Connection"/> for executing the requests
        /// </summary>
        public IIS24Connection Connection { get; private set; }

        /// <summary>
        /// Gets the <see cref="IAttachmentResource.RealEstate"/> instance the attachments belong to
        /// </summary>
        public RealEstate RealEstate { get; private set; }

        /// <summary>
        /// Gets the resource responsible for managing the order of uploaded <see cref="Attachment"/>s
        /// </summary>
        public IAttachmentsOrderResource AttachmentsOrder { get; private set; }

        /// <summary>
        /// Creates a new <see cref="AttachmentResource"/> instance
        /// </summary>
        /// <param name="realEstate"></param>
        /// <param name="connection"></param>
        public AttachmentResource(RealEstate realEstate, IIS24Connection connection)
        {
            RealEstate = realEstate;
            Connection = connection;
            AttachmentsOrder = new AttachmentsOrderResource(realEstate, connection);
        }

        /// <summary>
        /// Gets all attachments of a RealEstate object.
        /// </summary>
        /// <returns>The attachment.</returns>
        public async Task<IEnumerable<Attachment>> GetAsync()
        {
            var request = Connection.CreateRequest("realestate/{id}/attachment");
            request.AddParameter("id", RealEstate.id, ParameterType.UrlSegment);
            var attachments = await Connection.ExecuteAsync<Attachments>(request);
            return attachments.attachment;
        }

        /// <summary>
        /// Gets a single attachment identified by the specified id.
        /// </summary>
        /// <param name="id">The attachment id.</param>
        /// <returns>The attachment or null.</returns>
        public Task<Attachment> GetAsync(string id)
        {
            var request = Connection.CreateRequest("realestate/{realEstate}/attachment/{id}");
            request.AddParameter("realEstate", RealEstate.id, ParameterType.UrlSegment);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            return Connection.ExecuteAsync<Attachment>(request);
        }

        /// <summary>
        /// Deletes an attachment identified by the specified id.
        /// </summary>
        /// <param name="id">The attachment id.</param>
        public async Task DeleteAsync(string id)
        {
            var request = Connection.CreateRequest("realestate/{realEstate}/attachment/{id}", Method.DELETE);
            request.AddParameter("realEstate", RealEstate.id, ParameterType.UrlSegment);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            var response = await Connection.ExecuteAsync<messages>(request);
            if (!response.IsSuccessful(MessageCode.MESSAGE_RESOURCE_DELETED))
            {
                throw new IS24Exception(string.Format("Error deleting attachment {0}: {1}", id, response.message.ToMessage())) { Messages = response };
            }
        }

        /// <summary>
        /// Creates a new <see cref="Attachment"/>
        /// </summary>
        /// <param name="attachment">The <see cref="Attachment"/> data</param>
        /// <param name="content">The content to be uploaded to IS24</param>
        /// <param name="fileName">The filename of the content transfered</param>
        /// <param name="mimeType">the mime-type of the file transfered</param>
        /// <returns>The updated <see cref="Attachment"/> data. It now contains the ScoutId if uploaded successfully</returns>
        public async Task<Attachment> CreateAsync(Attachment attachment, Stream content, string fileName, string mimeType)
        {
            var request = Connection.CreateRequest("realestate/{id}/attachment", Method.POST);
            request.AddParameter("id", RealEstate.id, ParameterType.UrlSegment);

            byte[] binaryContent = null;
            using (var reader = new BinaryReader(content))
            {
                binaryContent = reader.ReadBytes(Convert.ToInt32(content.Length));
            }

            request.AddFile("attachment", binaryContent, fileName, mimeType);
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Attachment), "http://rest.immobilienscout24.de/schema/common/1.0");

            var sw = new Utf8StringWriter();
            serializer.Serialize(sw, attachment);
            var metaData = Encoding.UTF8.GetBytes(sw.ToString());
            request.AddFile("metadata", metaData, "body.xml", "application/xml");

            var resp = await Connection.ExecuteAsync<messages>(request);
            var id = resp.ExtractCreatedResourceId();

            if (!id.HasValue)
            {
                throw new IS24Exception(string.Format("Error creating attachment {0}: {1}", fileName, resp.message.ToMessage())) { Messages = resp };
            }

            attachment.id = id.Value;
            attachment.idSpecified = true;

            return attachment;
        }

        /// <summary>
        /// Creates an attachment.
        /// </summary>
        /// <param name="att">The attachment.</param>
        /// <param name="path">The path to the attachment file.</param>
        public async Task<Attachment> CreateAsync(Attachment att, string path)
        {
            var fileName = Path.GetFileName(path);
            using (var stream = new FileStream(path, FileMode.Open))
            {
                if (fileName == null)
                {
                    throw new ArgumentException(string.Format("The file at path '{0}' is not available.", path));
                }

                return await CreateAsync(att, stream, fileName, MimeMapping.GetMimeMapping(fileName));
            }
        }

        /// <summary>
        /// Updates an attachment.
        /// </summary>
        /// <param name="att">The attachment.</param>
        public async Task UpdateAsync(Attachment att)
        {
            var req = Connection.CreateRequest("realestate/{realEstate}/attachment/{id}", Method.PUT);
            req.AddParameter("realEstate", RealEstate.id, ParameterType.UrlSegment);
            req.AddParameter("id", att.id, ParameterType.UrlSegment);
            req.AddBody(att, typeof(Attachment));

            var resp = await Connection.ExecuteAsync<messages>(req);
            if (!resp.IsSuccessful())
            {
                throw new IS24Exception(string.Format("Error updating attachment {0}: {1}", att.title, resp.message.ToMessage())) { Messages = resp };
            }
        }
    }
}