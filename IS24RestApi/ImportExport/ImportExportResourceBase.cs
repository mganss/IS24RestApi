using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi
{
    /// <summary>
    /// Base class for Import/Export API resources.
    /// </summary>
    public class ImportExportResourceBase: ResourceBase
    {
        /// <summary>
        /// Gets the default value for the current user
        /// </summary>
        public const string User = "me";

        /// <summary>
        /// The URL path segment identifying the resource, e.g. "offer/v1.0/user/{username}"
        /// </summary>
        public override string UrlPath
        {
            get
            {
                return string.Format("offer/v1.0/user/{0}/", Uri.EscapeDataString(User));
            }
        }
    }
}
