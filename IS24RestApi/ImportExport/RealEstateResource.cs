using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using RestSharp;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.Common;
using IS24RestApi.Offer.ListElement;

namespace IS24RestApi
{
    /// <summary>
    /// The resource responsible for managing real estate data
    /// </summary>
    public class RealEstateResource : ImportExportResourceBase, IRealEstateResource
    {
        /// <summary>
        /// Creates a new <see cref="RealEstateResource"/> instance
        /// </summary>
        /// <param name="connection"></param>
        public RealEstateResource(IIS24Connection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Gets the underlying <see cref="IIS24Connection"/> for executing the requests
        /// </summary>
        public IIS24Connection Connection { get; private set; }

        /// <summary>
        /// Get all real estates as an observable sequence.
        /// </summary>
        /// <returns>The real estates.</returns>
        public IObservable<IRealEstate> GetAsync()
        {
            return GetSummariesAsync().SelectMany(async r => await GetAsync(r.Id.ToString()));
        }

        /// <summary>
        /// Get summaries for all real estates as an observable sequence.
        /// </summary>
        /// <returns>The summaries of all real estates.</returns>
        public IObservable<OfferRealEstateForList> GetSummariesAsync()
        {
            return Observable.Create<OfferRealEstateForList>(
                async o =>
                {
                    var page = 1;

                    while (true)
                    {
                        var req = Connection.CreateRequest("realestate");
                        req.AddParameter("pagesize", 100);
                        req.AddParameter("pagenumber", page);
                        var rel = await ExecuteAsync<RealEstates>(Connection, req);

                        foreach (var ore in rel.RealEstateList)
                            o.OnNext(ore);

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
        public async Task<IRealEstate> GetAsync(string id, bool isExternal = false)
        {
            var req = Connection.CreateRequest("realestate/{id}");
            req.AddParameter("usenewenergysourceenev2014values", "true", ParameterType.QueryString);
            req.AddParameter("id", isExternal ? "ext-" + id : id, ParameterType.UrlSegment);
            var realEstate = await ExecuteAsync<RealEstate>(Connection, req);
            return new RealEstateItem(realEstate, Connection);
        }

        /// <summary>
        /// Creates a RealEstate object.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        public async Task CreateAsync(RealEstate re)
        {
            var req = Connection.CreateRequest("realestate", Method.POST);
            req.AddParameter("usenewenergysourceenev2014values", "true", ParameterType.QueryString);
            req.AddBody(re);
            var resp = await ExecuteAsync<Messages>(Connection, req);
            var id = resp.ExtractCreatedResourceId();
            if (!id.HasValue)
            {
                throw new IS24Exception(string.Format("Error creating RealEstate {0}: {1}", re.ExternalId, resp.ToMessage())) { Messages = resp };
            }

            re.Id = id.Value;
        }

        /// <summary>
        /// Updates a RealEstate object.
        /// </summary>
        /// <param name="re">The RealEstate object.</param>
        public async Task UpdateAsync(RealEstate re)
        {
            var req = Connection.CreateRequest("realestate/{id}", Method.PUT);
            req.AddParameter("usenewenergysourceenev2014values", "true", ParameterType.QueryString);
            req.AddParameter("id", re.Id, ParameterType.UrlSegment);
            req.AddBody(re);
            var messages = await ExecuteAsync<Messages>(Connection, req);
            if (!messages.IsSuccessful())
            {
                throw new IS24Exception(string.Format("Error updating RealEstate {0}: {1}", re.ExternalId, messages.ToMessage())) { Messages = messages };
            }
        }

        /// <summary>
        /// Deletes a RealEstate object. This seems to be possible if the real estate is not published.
        /// </summary>
        /// <param name="id">The id of the RealEstate object to be deleted.</param>
        public async Task DeleteAsync(string id)
        {
            var request = Connection.CreateRequest("realestate/{id}", Method.DELETE);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            await ExecuteAsync<Messages>(Connection, request);
        }
    }
}