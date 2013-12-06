using System.Collections.Generic;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the resource responsible for managing contacts
    /// </summary>
    public interface IContactResource : IResource
    {
        /// <summary>
        /// Gets all contacts.
        /// </summary>
        /// <returns>The contacts.</returns>
        Task<IEnumerable<RealtorContactDetails>> GetAsync();

        /// <summary>
        /// Gets a single contact identified by the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="isExternal">true if the id is an external id.</param>
        /// <returns>The contact or null.</returns>
        Task<RealtorContactDetails> GetAsync(string id, bool isExternal = false);

        /// <summary>
        /// Creates a contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        Task CreateAsync(RealtorContactDetails contact);

        /// <summary>
        /// Updates a contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        Task UpdateAsync(RealtorContactDetails contact);
    }
}