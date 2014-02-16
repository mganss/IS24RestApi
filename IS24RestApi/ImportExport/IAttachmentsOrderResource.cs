using IS24RestApi.AttachmentsOrder;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.Common;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the resource responsible for managing the order of attachments displayed on IS24
    /// </summary>
    public interface IAttachmentsOrderResource: IResource
    {
        /// <summary>
        /// Gets the <see cref="RealEstate"/> data
        /// </summary>
        RealEstate RealEstate { get; }

        /// <summary>
        /// Get the current order of uploaded <see cref="Attachment"/>s
        /// </summary>
        /// <returns></returns>
        Task<List> GetAsync();

        /// <summary>
        /// Update the order of uploaded <see cref="Attachment"/>s
        /// </summary>
        /// <param name="attachmentsOrder"></param>
        /// <returns></returns>
        Task UpdateAsync(List attachmentsOrder);
    }
}