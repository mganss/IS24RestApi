using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using RestSharp;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.Common;

namespace IS24RestApi
{
    /// <summary>
    /// The resource responsible for managing real estate data
    /// </summary>
    public class RealEstateResource : ImportExportResourceBase, IRealEstateResource
    {
        private readonly IIS24Connection connection;

        /// <summary>
        /// Creates a new <see cref="RealEstateResource"/> instance
        /// </summary>
        /// <param name="connection"></param>
        public RealEstateResource(IIS24Connection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Get all RealEstate objects as an observable sequence.
        /// </summary>
        /// <returns>The RealEstate objects.</returns>
        public IObservable<RealEstate> GetAsync()
        {
            return Observable.Create<RealEstate>(
                async o =>
                {
                    var page = 1;

                    while (true)
                    {
                        var req = connection.CreateRequest("realestate");
                        req.AddParameter("pagesize", 100);
                        req.AddParameter("pagenumber", page);
                        var rel = await ExecuteAsync<RealEstates>(connection, req);

                        foreach (var ore in rel.RealEstateList)
                        {
                            var oreq = connection.CreateRequest("realestate/{id}");
                            oreq.AddParameter("id", ore.Id, ParameterType.UrlSegment);
                            var re = await ExecuteAsync<RealEstate>(connection, oreq);
                            o.OnNext(re);
                        }

                        if (page >= rel.Paging.NumberOfPages) break;
                        page++;
                    }
                });
        }

        /// <summary>
        /// Gets a single RealEstate object identified by the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="isExternal">true if the id is an external id.</param>
        /// <returns>The RealEstate object or null.</returns>
        public Task<RealEstate> GetAsync(string id, bool isExternal = false)
        {
            var req = connection.CreateRequest("realestate/{id}");
            req.AddParameter("id", isExternal ? "ext-" + id : id, ParameterType.UrlSegment);
            return ExecuteAsync<RealEstate>(connection, req);
        }

        /// <summary>
        /// Creates a RealEstate object.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        public async Task CreateAsync(RealEstate re)
        {
            var req = connection.CreateRequest("realestate", Method.POST);
            req.AddBody(re);
            var resp = await ExecuteAsync<Messages>(connection, req);
            var id = resp.ExtractCreatedResourceId();
            if (!id.HasValue)
            {
                throw new IS24Exception(string.Format("Error creating RealEstate {0}: {1}", re.ExternalId, resp.Message.ToMessage())) { Messages = resp };
            }
            re.Id = id.Value;
        }

        /// <summary>
        /// Updates a RealEstate object.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        public async Task UpdateAsync(RealEstate re)
        {
            var req = connection.CreateRequest("realestate/{id}", Method.PUT);
            req.AddParameter("id", re.Id, ParameterType.UrlSegment);
            req.AddBody(re);
            var Messages = await ExecuteAsync<Messages>(connection, req);
            if (!Messages.IsSuccessful())
            {
                throw new IS24Exception(string.Format("Error updating RealEstate {0}: {1}", re.ExternalId, Messages.Message.ToMessage())) { Messages = Messages };
            }
        }

        /// <summary>
        /// Deletes a RealEstate object. This seems to be possible if the real estate is not published.
        /// </summary>
        /// <param name="id">The id of the RealEstate object to be deleted.</param>
        public async Task DeleteAsync(string id)
        {
            var request = connection.CreateRequest("realestate/{id}", Method.DELETE);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            await ExecuteAsync<Messages>(connection, request);
        }
    }
}