using System.Collections.Generic;
using System.Threading.Tasks;
using IS24RestApi.Offer;
using IS24RestApi.Common;
using IS24RestApi.Realestate.Counts;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the RealEstateCounts resource.
    /// </summary>
    public interface IRealEstateCounts : IResource
    {
        /// <summary>
        /// Gets the real estate counts.
        /// </summary>
        /// <returns>The real estate counts.</returns>
        Task<RealEstateCounts> GetAsync();
    }
}