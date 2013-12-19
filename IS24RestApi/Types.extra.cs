using IS24RestApi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IS24RestApi.Common
{
    public partial class Message
    {
        /// <summary>
        /// The details of the message as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}: {1}", MessageCode, MessageProperty);
        }
    }

    /// <summary>
    /// ExtensionMethods extending the type <see cref="Messages"/>
    /// </summary>
    public static class MessagesExtensions
    {
        /// <summary>
        /// Extracts the included resource id
        /// </summary>
        /// <param name="resp"></param>
        /// <returns></returns>
        public static long? ExtractCreatedResourceId(this Messages resp)
        {
            if (resp.Message != null 
                && resp.Message.Any() 
                && resp.Message[0].MessageCode == MessageCode.MESSAGE_RESOURCE_CREATED)
            {
                var m = Regex.Match(resp.Message[0].MessageProperty, @"with id \[(\d+)\] has been created");
                if (m.Success) return long.Parse(m.Groups[1].Value);
            }

            return null;
        }

        /// <summary>
        /// Checks if the <see cref="Messages"/> contains a successful content depending on the internal <see cref="MessageCode"/>
        /// </summary>
        /// <param name="resp"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsSuccessful(this Messages resp, MessageCode code = MessageCode.MESSAGE_RESOURCE_UPDATED)
        {
            return (resp.Message != null
                && resp.Message.Any()
                && resp.Message[0].MessageCode == code);
        }

        /// <summary>
        /// Collects multiple <see cref="Message"/>s into a single string.
        /// </summary>
        /// <param name="msgs">The <see cref="Message"/>s</param>
        /// <returns>A single string containing all messages</returns>
        public static string ToMessage(this IEnumerable<Message> msgs)
        {
            return msgs == null ? "" : string.Join(Environment.NewLine, msgs.Select(m => m.ToString()).ToArray());
        }
    }
}

