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
    /// <summary>
    /// Represents search features.
    /// </summary>
    public enum Feature
    {
        /// <summary>
        /// Enables the search for grouped/similar objects this feature applies for commercial objects.
        /// </summary>
        Grouping, 
        /// <summary>
        /// When this feature is enabled the count for each search criteria is returned. 
        /// Please note that the returned match counts have the same capitalization as the search parameter. 
        /// Also not all parameters that we have in the search are returned as match counts.
        /// </summary>
        MatchCount
    }

    /// <summary>
    /// Represents a search query.
    /// </summary>
    public abstract class Query
    {
        /// <summary>
        /// Gets or sets the type of the real estate.
        /// </summary>
        /// <value>
        /// The type of the real estate.
        /// </value>
        public RealEstateType RealEstateType { get; set; }
        /// <summary>
        /// Gets or sets the full text search parameter.
        /// The whole expose is searched: full text as well as values within the other expose fields (like "Balcony true/false". Separator is "%20". Example: Balkon%20Altbau.
        /// </summary>
        /// <value>
        /// The string to search for.
        /// </value>
        public string FullText { get; set; }
        /// <summary>
        /// Gets or sets the last modification date search parameter.
        /// Objects that were created (the object is not yet published, please use the filter "firstactivation" to obtain objects which are published after date X) or modified after the given date and time will be returned.
        /// </summary>
        /// <value>
        /// The last modification date.
        /// </value>
        public DateTime? LastModification { get; set; }
        /// <summary>
        /// Gets or sets the published after date.
        /// Objects will get an additiional attribute "publishedAfter". It is a boolean field with information whether the object publish date was after the publishedafter query date. Objects Are not filtered, it is only "decoration" of the object with an additional attribute.
        /// </summary>
        /// <value>
        /// The published after date.
        /// </value>
        public DateTime? PublishedAfter { get; set; }
        /// <summary>
        /// Gets or sets the first activation date.
        /// </summary>
        /// <value>
        /// The first activation date.
        /// Objects that were published after the given date and time will be returned.
        /// </value>
        public DateTime? FirstActivation { get; set; }
        /// <summary>
        /// Gets or sets the API search field 1.
        /// </summary>
        /// <value>
        /// The API search field 1.
        /// </value>
        public string ApiSearchField1 { get; set; }
        /// <summary>
        /// Gets or sets the API search field 2.
        /// </summary>
        /// <value>
        /// The API search field 2.
        /// </value>
        public string ApiSearchField2 { get; set; }
        /// <summary>
        /// Gets or sets the API search field 3.
        /// </summary>
        /// <value>
        /// The API search field 3.
        /// </value>
        public string ApiSearchField3 { get; set; }
        /// <summary>
        /// Gets or sets the channel.
        /// </summary>
        /// <value>
        /// The channel.
        /// </value>
        public Channel Channel { get; set; }
        /// <summary>
        /// Gets the search features list.
        /// </summary>
        /// <value>
        /// The search features.
        /// </value>
        public IList<Feature> Features { get; private set; }
        /// <summary>
        /// Gets or sets the search parameters.
        /// Can be an <see cref="IDictionary"/> or any object with properties.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public object Parameters { get; set; }
        /// <summary>
        /// Gets or sets the sort parameter.
        /// </summary>
        /// <value>
        /// The sort parameter.
        /// </value>
        public Sorting? Sort { get; set; }
        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        /// <value>
        /// The sort direction.
        /// </value>
        public ListSortDirection? SortDirection { get; set; }

        const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

        /// <summary>
        /// Initializes a new instance of the <see cref="Query"/> class.
        /// </summary>
        public Query()
        {
            Features = new List<Feature>();
        }

        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        /// <returns></returns>
        public abstract string GetResource();

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <returns></returns>
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
            if (Features.Any()) yield return new KeyValuePair<string, string>("features", GetParameterValue(Features));

            var dict = Parameters as IDictionary;
            if (dict != null)
            {
                foreach (var p in dict.Keys.Cast<object>().Select(o => o.ToString()))
                {
                    yield return new KeyValuePair<string, string>(p.ToLowerInvariant(), GetParameterValue(dict[p]));
                }
            }
            else if (Parameters != null)
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

        /// <summary>
        /// Gets the parameter value from an object.
        /// </summary>
        /// <param name="p">The object.</param>
        /// <returns></returns>
        protected string GetParameterValue(object p)
        {
            if (p is string) return (string)p;
            if (p is bool) return (bool)p ? "true" : "false";
            if (p.GetType().IsEnum) return p.ToString().ToLowerInvariant();
            if (p is IEnumerable) return string.Join(",", ((IEnumerable)p).Cast<object>().Select(o => GetParameterValue(o)).ToArray());

            return p.ToString();
        }
    }

    /// <summary>
    /// Represents a radius query.
    /// </summary>
    public class RadiusQuery: Query
    {
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public double Latitude { get; set; }
        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public double Longitude { get; set; }
        /// <summary>
        /// Gets or sets the radius in km.
        /// </summary>
        /// <value>
        /// The radius in km.
        /// </value>
        public int Radius { get; set; }

        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        /// <returns></returns>
        public override string GetResource()
        {
            return "radius";
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <returns>The parameters.</returns>
        public override IEnumerable<KeyValuePair<string, string>> GetParameters()
        {
            yield return new KeyValuePair<string, string>("geocoordinates",
                string.Format(CultureInfo.InvariantCulture, "{0:f2};{1:f2};{2}", Latitude, Longitude, Radius));
            foreach (var p in base.GetParameters()) yield return p;
        }
    }

    /// <summary>
    /// Represents a region query.
    /// </summary>
    public class RegionQuery : Query
    {
        /// <summary>
        /// Gets or sets the geo code identifier.
        /// </summary>
        /// <value>
        /// The geo code identifier.
        /// </value>
        public GeoCodeId GeoCodeId { get; set; }

        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        /// <returns></returns>
        public override string GetResource()
        {
            return "region";
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<KeyValuePair<string, string>> GetParameters()
        {
            yield return new KeyValuePair<string, string>("geocodes", GeoCodeId.ToString());
            foreach (var p in base.GetParameters()) yield return p;
        }
    }
}
