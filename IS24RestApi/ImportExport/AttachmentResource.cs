using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RestSharp;
using IS24RestApi.Common;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.VideoUpload;
using System;

namespace IS24RestApi
{
    /// <summary>
    /// The resource responsible for managing attachments for real estates.
    /// </summary>
    public class AttachmentResource : ImportExportResourceBase, IAttachmentResource
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
            request.AddParameter("id", RealEstate.Id, ParameterType.UrlSegment);
            var attachments = await ExecuteAsync<Attachments>(Connection, request);
            return attachments.Attachment;
        }

        /// <summary>
        /// Gets a single attachment identified by the specified id.
        /// </summary>
        /// <param name="id">The attachment id.</param>
        /// <returns>The attachment or null.</returns>
        public Task<Attachment> GetAsync(string id)
        {
            var request = Connection.CreateRequest("realestate/{realEstate}/attachment/{id}");
            request.AddParameter("realEstate", RealEstate.Id, ParameterType.UrlSegment);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            return ExecuteAsync<Attachment>(Connection, request);
        }

        /// <summary>
        /// Deletes an attachment identified by the specified id.
        /// </summary>
        /// <param name="id">The attachment id.</param>
        public async Task DeleteAsync(string id)
        {
            var request = Connection.CreateRequest("realestate/{realEstate}/attachment/{id}", Method.DELETE);
            request.AddParameter("realEstate", RealEstate.Id, ParameterType.UrlSegment);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            var response = await ExecuteAsync<Messages>(Connection, request);
            if (!response.IsSuccessful(MessageCode.MESSAGE_RESOURCE_DELETED))
            {
                throw new IS24Exception(string.Format("Error deleting attachment {0}: {1}", id, response.Message.ToMessage())) { Messages = response };
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
            request.AddParameter("id", RealEstate.Id, ParameterType.UrlSegment);

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

            var resp = await ExecuteAsync<Messages>(Connection, request);
            var id = resp.ExtractCreatedResourceId();

            if (!id.HasValue)
            {
                throw new IS24Exception(string.Format("Error creating attachment {0}: {1}", fileName, resp.Message.ToMessage())) { Messages = resp };
            }

            attachment.Id = id.Value;

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
        /// Creates a link attachment.
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        /// <exception cref="IS24Exception"></exception>
        public async Task CreateAsync(Link link)
        {
            var req = Connection.CreateRequest("realestate/{id}/attachment", Method.POST);
            req.AddParameter("id", RealEstate.Id, ParameterType.UrlSegment);
            req.AddBody(link, typeof(Attachment));            

            var resp = await ExecuteAsync<Messages>(Connection, req);
            var id = resp.ExtractCreatedResourceId();

            if (!id.HasValue)
            {
                throw new IS24Exception(string.Format("Error creating link attachment {0}: {1}", link.Url, resp.Message.ToMessage())) { Messages = resp };
            }

            link.Id = id.Value;
        }

        /// <summary>
        /// Updates an attachment.
        /// </summary>
        /// <param name="att">The attachment.</param>
        public async Task UpdateAsync(Attachment att)
        {
            var req = Connection.CreateRequest("realestate/{realEstate}/attachment/{id}", Method.PUT);
            req.AddParameter("realEstate", RealEstate.Id, ParameterType.UrlSegment);
            req.AddParameter("id", att.Id, ParameterType.UrlSegment);
            req.AddBody(att, typeof(Attachment));

            var resp = await ExecuteAsync<Messages>(Connection, req);
            if (!resp.IsSuccessful())
            {
                throw new IS24Exception(string.Format("Error updating attachment {0}: {1}", att.Title, resp.Message.ToMessage())) { Messages = resp };
            }
        }

        /// <summary>
        /// Creates a video stream attachment (upload to http://www.screen9.com/).
        /// </summary>
        /// <param name="video">The attachment.</param>
        /// <param name="path">The path to the attachment file.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task CreateStreamingVideoAsync(StreamingVideo video, string path)
        {
            // 1. Request a ticket for upload
            var req = Connection.CreateRequest("videouploadticket");
            var videoUploadTicket = await ExecuteAsync<VideoUploadTicket>(Connection, req);

            // 2. Upload your video to screen9
            var uploadClient = new RestClient(videoUploadTicket.UploadUrl);
            if (Connection.HttpFactory != null) uploadClient.HttpFactory = Connection.HttpFactory;
            req = new RestRequest();

            req.AddParameter("auth", videoUploadTicket.Auth);

            var fileName = Path.GetFileName(path);
            req.AddFile("videofile", File.ReadAllBytes(path), fileName, "application/octet-stream");

            var resp = await uploadClient.ExecutePostTaskAsync(req);
            if (resp.ErrorException != null) throw new IS24Exception(string.Format("Error uploading video {0}.", path), resp.ErrorException);
            if ((int)resp.StatusCode >= 400) throw new IS24Exception(string.Format("Error uploading video {0}: {1}", path, resp.StatusDescription));

            // 3. Post StreamingVideo attachment
            req = Connection.CreateRequest("realestate/{id}/attachment", Method.POST);
            req.AddParameter("id", RealEstate.Id, ParameterType.UrlSegment);
            video.VideoId = videoUploadTicket.VideoId;
            req.AddBody(video, typeof(Attachment));

            var msg = await ExecuteAsync<Messages>(Connection, req);
            var id = msg.ExtractCreatedResourceId();

            if (!id.HasValue)
            {
                throw new IS24Exception(string.Format("Error creating attachment {0}: {1}", path, msg.Message.ToMessage())) { Messages = msg };
            }

            video.Id = id.Value;
        }
    }
}