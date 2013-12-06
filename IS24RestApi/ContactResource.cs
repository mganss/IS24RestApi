using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace IS24RestApi
{
    /// <summary>
    /// The Resource responsible for managing contacts for customer using the IS24 REST API
    /// </summary>
    public class ContactResource : IContactResource
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
            var contacts = await Connection.ExecuteAsync<realtorContactDetailsList>(Connection.CreateRequest("contact"));
            return contacts.realtorContactDetails;
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
            return Connection.ExecuteAsync<RealtorContactDetails>(req);
        }

        /// <summary>
        /// Creates a contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public async Task CreateAsync(RealtorContactDetails contact)
        {
            var request = Connection.CreateRequest("contact", Method.POST);
            request.AddBody(contact);

            var resp = await Connection.ExecuteAsync<messages>(request);
            var id = resp.ExtractCreatedResourceId();
            if (!id.HasValue)
            {
                throw new IS24Exception(string.Format("Error creating contact {0}: {1}", contact.lastname, resp.message.ToMessage())) { Messages = resp };
            }
            contact.id = id.Value;
            contact.idSpecified = true;
        }

        /// <summary>
        /// Updates a contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public async Task UpdateAsync(RealtorContactDetails contact)
        {
            var req = Connection.CreateRequest("contact/{id}", Method.PUT);
            req.AddParameter("id", contact.id, ParameterType.UrlSegment);
            req.AddBody(contact);

            var resp = await Connection.ExecuteAsync<messages>(req);
            if (!resp.IsSuccessful())
            {
                throw new IS24Exception(string.Format("Error updating contact {0}: {1}", contact.lastname, resp.message.ToMessage())) { Messages = resp };
            }
        }
    }
}