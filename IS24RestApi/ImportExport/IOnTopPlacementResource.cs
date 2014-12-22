using IS24RestApi.Offer.PremiumPlacement;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.Offer.ShowcasePlacement;
using IS24RestApi.Offer.TopPlacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the resource responsible for managing placements.
    /// <a href="http://api.immobilienscout24.de/our-apis/import-export/ontop-placement.html">API Documentation</a>.
    /// </summary>
    /// <typeparam name="T">The type of placements</typeparam>
    /// <typeparam name="V">The type of placement</typeparam>
    public interface IOnTopPlacementResource<T, V>: IResource
    {
        /// <summary>
        /// Gets the <see cref="RealEstate"/> instance the attachments belong to
        /// </summary>
        RealEstate RealEstate { get; }
        
        /// <summary>
        /// Creates the specified placements.
        /// </summary>
        /// <param name="placements">The placements.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task<T> CreateAsync(T placements);

        /// <summary>
        /// Creates a placement for the real estate identified by the specified id.
        /// </summary>
        /// <param name="id">The real estate id.</param>
        /// <param name="isExternal">true if the real estate id is an external id.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task CreateAsync(string id, bool isExternal = false);

        /// <summary>
        /// Creates a placement for the real estate object associated with this resource.
        /// </summary>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task CreateAsync();

        /// <summary>
        /// Gets all placements.
        /// </summary>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task<T> GetAllAsync();

        /// <summary>
        /// Gets the placement for the real estate identified by the specified id.
        /// </summary>
        /// <param name="id">The real estate id.</param>
        /// <param name="isExternal">true if the real estate id is an external id.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task<V> GetAsync(string id, bool isExternal = false);

        /// <summary>
        /// Gets the placement for the real estate associated with this resource.
        /// </summary>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task<V> GetAsync();

        /// <summary>
        /// Removes all placements.
        /// </summary>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task<T> RemoveAllAsync();

        /// <summary>
        /// Removes the placements for the real estate identified by the specified id.
        /// </summary>
        /// <param name="id">The real estate id.</param>
        /// <param name="isExternal">true if the real estate id is an external id.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task RemoveAsync(string id, bool isExternal = false);

        /// <summary>
        /// Removes the placements for the real estate associated with this resource.
        /// </summary>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task RemoveAsync();

        /// <summary>
        /// Removes the placements for the real estates identified by the specified ids.
        /// </summary>
        /// <param name="ids">The real estate ids.</param>
        /// <param name="isExternal">true if the real estate ids are external ids.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task<T> RemoveAsync(IEnumerable<string> ids, bool isExternal = false);
    }

    /// <summary>
    /// Describes premiumplacement ("Premium-Platzierung") resources.
    /// </summary>
    public interface IPremiumPlacementResource : IOnTopPlacementResource<Premiumplacements, Premiumplacement> { }

    /// <summary>
    /// Describes showcaseplacement ("Schaufenster-Platzierung") resources.
    /// </summary>
    public interface IShowcasePlacementResource : IOnTopPlacementResource<Showcaseplacements, Showcaseplacement> { }

    /// <summary>
    /// Describes topplacement ("Top-Platzierung") resources.
    /// </summary>
    public interface ITopPlacementResource : IOnTopPlacementResource<Topplacements, Topplacement> { }
}
