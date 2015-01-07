using System.Collections.Generic;
using System.Threading.Tasks;
using IS24RestApi.Offer;
using IS24RestApi.Common;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the resource responsible for managing contacts
    /// </summary>
    public interface IRealEstateCounts : IResource
    {
        /// <summary>
        /// Gets the realestatecounts.
        /// </summary>
        /// <returns>The realestatecounts.</returns>
        Task<Realestate.Counts.RealEstateCounts> GetAsync();
    }
}