using System.IO;
using System.Text;

namespace ImmobilienscoutDotNet
{
    /// <summary>
    /// A <see cref="StringWriter"/> that uses <see cref="System.Text.Encoding.UTF8"/>.
    /// </summary>
    public sealed class Utf8StringWriter : StringWriter
    {
        /// <summary>
        /// The encoding.
        /// </summary>
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
}