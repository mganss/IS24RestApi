using System.Collections.Generic;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// The resources responsible for getting the publish channels the current user
    /// has access to
    /// </summary>
    public class PublishChannelResource : IPublishChannelResource
    {
        /// <summary>
        /// Gets the underlying <see cref="IIS24Connection"/> for executing the requests
        /// </summary>
        public IIS24Connection Connection { get; private set; }

        /// <summary>
        /// Creates a new <see cref="PublishChannelResource"/> instance
        /// </summary>
        /// <param name="connection"></param>
        public PublishChannelResource(IIS24Connection connection)
        {
            this.Connection = connection;
        }

        /// <summary>
        /// Gets all <see cref="PublishChannel"/>s the user has access to
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PublishChannel>> GetAsync()
        {
            var request = Connection.CreateRequest("publishchannel");
            var publishObjectsResult = await Connection.ExecuteAsync<publishChannels>(request);
            return publishObjectsResult.publishChannel;
        }
    }
}