using System;
using System.Threading.Tasks;
using IS24RestApi.Offer;
using IS24RestApi.Offer.RealEstates;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the resource responsible for managing real estate data
    /// </summary>
    public interface IRealEstateResource : IResource
    {
        /// <summary>
        /// Get all RealEstate objects as an observable sequence.
        /// </summary>
        /// <returns>The RealEstate objects.</returns>
        IObservable<IRealEstate> GetAsync();

        /// <summary>
        /// Gets a single RealEstate object identified by the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="isExternal">true if the id is an external id.</param>
        /// <returns>The RealEstate object or null.</returns>
        Task<IRealEstate> GetAsync(string id, bool isExternal = false);

        /// <summary>
        /// Creates a RealEstate object.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        Task CreateAsync(RealEstate re);

        /// <summary>
        /// Updates a RealEstate object.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        Task UpdateAsync(RealEstate re);

        /// <summary>
        /// Deletes a RealEstate object. This seems to be possible if the real estate is not published.
        /// </summary>
        /// <param name="id">The id of the RealEstate object to be deleted.</param>
        Task DeleteAsync(string id);
    }
}