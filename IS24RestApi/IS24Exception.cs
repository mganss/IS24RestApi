using IS24RestApi.Common;
using System;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace IS24RestApi
{
    /// <summary>
    /// Represents errors that occur during interaction with the IS24 REST API.
    /// </summary>
    public class IS24Exception : Exception
    {
        /// <summary>
        /// Gets or sets the detailed error messages. See http://api.immobilienscout24.de/get-started/responses.html for more.
        /// </summary>
        public Messages Messages { get; set; }
        /// <summary>
        /// Gets or sets the HTTP status code of the response.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// Initializes a new instance of the IS24Exception class.
        /// </summary>
        public IS24Exception() { }
        /// <summary>
        /// Initializes a new instance of the IS24Exception class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public IS24Exception(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the IS24Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public IS24Exception(string message, Exception inner) : base(message, inner) { }
    }
}
