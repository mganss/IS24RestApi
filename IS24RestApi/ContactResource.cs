using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using IS24RestApi.Offer;
using IS24RestApi.Common;

namespace IS24RestApi
{
    /// <summary>
    /// The Resource responsible for managing contacts for customer using the IS24 REST API
    /// </summary>
    public class ContactResource : IContactResource
    {
        private readonly IIS24Connection is24Connection;

        /// <summary>
        /// Creates a new <see cref="ContactResource"/> instance
        /// </summary>
        /// <param name="is24Connection"></param>
        public ContactResource(IIS24Connection is24Connection)
        {
            this.is24Connection = is24Connection;
        }

        /// <summary>
        /// Gets all contacts.
        /// </summary>
        /// <returns>The contacts.</returns>
        public async Task<IEnumerable<RealtorContactDetails>> GetAsync()
        {
            var contacts = await is24Connection.ExecuteAsync<RealtorContactDetailsList>(is24Connection.CreateRequest("contact"));
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
            var req = is24Connection.CreateRequest("contact/{id}");
            req.AddParameter("id", isExternal ? "ext-" + id : id, ParameterType.UrlSegment);
            return is24Connection.ExecuteAsync<RealtorContactDetails>(req);
        }

        /// <summary>
        /// Creates a contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public async Task CreateAsync(RealtorContactDetails contact)
        {
            var request = is24Connection.CreateRequest("contact", Method.POST);
            request.AddBody(contact);

            var resp = await is24Connection.ExecuteAsync<Messages>(request);
            var id = resp.ExtractCreatedResourceId();
            if (!id.HasValue)
            {
                throw new IS24Exception(string.Format("Error creating contact {0}: {1}", contact.Lastname, resp.Message.ToMessage())) { Messages = resp };
            }
            contact.Id = id.Value;
            contact.IdSpecified = true;
        }

        /// <summary>
        /// Updates a contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public async Task UpdateAsync(RealtorContactDetails contact)
        {
            var req = is24Connection.CreateRequest("contact/{id}", Method.PUT);
            req.AddParameter("id", contact.Id, ParameterType.UrlSegment);
            req.AddBody(contact);

            var resp = await is24Connection.ExecuteAsync<Messages>(req);
            if (!resp.IsSuccessful())
            {
                throw new IS24Exception(string.Format("Error updating contact {0}: {1}", contact.Lastname, resp.Message.ToMessage())) { Messages = resp };
            }
        }
    }
}