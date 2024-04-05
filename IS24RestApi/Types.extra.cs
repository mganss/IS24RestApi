using IS24RestApi.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RestSharp;

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
            if (resp != null && resp.Message != null
                && resp.Message.Any()
                && resp.Message[0].MessageCode == MessageCode.MESSAGE_RESOURCE_CREATED)
            {
                return long.Parse(resp.Message[0].Id);
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
            return (resp != null && resp.Message != null
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

        /// <summary>
        /// Gets a single message string for a <see cref="Messages"/> object.
        /// </summary>
        /// <param name="resp">The <see cref="Messages"/> object</param>
        /// <returns>A single string containing all messages</returns>
        public static string ToMessage(this Messages resp)
        {
            return resp == null ? "No messages" : resp.Message.ToMessage();
        }
    }

    public partial class Attachment
    {
        /// <summary>
        /// Calculates MD5 hash from specified file and sets <see cref="ExternalCheckSum"/>.
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public async Task CalculateCheckSumAsync(string path)
        {
            if (path != null && path.Contains("http"))
            {
                var stream = await new RestClient().DownloadStreamAsync(new RestRequest(new Uri(path)));
                await CalculateCheckSumAsync(stream);
            }
            else
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
                    await CalculateCheckSumAsync(fs);   
            }
        }

        /// <summary>
        /// Calculates MD5 hash from specified stream and sets <see cref="ExternalCheckSum"/>.
        /// </summary>
        /// <param name="stream">Stream to calculate hash for</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public async Task CalculateCheckSumAsync(Stream stream)
        {
            byte[] bytes = null;
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                bytes = ms.ToArray();
            }
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(bytes);
                ExternalCheckSum = string.Concat(Array.ConvertAll(hash, b => b.ToString("x2")));
            }
        }
    }
}

namespace IS24RestApi
{
    /// <summary>
    /// Provides extra message related properties.
    /// </summary>
    public class MessageExtra
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [System.Xml.Serialization.XmlElementAttribute("message", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "string")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the message code value.
        /// </summary>
        /// <value>
        /// The message code value.
        /// </value>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("messageCode", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MessageCode MessageCodeValue { get; set; }

        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the MessageCode property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool MessageCodeValueSpecified { get; set; }

        /// <summary>
        /// Gets or sets the message code.
        /// </summary>
        /// <value>
        /// The message code.
        /// </value>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<MessageCode> MessageCode
        {
            get
            {
                if (this.MessageCodeValueSpecified)
                {
                    return this.MessageCodeValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.MessageCodeValue = value.GetValueOrDefault();
                this.MessageCodeValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// Gets the message object.
        /// </summary>
        /// <value>
        /// The message object.
        /// </value>
        [System.Xml.Serialization.XmlIgnore]
        public Message MessageObject
        {
            get
            {
                return MessageCode.HasValue ? new Message { MessageCode = MessageCode.Value, MessageProperty = Message } : null;
            }
        }
    }
}

namespace IS24RestApi.Offer.RealEstateProject
{
    public partial class RealEstateProjectEntry: MessageExtra
    {
    }
}

namespace IS24RestApi.Offer
{
    /// <summary>
    /// Common interface for OnTopPlacement classes.
    /// </summary>
    public interface ITopPlacement
    {
        /// <summary>
        /// Gets the message object.
        /// </summary>
        /// <value>
        /// The message object.
        /// </value>
        Message MessageObject { get; }
    }

    /// <summary>
    /// Common interface for collection of OnTopPlacement classes.
    /// </summary>
    public interface ITopPlacements<out T> where T : ITopPlacement
    {
        /// <summary>
        /// Gets the placements.
        /// </summary>
        /// <value>
        /// The placements.
        /// </value>
        IEnumerable<T> Placements { get; }
    }
}

namespace IS24RestApi.Offer.TopPlacement
{
    public partial class Topplacements: ITopPlacements<Topplacement>
    {
        /// <summary>
        /// Gets the placements.
        /// </summary>
        /// <value>
        /// The placements.
        /// </value>
        public IEnumerable<Topplacement> Placements
        {
            get { return Topplacement; }
        }
    }

    public partial class Topplacement: MessageExtra, ITopPlacement
    {
        /// <summary>
        /// <para xml:lang="de-DE">kunden referenznummer der immobilie (nur Ausgabe)</para>
        /// <para xml:lang="en">external id of real estate OUTPUT ONLY</para>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("externalId", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "string")]
        public string ExternalId { get; set; }
    }
}

namespace IS24RestApi.Offer.ShowcasePlacement
{
    public partial class Showcaseplacements : ITopPlacements<Showcaseplacement>
    {
        /// <summary>
        /// Gets the placements.
        /// </summary>
        /// <value>
        /// The placements.
        /// </value>
        public IEnumerable<Showcaseplacement> Placements
        {
            get { return Showcaseplacement; }
        }
    }

    public partial class Showcaseplacement : ITopPlacement
    {
        /// <summary>
        /// Gets the message object.
        /// </summary>
        /// <value>
        /// The message object.
        /// </value>
        [System.Xml.Serialization.XmlIgnore]
        public Message MessageObject
        {
            get
            {
                return new Message { MessageCode = (MessageCode)Enum.Parse(typeof(MessageCode), MessageCode), MessageProperty = Message };
            }
        }
    }
}

namespace IS24RestApi.Offer.PremiumPlacement
{
    public partial class Premiumplacements : ITopPlacements<Premiumplacement>
    {
        /// <summary>
        /// Gets the placements.
        /// </summary>
        /// <value>
        /// The placements.
        /// </value>
        public IEnumerable<Premiumplacement> Placements
        {
            get { return Premiumplacement; }
        }
    }

    public partial class Premiumplacement : MessageExtra, ITopPlacement
    {
        /// <summary>
        /// <para xml:lang="de-DE">kunden referenznummer der immobilie (nur Ausgabe)</para>
        /// <para xml:lang="en">external id of real estate OUTPUT ONLY</para>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("externalId", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "string")]
        public string ExternalId { get; set; }
    }
}
