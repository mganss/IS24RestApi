using System.Collections.Generic;
using System.Threading.Tasks;
using IS24RestApi.Rest;
using IS24RestApi.Offer;
using IS24RestApi.Common;
using IS24RestApi.Realestate.Counts;

namespace IS24RestApi
{
    /// <summary>
    /// The real estate counts resource.
    /// <a href="http://api.immobilienscout24.de/our-apis/import-export/realestate/realestates-counts.html">API Documentation</a>.
    /// </summary>
    public class RealEstateCountsResource : ImportExportResourceBase, IRealEstateCounts
    {
        /// <summary>
        /// Gets the underlying <see cref="IIS24Connection"/> for executing the requests
        /// </summary>
        public IIS24Connection Connection { get; private set; }

        /// <summary>
        /// Creates a new <see cref="RealEstateCountsResource"/> instance
        /// </summary>
        /// <param name="connection"></param>
        public RealEstateCountsResource(IIS24Connection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Gets the real estate counts.
        /// </summary>
        /// <returns>The real estate counts.</returns>
        public async Task<RealEstateCounts> GetAsync()
        {
            var realestatecounts = await ExecuteAsync<RealEstateCounts>(Connection, Connection.CreateRequest("realestatecounts"));
            return realestatecounts;
        }
    }
}