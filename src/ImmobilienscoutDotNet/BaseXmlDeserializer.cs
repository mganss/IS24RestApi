using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using RestSharp;
using RestSharp.Deserializers;

namespace ImmobilienscoutDotNet
{
    /// <summary>
    /// A deserializer which can deserialize derived classes.
    /// </summary>
    public class BaseXmlDeserializer : IDeserializer
    {
        /// <summary>
        /// Ignored
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// Ignored
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Ignored
        /// </summary>
        public string RootElement { get; set; }

        /// <summary>
        /// Deserialize a <see cref="IRestResponse"/> into an object.
        /// </summary>
        /// <typeparam name="T">The type of the deserialized object</typeparam>
        /// <param name="response">The response to deserialize</param>
        /// <returns>The deserialized object</returns>
        public T Deserialize<T>(IRestResponse response)
        {
            if (string.IsNullOrEmpty(response.Content))
            {
                return default(T);
            }

            var root = XElement.Parse(response.Content).Name.LocalName;
            var type = typeof(T);
            if (!string.Equals(type.Name, root, StringComparison.OrdinalIgnoreCase))
            {
                var derivedType = Assembly.GetAssembly(type).GetTypes()
                    .FirstOrDefault(t => t != type && type.IsAssignableFrom(t) && string.Equals(t.Name, root, StringComparison.OrdinalIgnoreCase));
                if (derivedType != null) type = derivedType;
            }

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(response.Content)))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(type);
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}
