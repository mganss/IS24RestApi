using IS24RestApi.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// The resources responsible for getting the publish channels the current user
    /// has access to
    /// </summary>
    public class PublishChannelResource : ImportExportResourceBase, IPublishChannelResource
    {
        private readonly IIS24Connection is24Connection;

        /// <summary>
        /// Creates a new <see cref="PublishChannelResource"/> instance
        /// </summary>
        /// <param name="is24Connection"></param>
        public PublishChannelResource(IIS24Connection is24Connection)
        {
            this.is24Connection = is24Connection;
        }

        /// <summary>
        /// Gets all <see cref="PublishChannel"/>s the user has access to
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PublishChannel>> GetAsync()
        {
            var request = is24Connection.CreateRequest("publishchannel");
            var publishObjectsResult = await ExecuteAsync<PublishChannels>(is24Connection, request);
            return publishObjectsResult.PublishChannel;
        }
    }
}