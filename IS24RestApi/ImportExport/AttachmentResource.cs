using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Linq;
using IS24RestApi.Rest;
using IS24RestApi.Common;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.VideoUpload;
using System.Security.Cryptography;

#if NET40
using System.Reflection;
#endif

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
        /// Gets the <see cref="RealEstate"/> instance the attachments belong to
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
                throw new IS24Exception(string.Format("Error deleting attachment {0}: {1}", id, response.ToMessage())) { Messages = response };
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
            using (var ms = new MemoryStream())
            {
                await content.CopyToAsync(ms);
                binaryContent = ms.ToArray();
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
                throw new IS24Exception(string.Format("Error creating attachment {0}: {1}", fileName, resp.ToMessage())) { Messages = resp };
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
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                if (fileName == null)
                {
                    throw new ArgumentException(string.Format("The file at path '{0}' is not available.", path));
                }

                return await CreateAsync(att, stream, fileName, GetMimeMapping(fileName));
            }
        }

#if NET40
        private static MethodInfo MimeMappingMethod = GetMimeMappingMethod();

        private static MethodInfo GetMimeMappingMethod()
        {
            var assembly = Assembly.GetAssembly(typeof(HttpApplication));
            var mimeMappingType = assembly.GetType("System.Web.MimeMapping");
            var getMimeMappingMethod = mimeMappingType.GetMethod("GetMimeMapping",
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            return getMimeMappingMethod;
        }
#endif

        private static string GetMimeMapping(string fileName)
        {
#if NET40
            return (string)MimeMappingMethod.Invoke(null, new[] { fileName });
#else
            return MimeMapping.GetMimeMapping(fileName);
#endif
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
                throw new IS24Exception(string.Format("Error creating link attachment {0}: {1}", link.Url, resp.ToMessage())) { Messages = resp };
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
                throw new IS24Exception(string.Format("Error updating attachment {0}: {1}", att.Title, resp.ToMessage())) { Messages = resp };
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
            byte[] bytes = null;

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            using (var ms = new MemoryStream())
            {
                await fs.CopyToAsync(ms);
                bytes = ms.ToArray();
            }

            req.AddFile("videofile", bytes, fileName, "application/octet-stream");

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
                throw new IS24Exception(string.Format("Error creating attachment {0}: {1}", path, msg.ToMessage())) { Messages = msg };
            }

            video.Id = id.Value;
        }

        /// <summary>
        /// Synchronizes attachments. After synchronization, the corresponding real estate object
        /// has only the attachments specified in the parameter <paramref name="attachments"/> and in the given order.
        /// If a given attachment's <code>ExternalCheckSum</code> property is null or empty, this method calculates it using <see cref="Attachment.CalculateCheckSumAsync(string)"/>.
        /// Attachments are matched on either <see cref="Attachment.Id"/> or <see cref="Attachment.ExternalId"/>.
        /// If an attachment's <see cref="Attachment.ExternalId"/> is null or empty, one will be calculated by this method
        /// as the MD5 hash of its <see cref="Attachment.Title"/> and its path (non-<see cref="Link"/> objects).
        /// </summary>
        /// <param name="attachments">The attachments as an ordered list of key value pairs. The key is the <see cref="Attachment"/> object,
        /// the value is the path to the attachment's file (or null for <see cref="Link"/> objects).</param>
        public async Task UpdateAsync(IList<KeyValuePair<Attachment, string>> attachments)
        {
            await CalculateCheckSum(attachments);
            CalculateExternalId(attachments);
            var currentAttachments = (await GetAsync()).ToList();
            await DeleteUnused(attachments, currentAttachments);
            await UpdateOrCreate(attachments, currentAttachments);
            await UpdateOrder(attachments, currentAttachments);
        }

        private async Task UpdateOrder(IList<KeyValuePair<Attachment, string>> attachments, List<Attachment> currentAttachments)
        {
            // update order if it has changed
            // no need for an order if there's only one picture
            if (currentAttachments.OfType<Picture>().Count() > 1)
            {
                // take into account only pictures, position of other attachments is irrelevant
                var targetOrder = attachments.Select(a => a.Key).OfType<Picture>().Select(a => a.Id.Value).ToList();
                var currentOrder = (await AttachmentsOrder.GetAsync()).AttachmentId.Where(i => targetOrder.Contains(i));
                if (!currentOrder.SequenceEqual(targetOrder))
                {
                    var order = new IS24RestApi.AttachmentsOrder.List();
                    foreach (var id in attachments.Where(a => IsFileAttachment(a.Key)).Select(a => a.Key.Id.Value)) order.AttachmentId.Add(id);
                    await AttachmentsOrder.UpdateAsync(order);
                }
            }
        }

        private async Task UpdateOrCreate(IList<KeyValuePair<Attachment, string>> attachments, List<Attachment> currentAttachments)
        {
            foreach (var attachment in attachments)
            {
                var correspondingAttachment = currentAttachments.FirstOrDefault(c => EqualsIdentity(c, attachment.Key));
                if (correspondingAttachment != null)
                {
                    attachment.Key.Id = correspondingAttachment.Id; // maybe attachment has only ExternalId
                    // update attachment whose metadata has changed (currently only Title)
                    if (correspondingAttachment.Title != attachment.Key.Title)
                        await UpdateAsync(attachment.Key);
                }
                else
                {
                    // create attachment whose hash has changed or which wasn't previously there
                    if (attachment.Key is Link)
                        await CreateAsync((Link)attachment.Key);
                    else if (attachment.Key is StreamingVideo)
                        await CreateStreamingVideoAsync((StreamingVideo)attachment.Key, attachment.Value);
                    else
                        await CreateAsync(attachment.Key, attachment.Value);
                    currentAttachments.Add(attachment.Key);
                }
            }
        }

        private bool EqualsIdentity(Attachment a1, Attachment a2)
        {
            if (a1.Id == a2.Id || (a1.ExternalId != null && a1.ExternalId == a2.ExternalId))
                return true;
            if (a1 is Link && a2 is Link) return a1.Title == a2.Title && ((Link)a1).Url == ((Link)a2).Url;
            return false;
        }

        private bool Equals(Attachment a1, Attachment a2)
        {
            return EqualsIdentity(a1, a2) && EqualsContent(a1, a2);
        }

        private static bool EqualsContent(Attachment a1, Attachment a2)
        {
            if (a1 is Link && a2 is Link) return ((Link)a1).Url == ((Link)a2).Url;
            else return (a1.ExternalCheckSum != null && a1.ExternalCheckSum == a2.ExternalCheckSum);
        }

        private async Task DeleteUnused(IList<KeyValuePair<Attachment, string>> attachments, List<Attachment> currentAttachments)
        {
            // determine attachments that are no longer used or whose hash has changed
            var attachmentsToDelete = currentAttachments.Except(attachments.Select(a =>
                currentAttachments.FirstOrDefault(c => Equals(c, a.Key)))
                .Where(c => c != null))
                .ToList();

            foreach (var attachment in attachmentsToDelete)
            {
                await DeleteAsync(attachment.Id.ToString());
                currentAttachments.Remove(attachment);
            }
        }

        private static void CalculateExternalId(IList<KeyValuePair<Attachment, string>> attachments)
        {
            // set ExternalId for attachments that do not have it
            foreach (var kvp in attachments.Where(a => !string.IsNullOrEmpty(a.Value) && string.IsNullOrEmpty(a.Key.ExternalId)))
            {
                var id = string.Format("{0}:{1}", (kvp.Key.Title ?? ""), kvp.Value);
                using (var md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(id));
                    kvp.Key.ExternalId = string.Concat(Array.ConvertAll(hash, b => b.ToString("x2")));
                }
            }
        }

        private static async Task CalculateCheckSum(IList<KeyValuePair<Attachment, string>> attachments)
        {
            // calculate hash for those attachments that do not have it yet
            foreach (var kvp in attachments.Where(a => string.IsNullOrEmpty(a.Key.ExternalCheckSum) && !string.IsNullOrEmpty(a.Value)))
                await kvp.Key.CalculateCheckSumAsync(kvp.Value);
        }

        private bool IsFileAttachment(Attachment attachment)
        {
            return attachment is Picture || attachment is PDFDocument;
        }
    }
}