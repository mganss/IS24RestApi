using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi.Search
{
    /// <summary>
    /// Possible sort parameters for search.
    /// </summary>
    public enum Sorting
    {
#pragma warning disable 1591
        Id,
        Distance,
        FirstActivation,
        Price,
        LivingSpace,
        Rooms,
        BudgetRent,
        MarketValue,
        AvailableFrom,
        UsableFloorSpace,
        BuildingType,
        TotalFloorSpace,
        Ground,
        NetFloorSpace,
        PlotArea,
        TotalRent,
        AccomodationType,
        StartRentalDate,
#pragma warning restore 1591
    }
}
