using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace IS24RestApi
{
    public class ContactResource
    {
        private IS24Client is24Client;

        public ContactResource(IS24Client is24Client)
        {
            this.is24Client = is24Client;
        }

        /// <summary>
        /// Gets all contacts.
        /// </summary>
        /// <returns>The contacts.</returns>
        public async Task<IEnumerable<RealtorContactDetails>> GetAsync()
        {
            var contacts = await is24Client.Is24RestClient.ExecuteAsync<realtorContactDetailsList>(is24Client.Is24RestClient.Request("contact"));
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
            var req = is24Client.Is24RestClient.Request("contact/{id}");
            req.AddParameter("id", isExternal ? "ext-" + id : id, ParameterType.UrlSegment);
            return is24Client.Is24RestClient.ExecuteAsync<RealtorContactDetails>(req);
        }

        /// <summary>
        /// Creates a contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public async Task CreateAsync(RealtorContactDetails contact)
        {
            var req = is24Client.Is24RestClient.Request("contact", Method.POST);
            req.AddBody(contact);

            var resp = await is24Client.Is24RestClient.ExecuteAsync<messages>(req);
            var id = is24Client.ExtractNewId(resp);
            if (!id.HasValue) throw new IS24Exception(string.Format("Error creating contact {0}: {1}", contact.lastname, resp.message.Msg())) { Messages = resp };
            contact.id = id.Value;
            contact.idSpecified = true;
        }

        /// <summary>
        /// Updates a contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public async Task UpdateAsync(RealtorContactDetails contact)
        {
            var req = is24Client.Is24RestClient.Request("contact/{id}", Method.PUT);
            req.AddParameter("id", contact.id, ParameterType.UrlSegment);
            req.AddBody(contact);

            var resp = await is24Client.Is24RestClient.ExecuteAsync<messages>(req);
            if (!is24Client.Ok(resp)) throw new IS24Exception(string.Format("Error updating contact {0}: {1}", contact.lastname, resp.message.Msg())) { Messages = resp };
        }
    }
}