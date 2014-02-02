using IS24RestApi.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi.Search
{
    public class Query
    {
        public RealEstateType RealEstateType { get; set; }
        public string FullText { get; set; }
        public DateTime? LastModification { get; set; }
        public DateTime? PublishedAfter { get; set; }
        public DateTime? FirstActivation { get; set; }
        public string ApiSearchField1 { get; set; }
        public string ApiSearchField2 { get; set; }
        public string ApiSearchField3 { get; set; }
        public Channel Channel { get; set; }
        public object Parameters { get; set; }
        public Sorting? Sort { get; set; }
        public ListSortDirection? SortDirection { get; set; }

        const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

        public virtual IEnumerable<KeyValuePair<string, string>> GetParameters()
        {
            yield return new KeyValuePair<string, string>("realestatetype", RealEstateType.ToString().Replace("_", "").ToLowerInvariant());
            if (FullText != null) yield return new KeyValuePair<string, string>("fulltext", FullText);
            if (LastModification.HasValue) yield return new KeyValuePair<string, string>("lastmodification", LastModification.Value.ToString(DateTimeFormat));
            if (PublishedAfter.HasValue) yield return new KeyValuePair<string, string>("publishedafter", PublishedAfter.Value.ToString(DateTimeFormat));
            if (FirstActivation.HasValue) yield return new KeyValuePair<string, string>("firstactivation", FirstActivation.Value.ToString(DateTimeFormat));
            if (ApiSearchField1 != null) yield return new KeyValuePair<string, string>("apisearchfield1", ApiSearchField1);
            if (ApiSearchField2 != null) yield return new KeyValuePair<string, string>("apisearchfield2", ApiSearchField2);
            if (ApiSearchField3 != null) yield return new KeyValuePair<string, string>("apisearchfield3", ApiSearchField3);
            if (Channel != null)
            {
                foreach (var p in Channel.GetParameters())
                {
                    yield return p;
                }
            }

            var dict = Parameters as IDictionary;
            if (dict != null)
            {
                foreach (var p in dict.Keys.Cast<string>())
                {
                    yield return new KeyValuePair<string, string>(p.ToLowerInvariant(), GetParameterValue(dict[p]));
                }
            }
            else
            {
                var type = Parameters.GetType();
                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead))
                {
                    var o = prop.GetValue(Parameters);
                    if (o != null)
                        yield return new KeyValuePair<string, string>(prop.Name.ToLowerInvariant(), GetParameterValue(o));
                }
            }

            if (Sort.HasValue)
            {
                var val = Sort.Value.ToString().ToLowerInvariant();
                if (SortDirection.HasValue && SortDirection.Value == ListSortDirection.Descending) val = "-" + val;
                yield return new KeyValuePair<string, string>("sort", val);
            }
        }

        protected string GetParameterValue(object p)
        {
            if (p is string) return (string)p;
            if (p is bool) return (bool)p ? "true" : "false";
            if (p.GetType().IsEnum) return p.ToString().ToLowerInvariant();
            if (p is IEnumerable) return string.Join(",", ((IEnumerable)p).Cast<object>().Select(o => GetParameterValue(o)).ToArray());

            return p.ToString();
        }
    }

    public class RadiusQuery: Query
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Radius { get; set; }

        public override IEnumerable<KeyValuePair<string, string>> GetParameters()
        {
            yield return new KeyValuePair<string, string>("geocoordinates",
                string.Format(CultureInfo.InvariantCulture, "{0:f2};{1:f2};{2}", Latitude, Longitude, Radius));
            foreach (var p in base.GetParameters()) yield return p;
        }
    }

    public class RegionQuery : Query
    {
        public GeoCodeId GeoCodeId { get; set; }
     
        public override IEnumerable<KeyValuePair<string, string>> GetParameters()
        {
            yield return new KeyValuePair<string, string>("geocodes", GeoCodeId.ToString());
            foreach (var p in base.GetParameters()) yield return p;
        }
    }
}
