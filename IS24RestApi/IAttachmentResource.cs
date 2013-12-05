using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the resource responsible for managing real estate's attachments
    /// </summary>
    public interface IAttachmentResource
    {
        /// <summary>
        /// Gets the <see cref="RealEstate"/> instance the attachments belong to
        /// </summary>
        RealEstate RealEstate { get; }

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
    }
}