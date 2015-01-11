using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using IS24RestApi.Offer;
using IS24RestApi.Common;

namespace IS24RestApi
{
    /// <summary>
    /// The contacts resource.
    /// </summary>
    public class RealEstateCounts : ImportExportResourceBase, IRealEstateCounts
    {
        /// <summary>
        /// Gets the underlying <see cref="IIS24Connection"/> for executing the requests
        /// </summary>
        public IIS24Connection Connection { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ContactResource"/> instance
        /// </summary>
        /// <param name="connection"></param>
        public RealEstateCounts(IIS24Connection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Gets the realestatecounts.
        /// </summary>
        /// <returns>The realestatecounts.</returns>
        public async Task<Realestate.Counts.RealEstateCounts> GetAsync()
        {
            var realestatecounts = await ExecuteAsync<Realestate.Counts.RealEstateCounts>(Connection, Connection.CreateRequest("realestatecounts"));
            return realestatecounts;
        }
    }
}