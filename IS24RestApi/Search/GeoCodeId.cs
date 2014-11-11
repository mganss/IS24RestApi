using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi.Search
{
    /// <summary>
    /// Represents an IS24 GeoCodeId.
    /// <seealso cref="!:http://api.immobilienscout24.de/get-started/glossary.html"/>
    /// </summary>
    public class GeoCodeId
    {
        /// <summary>
        /// Gets or sets a value indicating the continent.
        /// <list type="table">
        /// <item>
        /// <term>1</term>
        /// <description>Europe</description>
        /// </item>
        /// <item>
        /// <term>2</term>
        /// <description>Asia</description>
        /// </item>
        /// <item>
        /// <term>3</term>
        /// <description>Africa</description>
        /// </item>
        /// <item>
        /// <term>4</term>
        /// <description>Americas</description>
        /// </item>
        /// <item>
        /// <term>5</term>
        /// <description>Australia</description>
        /// </item>
        /// <item>
        /// <term>6</term>
        /// <description>Antarctica :)</description>
        /// </item>
        /// </list>
        /// </summary>
        public int Continent { get; set; }
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public int Country{ get; set; }
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public int? Region { get; set; }
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public int? City { get; set; }
        /// <summary>
        /// Gets or sets the quarter.
        /// </summary>
        /// <value>
        /// The quarter.
        /// </value>
        public int? Quarter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCodeId"/> class.
        /// </summary>
        public GeoCodeId()
        {
            Continent = 1; // Europe
            Country = 276; // Germany
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var s = string.Format("{0}{1:d3}", Continent, Country);

            if (Region.HasValue)
            {
                s += Region.Value.ToString("d3");
                if (City.HasValue)
                {
                    s += City.Value.ToString("d3");
                    if (Quarter.HasValue) s += Quarter.Value.ToString("d3");
                }
            }

            return s;
        }
    }
}
