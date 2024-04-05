using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using RestSharp;
using RestSharp.Serializers.Xml;

namespace IS24RestApi
{
    /// <summary>
    /// A serializer which can serialize objects into XML representations of their base class.
    /// </summary>
    public class BaseXmlSerializer: IXmlSerializer
    {
        /// <summary>
        /// Serializes an object into an XML document using the specified type.
        /// </summary>
        /// <param name="o">The object to serialize</param>
        /// <param name="t">The type</param>
        /// <returns></returns>
        public string Serialize(object o)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, Namespace);
            var serializer = new System.Xml.Serialization.XmlSerializer(o.GetType());
            var writer = new EncodingStringWriter(Encoding.UTF8);
            serializer.Serialize(writer, o, ns);

            return writer.ToString();
        }

        public ContentType ContentType { get; set; }
        public string Namespace { get; set; }

        private class EncodingStringWriter : StringWriter
        {
            private readonly Encoding encoding;

            public EncodingStringWriter(Encoding encoding)
            {
                this.encoding = encoding;
            }

            public override Encoding Encoding
            {
                get { return encoding; }
            }
        }
    }
}
