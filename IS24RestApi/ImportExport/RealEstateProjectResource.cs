using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS24RestApi.Offer.RealEstateProject;
using RestSharp;
using IS24RestApi.Common;
using IS24RestApi.Offer.RealEstates;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the resource responsible for managing real estate projects.
    /// <a href="http://api.immobilienscout24.de/our-apis/import-export/realestateproject.html">API Documentation</a>.
    /// </summary>
    public class RealEstateProjectResource : ImportExportResourceBase, IRealEstateProjectResource
    {
        /// <summary>
        /// Creates a new <see cref="RealEstateProjectResource"/> instance
        /// </summary>
        /// <param name="connection"></param>
        public RealEstateProjectResource(IIS24Connection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Gets the underlying <see cref="IIS24Connection"/> for executing the requests
        /// </summary>
        public IIS24Connection Connection { get; private set; }

        /// <summary>
        /// Gets all real estate projects.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<RealEstateProjects> GetAllAsync()
        {
            var req = Connection.CreateRequest("realestateproject");
            return ExecuteAsync<RealEstateProjects>(Connection, req);
        }

        /// <summary>
        /// Gets all real real estate objects belonging to the real estate project identified by the specified id.
        /// </summary>
        /// <param name="realEstateProjectId">The id.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task<RealEstateProjectEntries> GetAllAsync(long realEstateProjectId)
        {
            var req = Connection.CreateRequest("realestateproject/{id}/realestateprojectentry");
            req.AddParameter("id", realEstateProjectId, ParameterType.UrlSegment);
            return ExecuteAsync<RealEstateProjectEntries>(Connection, req);
        }

        /// <summary>
        /// Gets a real estate project identified by the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task<RealEstateProject> GetAsync(long id)
        {
            var req = Connection.CreateRequest("realestateproject/{id}");
            req.AddParameter("id", id, ParameterType.UrlSegment);
            return ExecuteAsync<RealEstateProject>(Connection, req);
        }

        /// <summary>
        /// Updates a real estate project.
        /// </summary>
        /// <param name="realEstateProject">The <see cref="IS24RestApi.Offer.RealEstateProject.RealEstateProject" /> object.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="IS24Exception"></exception>
        public async Task UpdateAsync(RealEstateProject realEstateProject)
        {
            var req = Connection.CreateRequest("realestateproject/{id}", Method.PUT);
            req.AddParameter("id", realEstateProject.Id.Value, ParameterType.UrlSegment);
            req.AddBody(realEstateProject);

            var resp = await ExecuteAsync<Messages>(Connection, req);
            if (!resp.IsSuccessful())
            {
                throw new IS24Exception(string.Format("Error updating real estate project {0}: {1}", realEstateProject.Id, resp.ToMessage())) { Messages = resp };
            }
        }

        /// <summary>
        /// Adds real estate objects to the real estate project identified by the specified id.
        /// </summary>
        /// <param name="realEstateProjectId">The id.</param>
        /// <param name="entries">Identifies the real estate objects.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task<RealEstateProjectEntries> AddAsync(long realEstateProjectId, RealEstateProjectEntries entries)
        {
            var req = Connection.CreateRequest("realestateproject/{id}/realestateprojectentry", Method.POST);
            req.AddParameter("id", realEstateProjectId, ParameterType.UrlSegment);
            req.AddBody(entries);
            return ExecuteAsync<RealEstateProjectEntries>(Connection, req);
        }

        /// <summary>
        /// Adds real estate objects to the real estate project identified by the specified id.
        /// </summary>
        /// <param name="realEstateProjectId">The id.</param>
        /// <param name="realEstates">Identifies the real estate objects.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task<RealEstateProjectEntries> AddAsync(long realEstateProjectId, IEnumerable<RealEstate> realEstates)
        { 
            var entries = new RealEstateProjectEntries();
            foreach (var entry in realEstates.Select(r => new RealEstateProjectEntry { RealEstateId = r.Id.Value }))
                entries.RealEstateProjectEntry.Add(entry);
            return AddAsync(realEstateProjectId, entries);
        }

        /// <summary>
        /// Adds a real estate object to the real estate project identified by the specified id.
        /// </summary>
        /// <param name="realEstateProjectId">The id.</param>
        /// <param name="realEstate">Identifies the real estate object.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task<RealEstateProjectEntries> AddAsync(long realEstateProjectId, RealEstate realEstate)
        {
            return AddAsync(realEstateProjectId, new[] { realEstate });
        }

        /// <summary>
        /// Removes a real estate object from a real estate project.
        /// </summary>
        /// <param name="realEstateProjectId">The real estate project id.</param>
        /// <param name="realEstateId">The real estate id.</param>
        /// <param name="isExternal">true if the real estate id is an external id.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="IS24Exception"></exception>
        public async Task RemoveAsync(long realEstateProjectId, string realEstateId, bool isExternal = false)
        {
            var req = Connection.CreateRequest("realestateproject/{realestateprojectid}/realestateprojectentry/{realestateid}", Method.DELETE);
            req.AddParameter("realestateprojectid", realEstateProjectId, ParameterType.UrlSegment);
            req.AddParameter("realestateid", isExternal ? "ext-" + realEstateId : realEstateId, ParameterType.UrlSegment);
            var resp = await ExecuteAsync<Messages>(Connection, req);
            if (!resp.IsSuccessful(MessageCode.MESSAGE_RESOURCE_DELETED))
            {
                throw new IS24Exception(string.Format("Error deleting real estate {0} from real estate project {1}: {2}", realEstateId, realEstateProjectId, resp.ToMessage())) { Messages = resp };
            }
        }

        /// <summary>
        /// Removes all real real estate objects from the real estate project identified by the specified id.
        /// </summary>
        /// <param name="realEstateProjectId">The id.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="IS24Exception"></exception>
        public async Task RemoveAsync(long realEstateProjectId)
        {
            var req = Connection.CreateRequest("realestateproject/{realestateprojectid}/realestateprojectentry", Method.DELETE);
            req.AddParameter("realestateprojectid", realEstateProjectId, ParameterType.UrlSegment);
            var resp = await ExecuteAsync<Messages>(Connection, req);
            if (!resp.IsSuccessful(MessageCode.MESSAGE_RESOURCE_DELETED))
            {
                throw new IS24Exception(string.Format("Error deleting real estate objects from real estate project {0}: {1}", realEstateProjectId, resp.ToMessage())) { Messages = resp };
            }
        }
    }
}
