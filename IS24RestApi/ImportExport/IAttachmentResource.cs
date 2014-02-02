using IS24RestApi.Common;
using IS24RestApi.Offer.RealEstates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the resource responsible for managing real estate's attachments
    /// </summary>
    public interface IAttachmentResource
    {
        /// <summary>
        /// Gets all attachments of a RealEstate object.
        /// </summary>
        /// <param name="realEstate">The RealEstate object.</param>
        /// <returns>The attachment.</returns>
        Task<IEnumerable<Attachment>> GetAsync(RealEstate realEstate);

        /// <summary>
        /// Gets a single attachment identified by the specified id.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        /// <param name="id">The attachment id.</param>
        /// <returns>The attachment or null.</returns>
        Task<Attachment> GetAsync(RealEstate re, string id);

        /// <summary>
        /// Deletes an attachment identified by the specified id.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        /// <param name="id">The attachment id.</param>
        Task DeleteAsync(RealEstate re, string id);

        /// <summary>
        /// Creates an attachment.
        /// </summary>
        /// <param name="re">The RealEstate.</param>
        /// <param name="att">The attachment.</param>
        /// <param name="path">The path to the attachment file.</param>
        Task CreateAsync(RealEstate re, Attachment att, string path);

        /// <summary>
        /// Creates a video stream attachment (upload to http://www.screen9.com/).
        /// </summary>
        /// <param name="re">The RealEstate.</param>
        /// <param name="video">The attachment.</param>
        /// <param name="path">The path to the attachment file.</param>
        Task CreateStreamingVideoAsync(RealEstate re, StreamingVideo video, string path);

        /// <summary>
        /// Creates a link attachment.
        /// </summary>
        /// <param name="re">The RealEstate.</param>
        /// <param name="link">The link attachment.</param>
        Task CreateAsync(RealEstate re, Link link);

        /// <summary>
        /// Updates an attachment.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        /// <param name="att">The attachment.</param>
        Task UpdateAsync(RealEstate re, Attachment att);
    }
}