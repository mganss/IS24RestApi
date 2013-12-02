using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace IS24RestApi
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
    /// ExtensionMethods extending the type <see cref="messages"/>
    /// </summary>
    public static class MessagesExtensions
    {
        /// <summary>
        /// Extracts the included resource id
        /// </summary>
        /// <param name="resp"></param>
        /// <returns></returns>
        public static long? ExtractCreatedResourceId(this messages resp)
        {
            if (resp.message != null 
                && resp.message.Any() 
                && resp.message[0].messageCode == MessageCode.MESSAGE_RESOURCE_CREATED)
            {
                var m = Regex.Match(resp.message[0].message, @"with id \[(\d+)\] has been created");
                if (m.Success) return long.Parse(m.Groups[1].Value);
            }

            return null;
        }

        /// <summary>
        /// Checks if the <see cref="messages"/> contains a successful content depending on the internal <see cref="MessageCode"/>
        /// </summary>
        /// <param name="resp"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsSuccessful(this messages resp, MessageCode code = MessageCode.MESSAGE_RESOURCE_UPDATED)
        {
            return (resp.message != null
                && resp.message.Any()
                && resp.message[0].messageCode == code);
        }

        /// <summary>
        /// Collects multiple <see cref="Message"/>s into a single string.
        /// </summary>
        /// <param name="msgs">The <see cref="Message"/>s</param>
        /// <returns>A single string containing all messages</returns>
        public static string ToMessage(this Message[] msgs)
        {
            return string.Join(Environment.NewLine, msgs.Select(m => m.ToString()).ToArray());
        }
    }
}

