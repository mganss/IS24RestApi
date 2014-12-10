using IS24RestApi.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

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

namespace IS24RestApi.Offer.RealEstateProject
{
    public partial class RealEstateProjectEntry
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

namespace IS24RestApi.Offer.TopPlacement
{
    public partial class Topplacement
    {
        /// <summary>
        /// <para xml:lang="de-DE">kunden referenznummer der immobilie (nur Ausgabe)</para>
        /// <para xml:lang="en">external id of real estate OUTPUT ONLY</para>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("externalId", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "string")]
        public string ExternalId { get; set; }

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

namespace IS24RestApi.Offer.ShowcasePlacement
{
    public partial class Showcaseplacement
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
    public partial class Premiumplacement
    {
        /// <summary>
        /// <para xml:lang="de-DE">kunden referenznummer der immobilie (nur Ausgabe)</para>
        /// <para xml:lang="en">external id of real estate OUTPUT ONLY</para>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("externalId", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "string")]
        public string ExternalId { get; set; }

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
