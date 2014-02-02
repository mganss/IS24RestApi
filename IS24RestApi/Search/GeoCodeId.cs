using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi.Search
{
    public class GeoCodeId
    {
        public int Continent { get; set; }
        public int Country{ get; set; }
        public int? Region { get; set; }
        public int? City { get; set; }
        public int? Quarter { get; set; }

        public GeoCodeId()
        {
            Continent = 1; // Europe
            Country = 276; // Germany
        }

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
