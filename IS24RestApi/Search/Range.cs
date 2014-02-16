using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi.Search
{
    /// <summary>
    /// Represents a range of values for search.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    public class Range<T> where T: struct, IFormattable
    {
        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public T? Min { get; set; }
        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        public T? Max { get; set; }
        /// <summary>
        /// Gets the format string to be used when transforming to a parameter value.
        /// </summary>
        /// <value>
        /// The format string.
        /// </value>
        protected virtual string FormatString { get { return "f2"; } }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}-{1}", Min != null ? Min.Value.ToString(FormatString, CultureInfo.InvariantCulture) : "", 
                Max != null ? Max.Value.ToString(FormatString, CultureInfo.InvariantCulture) : "");
        }
    }

    /// <summary>
    /// A range of decimal values.
    /// </summary>
    public class DecimalRange : Range<decimal> { }

    /// <summary>
    /// A range of double values.
    /// </summary>
    public class DoubleRange : Range<double> { }

    /// <summary>
    /// A range of int values.
    /// </summary>
    public class IntRange : Range<int>
    {
        /// <summary>
        /// Gets the format string.
        /// </summary>
        /// <value>
        /// The format string.
        /// </value>
        protected override string FormatString
        {
            get
            {
                return "d";
            }
        }
    }
}
