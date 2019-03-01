using IS24RestApi.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// Base class for REST API resources.
    /// </summary>
    public abstract class ResourceBase
    {
        /// <summary>
        /// The URL path segment identifying the resource, e.g. "offer/v1.0/user/{username}"
        /// </summary>
        public abstract string UrlPath { get; }

        /// <summary>
        /// Performs an API request as an asynchronous task.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="request">The request object.</param>
        /// <returns>
        /// The task representing the request.
        /// </returns>
        protected Task<T> ExecuteAsync<T>(IIS24Connection connection, IRestRequest request) where T : new()
        {
            return connection.ExecuteAsync<T>(request, UrlPath);
        }
    }
}
