using IS24RestApi.Offer.RealEstateProject;
using IS24RestApi.Offer.RealEstates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the resource responsible for managing real estate projects.
    /// <a href="http://api.immobilienscout24.de/our-apis/import-export/realestateproject.html">API Documentation</a>.
    /// </summary>
    public interface IRealEstateProjectResource: IResource
    {
        /// <summary>
        /// Gets all real estate projects.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<RealEstateProjects> GetAllAsync();

        /// <summary>
        /// Gets all real real estate objects belonging to the real estate project identified by the specified id.
        /// </summary>
        /// <param name="realEstateProjectId">The id.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<RealEstateProjectEntries> GetAllAsync(long realEstateProjectId);

        /// <summary>
        /// Gets a real estate project identified by the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<RealEstateProject> GetAsync(long id);

        /// <summary>
        /// Updates a real estate project.
        /// </summary>
        /// <param name="realEstateProject">The <see cref="IS24RestApi.Offer.RealEstateProject.RealEstateProject" /> object.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task UpdateAsync(RealEstateProject realEstateProject);

        /// <summary>
        /// Adds real estate objects to the real estate project identified by the specified id.
        /// </summary>
        /// <param name="realEstateProjectId">The id.</param>
        /// <param name="entries">Identifies the real estate objects.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task<RealEstateProjectEntries> AddAsync(long realEstateProjectId, RealEstateProjectEntries entries);

        /// <summary>
        /// Adds real estate objects to the real estate project identified by the specified id.
        /// </summary>
        /// <param name="realEstateProjectId">The id.</param>
        /// <param name="realEstates">Identifies the real estate objects.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task<RealEstateProjectEntries> AddAsync(long realEstateProjectId, IEnumerable<RealEstate> realEstates);

        /// <summary>
        /// Adds a real estate object to the real estate project identified by the specified id.
        /// </summary>
        /// <param name="realEstateProjectId">The id.</param>
        /// <param name="realEstate">Identifies the real estate object.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        Task<RealEstateProjectEntries> AddAsync(long realEstateProjectId, RealEstate realEstate);

        /// <summary>
        /// Removes a real estate object from a real estate project.
        /// </summary>
        /// <param name="realEstateProjectId">The real estate project id.</param>
        /// <param name="realEstateId">The real estate id.</param>
        /// <param name="isExternal">true if the real estate id is an external id.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task RemoveAsync(long realEstateProjectId, string realEstateId, bool isExternal = false);

        /// <summary>
        /// Removes all real real estate objects from the real estate project identified by the specified id.
        /// </summary>
        /// <param name="realEstateProjectId">The id.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task RemoveAsync(long realEstateProjectId);
    }
}
