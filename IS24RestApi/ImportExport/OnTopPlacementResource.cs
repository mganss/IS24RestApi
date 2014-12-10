﻿using IS24RestApi.Common;
using IS24RestApi.Offer.PremiumPlacement;
using IS24RestApi.Offer.RealEstates;
using IS24RestApi.Offer.ShowcasePlacement;
using IS24RestApi.Offer.TopPlacement;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// Abstract base class for placement resources.
    /// <a href="http://api.immobilienscout24.de/our-apis/import-export/ontop-placement.html">API Documentation</a>.
    /// </summary>
    /// <typeparam name="T">The type of placement</typeparam>
    public abstract class OnTopPlacementResource<T> : ImportExportResourceBase, IOnTopPlacementResource<T> where T : new()
    {
        /// <summary>
        /// Creates a new <see cref="OnTopPlacementResource{T}"/> instance
        /// </summary>
        /// <param name="connection"></param>
        public OnTopPlacementResource(IIS24Connection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Gets the <see cref="RealEstate"/> instance the placements belong to
        /// </summary>
        public RealEstate RealEstate { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OnTopPlacementResource{T}"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="realEstate">The real estate.</param>
        public OnTopPlacementResource(IIS24Connection connection, RealEstate realEstate)
        {
            Connection = connection;
            RealEstate = realEstate;
        }

        /// <summary>
        /// Gets the underlying <see cref="IIS24Connection"/> for executing the requests
        /// </summary>
        public IIS24Connection Connection { get; private set; }

        /// <summary>
        /// Gets the type of the placement. Can be one of "showcaseplacement", "premiumplacement", or "topplacement".
        /// </summary>
        /// <value>
        /// The type of the placement.
        /// </value>
        protected abstract string PlacementType { get; }

        /// <summary>
        /// Creates the specified placements.
        /// </summary>
        /// <param name="placements">The placements.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task<T> CreateAsync(T placements)
        {
            var req = Connection.CreateRequest(PlacementType + "/list", Method.POST);
            req.AddBody(placements);
            return ExecuteAsync<T>(Connection, req);
        }

        /// <summary>
        /// Creates a placement for the real estate identified by the specified id.
        /// </summary>
        /// <param name="id">The real estate id.</param>
        /// <param name="isExternal">true if the real estate id is an external id.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public async Task CreateAsync(string id, bool isExternal = false)
        {
            var req = Connection.CreateRequest("realestate/{id}/" + PlacementType, Method.POST);
            req.AddParameter("id", isExternal ? "ext-" + id : id, ParameterType.UrlSegment);
            var resp = await ExecuteAsync<Messages>(Connection, req);
            if (!resp.IsSuccessful(MessageCode.MESSAGE_RESOURCE_CREATED))
            {
                throw new IS24Exception(string.Format("Error creating placement {0} for real estate {1}: {2}", PlacementType, id, resp.Message.ToMessage())) { Messages = resp };
            }
        }

        /// <summary>
        /// Creates a placement for the real estate object associated with this resource.
        /// </summary>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task CreateAsync()
        {
            return CreateAsync(RealEstate.Id.ToString());
        }

        /// <summary>
        /// Gets all placements.
        /// </summary>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task<T> GetAllAsync()
        {
            var req = Connection.CreateRequest(PlacementType + "/all");
            return ExecuteAsync<T>(Connection, req);
        }

        /// <summary>
        /// Gets the placements for the real estate identified by the specified id.
        /// </summary>
        /// <param name="id">The real estate id.</param>
        /// <param name="isExternal">true if the real estate id is an external id.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task<T> GetAsync(string id, bool isExternal = false)
        {
            var req = Connection.CreateRequest("realestate/{id}/" + PlacementType);
            req.AddParameter("id", isExternal ? "ext-" + id : id, ParameterType.UrlSegment);
            return ExecuteAsync<T>(Connection, req);
        }

        /// <summary>
        /// Gets the placements for the real estate associated with this resource.
        /// </summary>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task<T> GetAsync()
        {
            return GetAsync(RealEstate.Id.ToString());
        }

        /// <summary>
        /// Removes all placements.
        /// </summary>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task<T> RemoveAllAsync()
        {
            var req = Connection.CreateRequest(PlacementType + "/all", Method.DELETE);
            return ExecuteAsync<T>(Connection, req);
        }

        /// <summary>
        /// Removes the placements for the real estate identified by the specified id.
        /// </summary>
        /// <param name="id">The real estate id.</param>
        /// <param name="isExternal">true if the real estate id is an external id.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public async Task RemoveAsync(string id, bool isExternal = false)
        {
            var req = Connection.CreateRequest("realestate/{id}/" + PlacementType, Method.DELETE);
            req.AddParameter("id", isExternal ? "ext-" + id : id, ParameterType.UrlSegment);
            var resp = await ExecuteAsync<Messages>(Connection, req);
            if (!resp.IsSuccessful(MessageCode.MESSAGE_RESOURCE_DELETED))
            {
                throw new IS24Exception(string.Format("Error deleting placement {0} for real estate {1}: {2}", PlacementType, id, resp.Message.ToMessage())) { Messages = resp };
            }
        }

        /// <summary>
        /// Removes the placements for the real estate associated with this resource.
        /// </summary>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task RemoveAsync()
        {
            return RemoveAsync(RealEstate.Id.ToString());
        }

        /// <summary>
        /// Removes the placements for the real estates identified by the specified ids.
        /// </summary>
        /// <param name="ids">The real estate ids.</param>
        /// <param name="isExternal">true if the real estate ids are external ids.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task<T> RemoveAsync(IEnumerable<string> ids, bool isExternal = false)
        {
            var req = Connection.CreateRequest(PlacementType + "/list", Method.DELETE);
            req.AddParameter("realestateids", string.Join(",", ids.Select(i => isExternal ? "ext-" + i : i)));
            return ExecuteAsync<T>(Connection, req);
        }
    }

    /// <summary>
    /// Describes showcaseplacement ("Schaufenster-Platzierung") resources.
    /// </summary>
    public class ShowcasePlacementResource : OnTopPlacementResource<Showcaseplacements>, IShowcasePlacementResource
    {
        /// <summary>
        /// Gets the type of the placement.
        /// </summary>
        /// <value>
        /// The type of the placement.
        /// </value>
        protected override string PlacementType
        {
            get { return "showcaseplacement"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowcasePlacementResource"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public ShowcasePlacementResource(IIS24Connection connection) : base(connection) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowcasePlacementResource" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="realEstate">The real estate.</param>
        public ShowcasePlacementResource(IIS24Connection connection, RealEstate realEstate) : base(connection, realEstate) { }
    }

    /// <summary>
    /// Describes premiumplacement ("Premium-Platzierung") resources.
    /// </summary>
    public class PremiumPlacementResource : OnTopPlacementResource<Premiumplacements>, IPremiumPlacementResource
    {
        /// <summary>
        /// Gets the type of the placement.
        /// </summary>
        /// <value>
        /// The type of the placement.
        /// </value>
        protected override string PlacementType
        {
            get { return "premiumplacement"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PremiumPlacementResource"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public PremiumPlacementResource(IIS24Connection connection) : base(connection) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PremiumPlacementResource" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="realEstate">The real estate.</param>
        public PremiumPlacementResource(IIS24Connection connection, RealEstate realEstate) : base(connection, realEstate) { }
    }

    /// <summary>
    /// Describes topplacement ("Top-Platzierung") resources.
    /// </summary>
    public class TopPlacementResource : OnTopPlacementResource<Topplacements>, ITopPlacementResource
    {
        /// <summary>
        /// Gets the type of the placement.
        /// </summary>
        /// <value>
        /// The type of the placement.
        /// </value>
        protected override string PlacementType
        {
            get { return "topplacement"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TopPlacementResource"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public TopPlacementResource(IIS24Connection connection) : base(connection) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TopPlacementResource" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="realEstate">The real estate.</param>
        public TopPlacementResource(IIS24Connection connection, RealEstate realEstate) : base(connection, realEstate) { }
    }
}
