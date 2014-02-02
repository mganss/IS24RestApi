using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RestSharp;
using IS24RestApi.Common;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.VideoUpload;

namespace IS24RestApi
{
    /// <summary>
    /// The resource responsible for managing attachments for real estates.
    /// </summary>
    public class AttachmentResource : ImportExportResourceBase, IAttachmentResource
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
            var atts = await ExecuteAsync<Attachments>(is24Connection, req);
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
            return ExecuteAsync<Attachment>(is24Connection, req);
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
            var resp = await ExecuteAsync<Messages>(is24Connection, req);
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

            var resp = await ExecuteAsync<Messages>(is24Connection, req);
            var id = resp.ExtractCreatedResourceId();

            if (!id.HasValue)
            {
                throw new IS24Exception(string.Format("Error creating attachment {0}: {1}", path, resp.Message.ToMessage())) { Messages = resp };
            }
            att.Id = id.Value;
        }

        /// <summary>
        /// Creates a link attachment.
        /// </summary>
        /// <param name="re">The RealEstate.</param>
        /// <param name="link"></param>
        /// <returns></returns>
        /// <exception cref="IS24Exception"></exception>
        public async Task CreateAsync(RealEstate re, Link link)
        {
            var req = is24Connection.CreateRequest("realestate/{id}/attachment", Method.POST);
            req.AddParameter("id", re.Id, ParameterType.UrlSegment);
            req.AddBody(link, typeof(Attachment));            

            var resp = await ExecuteAsync<Messages>(is24Connection, req);
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
        /// <param name="re">The RealEstate object.</param>
        /// <param name="att">The attachment.</param>
        public async Task UpdateAsync(RealEstate re, Attachment att)
        {
            var req = is24Connection.CreateRequest("realestate/{realEstate}/attachment/{id}", Method.PUT);
            req.AddParameter("realEstate", re.Id, ParameterType.UrlSegment);
            req.AddParameter("id", att.Id, ParameterType.UrlSegment);
            req.AddBody(att, typeof(Attachment));

            var resp = await ExecuteAsync<Messages>(is24Connection, req);
            if (!resp.IsSuccessful())
            {
                throw new IS24Exception(string.Format("Error updating attachment {0}: {1}", att.Title, resp.Message.ToMessage())) { Messages = resp };
            }
        }

        /// <summary>
        /// Creates a video stream attachment (upload to http://www.screen9.com/).
        /// </summary>
        /// <param name="re">The RealEstate.</param>
        /// <param name="video">The attachment.</param>
        /// <param name="path">The path to the attachment file.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task CreateStreamingVideoAsync(RealEstate re, StreamingVideo video, string path)
        {
            // 1. Request a ticket for upload
            var req = is24Connection.CreateRequest("videouploadticket");
            var videoUploadTicket = await ExecuteAsync<VideoUploadTicket>(is24Connection, req);

            // 2. Upload your video to screen9
            var uploadClient = new RestClient();
            if (is24Connection.HttpFactory != null) uploadClient.HttpFactory = is24Connection.HttpFactory;
            req = new RestRequest(videoUploadTicket.UploadUrl);

            req.AddParameter("auth", videoUploadTicket.Auth);

            var fileName = Path.GetFileName(path);
            req.AddFile("videofile", File.ReadAllBytes(path), fileName, "application/octet-stream");

            var resp = await uploadClient.ExecutePostTaskAsync(req);
            if (resp.ErrorException != null) throw new IS24Exception(string.Format("Error uploading video {0}.", path), resp.ErrorException);
            if ((int)resp.StatusCode >= 400) throw new IS24Exception(string.Format("Error uploading video {0}: {1}", path, resp.StatusDescription));

            // 3. Post StreamingVideo attachment
            req = is24Connection.CreateRequest("realestate/{id}/attachment", Method.POST);
            req.AddParameter("id", re.Id, ParameterType.UrlSegment);
            video.VideoId = videoUploadTicket.VideoId;
            req.AddBody(video, typeof(Attachment));

            var msg = await ExecuteAsync<Messages>(is24Connection, req);
            var id = msg.ExtractCreatedResourceId();

            if (!id.HasValue)
            {
                throw new IS24Exception(string.Format("Error creating attachment {0}: {1}", path, msg.Message.ToMessage())) { Messages = msg };
            }
            video.Id = id.Value;
        }
    }
}