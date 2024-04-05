using System;
using System.IO;
using System.Linq;
using RestSharp;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using RestSharp.Serializers.Xml;

namespace IS24RestApi
{
    /// <summary>
    /// A deserializer which can deserialize derived classes.
    /// </summary>
    public class BaseXmlDeserializer : IXmlDeserializer
    {
        /// <summary>
        /// Ignored
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Deserialize a <see cref="IRestResponse"/> into an object.
        /// </summary>
        /// <typeparam name="T">The type of the deserialized object</typeparam>
        /// <param name="response">The response to deserialize</param>
        /// <returns>The deserialized object</returns>
        public T Deserialize<T>(RestResponse response)
        {
            if (string.IsNullOrEmpty(response.Content))
            {
                return default;
            }

            if (response.ErrorException != null)
            {
                return default;
            }

            var root = XElement.Parse(response.Content).Name.LocalName;
            var type = typeof(T);
            if (!string.Equals(type.Name, root, StringComparison.OrdinalIgnoreCase))
            {
                var derivedType = Assembly.GetAssembly(type).GetTypes()
                    .FirstOrDefault(t => t != type && type.IsAssignableFrom(t) && string.Equals(t.Name, root, StringComparison.OrdinalIgnoreCase));
                if (derivedType != null) type = derivedType;
            }

            using (var sr = new StringReader(response.Content))
            using (var xr = XmlReader.Create(sr, new XmlReaderSettings { XmlResolver = null }))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(type);
                return (T)serializer.Deserialize(xr);
            }
        }
    }
}
