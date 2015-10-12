using IS24RestApi.Common;
using IS24RestApi.Offer.RealEstates;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the resource responsible for managing real estate's attachments
    /// </summary>
    public interface IAttachmentResource : IResource
    {
        /// <summary>
        /// Gets the <see cref="RealEstate"/> instance the attachments belong to
        /// </summary>
        RealEstate RealEstate { get; }

        /// <summary>
        /// Gets the resource responsible for managing the order of uploaded <see cref="Attachment"/>s
        /// </summary>
        IAttachmentsOrderResource AttachmentsOrder { get; }

        /// <summary>
        /// Gets all attachments of a RealEstate object.
        /// </summary>
        /// <returns>The attachment.</returns>
        Task<IEnumerable<Attachment>> GetAsync();

        /// <summary>
        /// Gets a single attachment identified by the specified id.
        /// </summary>
        /// <param name="id">The attachment id.</param>
        /// <returns>The attachment or null.</returns>
        Task<Attachment> GetAsync(string id);

        /// <summary>
        /// Deletes an attachment identified by the specified id.
        /// </summary>
        /// <param name="id">The attachment id.</param>
        Task DeleteAsync(string id);

        /// <summary>
        /// Creates an attachment.
        /// </summary>
        /// <param name="att">The attachment.</param>
        /// <param name="path">The path to the attachment file.</param>
        Task<Attachment> CreateAsync(Attachment att, string path);

        /// <summary>
        /// Creates a video stream attachment (upload to http://www.screen9.com/).
        /// </summary>
        /// <param name="video">The attachment.</param>
        /// <param name="path">The path to the attachment file.</param>
        Task CreateStreamingVideoAsync(StreamingVideo video, string path);

        /// <summary>
        /// Creates a link attachment.
        /// </summary>
        /// <param name="link">The link attachment.</param>
        Task CreateAsync(Link link);

        /// <summary>
        /// Updates an attachment.
        /// </summary>
        /// <param name="att">The attachment.</param>
        Task UpdateAsync(Attachment att);

        /// <summary>
        /// Creates a new <see cref="Attachment"/>
        /// </summary>
        /// <param name="attachment">The <see cref="Attachment"/> data</param>
        /// <param name="content">The content to be uploaded to IS24</param>
        /// <param name="fileName">The filename of the content transfered</param>
        /// <param name="mimeType">the mime-type of the file transfered</param>
        /// <returns>The updated <see cref="Attachment"/> data. It now contains the ScoutId if uploaded successfully</returns>
        Task<Attachment> CreateAsync(Attachment attachment, Stream content, string fileName, string mimeType);

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
        Task UpdateAsync(IList<KeyValuePair<Attachment, string>> attachments);
    }
}