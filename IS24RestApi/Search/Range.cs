using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi.Search
{
    public class Range<T> where T: struct, IFormattable
    {
        public T? Min { get; set; }
        public T? Max { get; set; }
        protected virtual string FormatString { get { return "f2"; } }

        public override string ToString()
        {
            return string.Format("{0}-{1}", Min != null ? Min.Value.ToString(FormatString, CultureInfo.InvariantCulture) : "", 
                Max != null ? Max.Value.ToString(FormatString, CultureInfo.InvariantCulture) : "");
        }
    }

    public class DecimalRange : Range<decimal> { }

    public class DoubleRange : Range<double> { }

    public class IntRange : Range<int>
    {
        protected override string FormatString
        {
            get
            {
                return "d";
            }
        }
    }
}
