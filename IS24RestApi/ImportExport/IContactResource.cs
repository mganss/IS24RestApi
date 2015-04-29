using System.Collections.Generic;
using System.Threading.Tasks;
using IS24RestApi.Offer;
using IS24RestApi.Common;

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

        /// <summary>
        /// Deletes a contact.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="isExternal">true if the id is an external id.</param>
        /// <exception cref="IS24Exception"></exception>
        Task DeleteAsync(string id, bool isExternal = false);

        /// <summary>
        /// Deletes a contact, assigning its real estates to a specified contact.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="assignToContactId">The contact id to assign real estates of contact to delete to.</param>
        /// <param name="isExternal">true if the id is an external id.</param>
        /// <param name="isAssignedToContactExternal">true if <paramref name="assignToContactId"/> is an external id.</param>
        /// <exception cref="IS24Exception"></exception>
        Task DeleteAsync(string id, string assignToContactId, bool isExternal = false, bool isAssignedToContactExternal = false);
    }
}