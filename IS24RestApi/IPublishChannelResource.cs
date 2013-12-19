using System.Collections.Generic;
using System.Threading.Tasks;
using IS24RestApi.Offer;
using IS24RestApi.Common;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the resources responsible for getting the publish channels the current user
    /// has access to
    /// </summary>
    public interface IPublishChannelResource
    {
        /// <summary>
        /// Gets all <see cref="PublishChannel"/>s the user has access to
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PublishChannel>> GetAsync();
    }
}
