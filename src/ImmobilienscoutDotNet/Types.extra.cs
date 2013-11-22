using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmobilienscoutDotNet
{
    public partial class Message
    {
        /// <summary>
        /// The details of the message as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}: {1}", messageCode, message);
        }
    }

    /// <summary>
    /// Extension Methods for REST API classes.
    /// </summary>
    public static class RestExtensions
    {
        /// <summary>
        /// Collects multiple <see cref="Message"/>s into a single string.
        /// </summary>
        /// <param name="msgs">The <see cref="Message"/>s</param>
        /// <returns>A single string containing all messages</returns>
        public static string Msg(this Message[] msgs)
        {
            return string.Join(Environment.NewLine, msgs.Select(m => m.ToString()).ToArray());
        }
    }
}

