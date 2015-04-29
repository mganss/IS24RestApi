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
    public class ContactResource : ImportExportResourceBase, IContactResource
    {
        /// <summary>
        /// Gets the underlying <see cref="IIS24Connection"/> for executing the requests
        /// </summary>
        public IIS24Connection Connection { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ContactResource"/> instance
        /// </summary>
        /// <param name="connection"></param>
        public ContactResource(IIS24Connection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Gets all contacts.
        /// </summary>
        /// <returns>The contacts.</returns>
        public async Task<IEnumerable<RealtorContactDetails>> GetAsync()
        {
            var contacts = await ExecuteAsync<RealtorContactDetailsList>(Connection, Connection.CreateRequest("contact"));
            return contacts.RealtorContactDetails;
        }

        /// <summary>
        /// Gets a single contact identified by the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="isExternal">true if the id is an external id.</param>
        /// <returns>The contact or null.</returns>
        public Task<RealtorContactDetails> GetAsync(string id, bool isExternal = false)
        {
            var req = Connection.CreateRequest("contact/{id}");
            req.AddParameter("id", isExternal ? "ext-" + id : id, ParameterType.UrlSegment);
            return ExecuteAsync<RealtorContactDetails>(Connection, req);
        }

        /// <summary>
        /// Creates a contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public async Task CreateAsync(RealtorContactDetails contact)
        {
            var request = Connection.CreateRequest("contact", Method.POST);
            request.AddBody(contact);

            var resp = await ExecuteAsync<Messages>(Connection, request);
            var id = resp.ExtractCreatedResourceId();
            if (!id.HasValue)
            {
                throw new IS24Exception(string.Format("Error creating contact {0}: {1}", contact.Lastname, resp.Message.ToMessage())) { Messages = resp };
            }
            contact.Id = id.Value;
        }

        /// <summary>
        /// Updates a contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public async Task UpdateAsync(RealtorContactDetails contact)
        {
            var req = Connection.CreateRequest("contact/{id}", Method.PUT);
            req.AddParameter("id", contact.Id.HasValue ? contact.Id.Value.ToString() : "ext-" + contact.ExternalId, ParameterType.UrlSegment);
            req.AddBody(contact);

            var resp = await ExecuteAsync<Messages>(Connection, req);
            if (!resp.IsSuccessful())
            {
                throw new IS24Exception(string.Format("Error updating contact {0}: {1}", contact.Lastname, resp.Message.ToMessage())) { Messages = resp };
            }
        }

        /// <summary>
        /// Deletes a contact.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="isExternal">true if the id is an external id.</param>
        /// <exception cref="IS24Exception"></exception>
        public async Task DeleteAsync(string id, bool isExternal = false)
        {
            var req = Connection.CreateRequest("contact/{id}", Method.DELETE);
            req.AddParameter("id", isExternal ? "ext-" + id : id, ParameterType.UrlSegment);
            var resp = await ExecuteAsync<Messages>(Connection, req);
            if (!resp.IsSuccessful(MessageCode.MESSAGE_RESOURCE_DELETED))
            {
                throw new IS24Exception(string.Format("Error deleting contact {0}: {1}", id, resp.Message.ToMessage())) { Messages = resp };
            }
        }

        /// <summary>
        /// Deletes a contact, assigning its real estates to a specified contact.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="assignToContactId">The contact id to assign real estates of contact to delete to.</param>
        /// <param name="isExternal">true if the id is an external id.</param>
        /// <param name="isAssignedToContactExternal">true if <paramref name="assignToContactId"/> is an external id.</param>
        /// <exception cref="IS24Exception"></exception>
        public async Task DeleteAsync(string id, string assignToContactId, bool isExternal = false, bool isAssignedToContactExternal = false)
        {
            var req = Connection.CreateRequest("contact/{id}", Method.DELETE);
            req.AddParameter("id", isExternal ? "ext-" + id : id, ParameterType.UrlSegment);
            req.AddParameter("assigntocontactid", isAssignedToContactExternal ? "ext-" + assignToContactId : assignToContactId, ParameterType.QueryString);
            var resp = await ExecuteAsync<Messages>(Connection, req);
            if (!resp.IsSuccessful(MessageCode.MESSAGE_RESOURCE_DELETED))
            {
                throw new IS24Exception(string.Format("Error deleting contact {0}: {1}", id, resp.Message.ToMessage())) { Messages = resp };
            }
        }
    }
}