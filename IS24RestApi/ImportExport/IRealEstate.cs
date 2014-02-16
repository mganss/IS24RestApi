using IS24RestApi.Offer.RealEstates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// Describes a real estate that was retrieved from the IS24 REST API.
    /// </summary>
    public interface IRealEstate
    {
        /// <summary>
        /// Get the <see cref="RealEstate"/> instance with data delivered from IS24
        /// </summary>
        RealEstate RealEstate { get; }

        /// <summary>
        /// Gets the <see cref="IAttachmentResource"/> for the real estate retrieved
        /// </summary>
        IAttachmentResource Attachments { get; }
    }
}
