using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS24RestApi.Rest;
using System.ComponentModel;
using IS24RestApi.Search;
using System.Globalization;

namespace IS24RestApi.Search
{
    /// <summary>
    /// The search resource.
    /// </summary>
    public class SearchResource: ResourceBase
    {
        private readonly IIS24Connection connection;

        /// <summary>
        /// Creates a new <see cref="SearchResource"/> instance
        /// </summary>
        /// <param name="connection">The connection that will be used to make requests to the service</param>
        public SearchResource(IIS24Connection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// The URL path segment identifying the resource, e.g. "offer/v1.0/user/{username}"
        /// </summary>
        public override string UrlPath
        {
            get { return "search/v1.0"; }
        }

        /// <summary>
        /// Performs a search query.
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="page">The page number of the result list</param>
        /// <param name="pageSize">The result page size</param>
        /// <returns>The result of the search</returns>
        public Task<ResultList.Resultlist> Search(Query query, int page = 1, int pageSize = 20)
        {
            var resource = "search/" + (query.GetResource());
            var req = connection.CreateRequest(resource);

            var parms = query.GetParameters();

            foreach (var p in parms)
            {
                req.AddParameter(p.Key, p.Value);
            }

            if (page > 1) req.AddParameter("pagenumber", page.ToString());
            if (pageSize != 20) req.AddParameter("pagesize", pageSize.ToString());

            return ExecuteAsync<ResultList.Resultlist>(connection, req);
        }
    }
}
