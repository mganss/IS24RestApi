using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IS24RestApi.Tests
{
    public class ChoiceAdapterPropertiesTests
    {
        [Fact]
        public void ChoiceAdapterPropertiesWork()
        {
            HouseBuyChoiceAdapterPropertiesWork();
            ApartmentRentChoiceAdapterPropertiesWork();
            ApartmentBuyChoiceAdapterPropertiesWork();
            AssistedLivingChoiceAdapterPropertiesWork();
            CompulsoryAuctionChoiceAdapterPropertiesWork();
            FlatShareRoomChoiceAdapterPropertiesWork();
            GastronomyChoiceAdapterPropertiesWork();
            HouseRentChoiceAdapterPropertiesWork();
            IndustryChoiceAdapterPropertiesWork();
            InvestmentChoiceAdapterPropertiesWork();
            OfficeChoiceAdapterPropertiesWork();
            ShortTermAccommodationChoiceAdapterPropertiesWork();
            SpecialPurposeChoiceAdapterPropertiesWork();
            StoreChoiceAdapterPropertiesWork();
        }

        private static void HouseBuyChoiceAdapterPropertiesWork()
        {
            var re = new HouseBuy();

            Assert.Null(re.Item);
            Assert.Null(re.ConstructionYear);
            re.ConstructionYear = 1900;
            Assert.Equal(1900, re.Item);
            Assert.Equal(1900, re.ConstructionYear);
            re.ConstructionYear = null;
            Assert.Null(re.ConstructionYear);
            Assert.True(re.ConstructionYearUnknown);
            re.ConstructionYearUnknown = true;
            Assert.IsType<bool>(re.Item);
            Assert.True((bool)re.Item);
            Assert.Null(re.ConstructionYear);

            Assert.Null(re.Item1);
            Assert.Null(re.HeatingType);
            Assert.Null(re.HeatingTypeEnev2014);
            re.HeatingType = HeatingType.STOVE_HEATING;
            Assert.IsType<HeatingType>(re.Item1);
            Assert.Equal(HeatingType.STOVE_HEATING, re.Item1);
            re.HeatingTypeEnev2014 = HeatingTypeEnev2014.FLOOR_HEATING;
            Assert.IsType<HeatingTypeEnev2014>(re.Item1);
            Assert.Equal(HeatingTypeEnev2014.FLOOR_HEATING, re.Item1);
            Assert.Null(re.HeatingType);

            Assert.Null(re.Item2);
            Assert.Null(re.FiringTypes);
            Assert.Null(re.EnergySourcesEnev2014);
            re.FiringTypes = new FiringTypes();
            Assert.IsType<FiringTypes>(re.Item2);
            Assert.Null(re.EnergySourcesEnev2014);
            re.EnergySourcesEnev2014 = new EnergySourcesEnev2014();
            Assert.IsType<EnergySourcesEnev2014>(re.Item2);
            Assert.Null(re.FiringTypes);
        }

        private static void ApartmentRentChoiceAdapterPropertiesWork()
        {
            var re = new ApartmentRent();

            Assert.Null(re.Item);
            Assert.Null(re.ConstructionYear);
            re.ConstructionYear = 1900;
            Assert.Equal(1900, re.Item);
            Assert.Equal(1900, re.ConstructionYear);
            re.ConstructionYear = null;
            Assert.Null(re.ConstructionYear);
            Assert.True(re.ConstructionYearUnknown);
            re.ConstructionYearUnknown = true;
            Assert.IsType<bool>(re.Item);
            Assert.True((bool)re.Item);
            Assert.Null(re.ConstructionYear);

            Assert.Null(re.Item1);
            Assert.Null(re.HeatingType);
            Assert.Null(re.HeatingTypeEnev2014);
            re.HeatingType = HeatingType.STOVE_HEATING;
            Assert.IsType<HeatingType>(re.Item1);
            Assert.Equal(HeatingType.STOVE_HEATING, re.Item1);
            re.HeatingTypeEnev2014 = HeatingTypeEnev2014.FLOOR_HEATING;
            Assert.IsType<HeatingTypeEnev2014>(re.Item1);
            Assert.Equal(HeatingTypeEnev2014.FLOOR_HEATING, re.Item1);
            Assert.Null(re.HeatingType);

            Assert.Null(re.Item2);
            Assert.Null(re.FiringTypes);
            Assert.Null(re.EnergySourcesEnev2014);
            re.FiringTypes = new FiringTypes();
            Assert.IsType<FiringTypes>(re.Item2);
            Assert.Null(re.EnergySourcesEnev2014);
            re.EnergySourcesEnev2014 = new EnergySourcesEnev2014();
            Assert.IsType<EnergySourcesEnev2014>(re.Item2);
            Assert.Null(re.FiringTypes);
        }

        private static void ApartmentBuyChoiceAdapterPropertiesWork()
        {
            var re = new ApartmentBuy();

            Assert.Null(re.Item);
            Assert.Null(re.ConstructionYear);
            re.ConstructionYear = 1900;
            Assert.Equal(1900, re.Item);
            Assert.Equal(1900, re.ConstructionYear);
            re.ConstructionYear = null;
            Assert.Null(re.ConstructionYear);
            Assert.True(re.ConstructionYearUnknown);
            re.ConstructionYearUnknown = true;
            Assert.IsType<bool>(re.Item);
            Assert.True((bool)re.Item);
            Assert.Null(re.ConstructionYear);

            Assert.Null(re.Item1);
            Assert.Null(re.HeatingType);
            Assert.Null(re.HeatingTypeEnev2014);
            re.HeatingType = HeatingType.STOVE_HEATING;
            Assert.IsType<HeatingType>(re.Item1);
            Assert.Equal(HeatingType.STOVE_HEATING, re.Item1);
            re.HeatingTypeEnev2014 = HeatingTypeEnev2014.FLOOR_HEATING;
            Assert.IsType<HeatingTypeEnev2014>(re.Item1);
            Assert.Equal(HeatingTypeEnev2014.FLOOR_HEATING, re.Item1);
            Assert.Null(re.HeatingType);

            Assert.Null(re.Item2);
            Assert.Null(re.FiringTypes);
            Assert.Null(re.EnergySourcesEnev2014);
            re.FiringTypes = new FiringTypes();
            Assert.IsType<FiringTypes>(re.Item2);
            Assert.Null(re.EnergySourcesEnev2014);
            re.EnergySourcesEnev2014 = new EnergySourcesEnev2014();
            Assert.IsType<EnergySourcesEnev2014>(re.Item2);
            Assert.Null(re.FiringTypes);
        }

        private static void AssistedLivingChoiceAdapterPropertiesWork()
        {
            var re = new AssistedLiving();

            Assert.Null(re.Item);
            Assert.Null(re.ConstructionYear);
            re.ConstructionYear = 1900;
            Assert.Equal(1900, re.Item);
            Assert.Equal(1900, re.ConstructionYear);
            re.ConstructionYear = null;
            Assert.Null(re.ConstructionYear);
            Assert.True(re.ConstructionYearUnknown);
            re.ConstructionYearUnknown = true;
            Assert.IsType<bool>(re.Item);
            Assert.True((bool)re.Item);
            Assert.Null(re.ConstructionYear);
        }

        private static void CompulsoryAuctionChoiceAdapterPropertiesWork()
        {
            var re = new CompulsoryAuction();

            Assert.Null(re.Item);
            Assert.Null(re.ConstructionYear);
            re.ConstructionYear = 1900;
            Assert.Equal(1900, re.Item);
            Assert.Equal(1900, re.ConstructionYear);
            re.ConstructionYear = null;
            Assert.Null(re.ConstructionYear);
            Assert.True(re.ConstructionYearUnknown);
            re.ConstructionYearUnknown = true;
            Assert.IsType<bool>(re.Item);
            Assert.True((bool)re.Item);
            Assert.Null(re.ConstructionYear);
        }

        private static void FlatShareRoomChoiceAdapterPropertiesWork()
        {
            var re = new FlatShareRoom();

            Assert.Null(re.Item);
            Assert.Null(re.ConstructionYear);
            re.ConstructionYear = 1900;
            Assert.Equal(1900, re.Item);
            Assert.Equal(1900, re.ConstructionYear);
            re.ConstructionYear = null;
            Assert.Null(re.ConstructionYear);
            Assert.True(re.ConstructionYearUnknown);
            re.ConstructionYearUnknown = true;
            Assert.IsType<bool>(re.Item);
            Assert.True((bool)re.Item);
            Assert.Null(re.ConstructionYear);

            Assert.Null(re.Item1);
            Assert.Null(re.HeatingType);
            Assert.Null(re.HeatingTypeEnev2014);
            re.HeatingType = HeatingType.STOVE_HEATING;
            Assert.IsType<HeatingType>(re.Item1);
            Assert.Equal(HeatingType.STOVE_HEATING, re.Item1);
            re.HeatingTypeEnev2014 = HeatingTypeEnev2014.FLOOR_HEATING;
            Assert.IsType<HeatingTypeEnev2014>(re.Item1);
            Assert.Equal(HeatingTypeEnev2014.FLOOR_HEATING, re.Item1);
            Assert.Null(re.HeatingType);
        }

        private static void GastronomyChoiceAdapterPropertiesWork()
        {
            var re = new Gastronomy();

            Assert.Null(re.Item);
            Assert.Null(re.ConstructionYear);
            re.ConstructionYear = 1900;
            Assert.Equal(1900, re.Item);
            Assert.Equal(1900, re.ConstructionYear);
            re.ConstructionYear = null;
            Assert.Null(re.ConstructionYear);
            Assert.True(re.ConstructionYearUnknown);
            re.ConstructionYearUnknown = true;
            Assert.IsType<bool>(re.Item);
            Assert.True((bool)re.Item);
            Assert.Null(re.ConstructionYear);

            Assert.Null(re.Item1);
            Assert.Null(re.HeatingType);
            Assert.Null(re.HeatingTypeEnev2014);
            re.HeatingType = HeatingType.STOVE_HEATING;
            Assert.IsType<HeatingType>(re.Item1);
            Assert.Equal(HeatingType.STOVE_HEATING, re.Item1);
            re.HeatingTypeEnev2014 = HeatingTypeEnev2014.FLOOR_HEATING;
            Assert.IsType<HeatingTypeEnev2014>(re.Item1);
            Assert.Equal(HeatingTypeEnev2014.FLOOR_HEATING, re.Item1);
            Assert.Null(re.HeatingType);

            Assert.Null(re.Item2);
            Assert.Null(re.FiringTypes);
            Assert.Null(re.EnergySourcesEnev2014);
            re.FiringTypes = new FiringTypes();
            Assert.IsType<FiringTypes>(re.Item2);
            Assert.Null(re.EnergySourcesEnev2014);
            re.EnergySourcesEnev2014 = new EnergySourcesEnev2014();
            Assert.IsType<EnergySourcesEnev2014>(re.Item2);
            Assert.Null(re.FiringTypes);
        }

        private static void HouseRentChoiceAdapterPropertiesWork()
        {
            var re = new HouseRent();

            Assert.Null(re.Item);
            Assert.Null(re.ConstructionYear);
            re.ConstructionYear = 1900;
            Assert.Equal(1900, re.Item);
            Assert.Equal(1900, re.ConstructionYear);
            re.ConstructionYear = null;
            Assert.Null(re.ConstructionYear);
            Assert.True(re.ConstructionYearUnknown);
            re.ConstructionYearUnknown = true;
            Assert.IsType<bool>(re.Item);
            Assert.True((bool)re.Item);
            Assert.Null(re.ConstructionYear);

            Assert.Null(re.Item1);
            Assert.Null(re.HeatingType);
            Assert.Null(re.HeatingTypeEnev2014);
            re.HeatingType = HeatingType.STOVE_HEATING;
            Assert.IsType<HeatingType>(re.Item1);
            Assert.Equal(HeatingType.STOVE_HEATING, re.Item1);
            re.HeatingTypeEnev2014 = HeatingTypeEnev2014.FLOOR_HEATING;
            Assert.IsType<HeatingTypeEnev2014>(re.Item1);
            Assert.Equal(HeatingTypeEnev2014.FLOOR_HEATING, re.Item1);
            Assert.Null(re.HeatingType);

            Assert.Null(re.Item2);
            Assert.Null(re.FiringTypes);
            Assert.Null(re.EnergySourcesEnev2014);
            re.FiringTypes = new FiringTypes();
            Assert.IsType<FiringTypes>(re.Item2);
            Assert.Null(re.EnergySourcesEnev2014);
            re.EnergySourcesEnev2014 = new EnergySourcesEnev2014();
            Assert.IsType<EnergySourcesEnev2014>(re.Item2);
            Assert.Null(re.FiringTypes);
        }

        private static void IndustryChoiceAdapterPropertiesWork()
        {
            var re = new Industry();

            Assert.Null(re.Item);
            Assert.Null(re.ConstructionYear);
            re.ConstructionYear = 1900;
            Assert.Equal(1900, re.Item);
            Assert.Equal(1900, re.ConstructionYear);
            re.ConstructionYear = null;
            Assert.Null(re.ConstructionYear);
            Assert.True(re.ConstructionYearUnknown);
            re.ConstructionYearUnknown = true;
            Assert.IsType<bool>(re.Item);
            Assert.True((bool)re.Item);
            Assert.Null(re.ConstructionYear);

            Assert.Null(re.Item1);
            Assert.Null(re.HeatingType);
            Assert.Null(re.HeatingTypeEnev2014);
            re.HeatingType = HeatingType.STOVE_HEATING;
            Assert.IsType<HeatingType>(re.Item1);
            Assert.Equal(HeatingType.STOVE_HEATING, re.Item1);
            re.HeatingTypeEnev2014 = HeatingTypeEnev2014.FLOOR_HEATING;
            Assert.IsType<HeatingTypeEnev2014>(re.Item1);
            Assert.Equal(HeatingTypeEnev2014.FLOOR_HEATING, re.Item1);
            Assert.Null(re.HeatingType);

            Assert.Null(re.Item2);
            Assert.Null(re.FiringTypes);
            Assert.Null(re.EnergySourcesEnev2014);
            re.FiringTypes = new FiringTypes();
            Assert.IsType<FiringTypes>(re.Item2);
            Assert.Null(re.EnergySourcesEnev2014);
            re.EnergySourcesEnev2014 = new EnergySourcesEnev2014();
            Assert.IsType<EnergySourcesEnev2014>(re.Item2);
            Assert.Null(re.FiringTypes);
        }

        private static void InvestmentChoiceAdapterPropertiesWork()
        {
            var re = new Investment();

            Assert.Null(re.Item);
            Assert.Null(re.ConstructionYear);
            re.ConstructionYear = 1900;
            Assert.Equal(1900, re.Item);
            Assert.Equal(1900, re.ConstructionYear);
            re.ConstructionYear = null;
            Assert.Null(re.ConstructionYear);
            Assert.True(re.ConstructionYearUnknown);
            re.ConstructionYearUnknown = true;
            Assert.IsType<bool>(re.Item);
            Assert.True((bool)re.Item);
            Assert.Null(re.ConstructionYear);

            Assert.Null(re.Item1);
            Assert.Null(re.HeatingType);
            Assert.Null(re.HeatingTypeEnev2014);
            re.HeatingType = HeatingType.STOVE_HEATING;
            Assert.IsType<HeatingType>(re.Item1);
            Assert.Equal(HeatingType.STOVE_HEATING, re.Item1);
            re.HeatingTypeEnev2014 = HeatingTypeEnev2014.FLOOR_HEATING;
            Assert.IsType<HeatingTypeEnev2014>(re.Item1);
            Assert.Equal(HeatingTypeEnev2014.FLOOR_HEATING, re.Item1);
            Assert.Null(re.HeatingType);

            Assert.Null(re.Item2);
            Assert.Null(re.FiringTypes);
            Assert.Null(re.EnergySourcesEnev2014);
            re.FiringTypes = new FiringTypes();
            Assert.IsType<FiringTypes>(re.Item2);
            Assert.Null(re.EnergySourcesEnev2014);
            re.EnergySourcesEnev2014 = new EnergySourcesEnev2014();
            Assert.IsType<EnergySourcesEnev2014>(re.Item2);
            Assert.Null(re.FiringTypes);
        }

        private static void OfficeChoiceAdapterPropertiesWork()
        {
            var re = new Office();

            Assert.Null(re.Item);
            Assert.Null(re.ConstructionYear);
            re.ConstructionYear = 1900;
            Assert.Equal(1900, re.Item);
            Assert.Equal(1900, re.ConstructionYear);
            re.ConstructionYear = null;
            Assert.Null(re.ConstructionYear);
            Assert.True(re.ConstructionYearUnknown);
            re.ConstructionYearUnknown = true;
            Assert.IsType<bool>(re.Item);
            Assert.True((bool)re.Item);
            Assert.Null(re.ConstructionYear);

            Assert.Null(re.Item1);
            Assert.Null(re.HeatingType);
            Assert.Null(re.HeatingTypeEnev2014);
            re.HeatingType = HeatingType.STOVE_HEATING;
            Assert.IsType<HeatingType>(re.Item1);
            Assert.Equal(HeatingType.STOVE_HEATING, re.Item1);
            re.HeatingTypeEnev2014 = HeatingTypeEnev2014.FLOOR_HEATING;
            Assert.IsType<HeatingTypeEnev2014>(re.Item1);
            Assert.Equal(HeatingTypeEnev2014.FLOOR_HEATING, re.Item1);
            Assert.Null(re.HeatingType);

            Assert.Null(re.Item2);
            Assert.Null(re.FiringTypes);
            Assert.Null(re.EnergySourcesEnev2014);
            re.FiringTypes = new FiringTypes();
            Assert.IsType<FiringTypes>(re.Item2);
            Assert.Null(re.EnergySourcesEnev2014);
            re.EnergySourcesEnev2014 = new EnergySourcesEnev2014();
            Assert.IsType<EnergySourcesEnev2014>(re.Item2);
            Assert.Null(re.FiringTypes);
        }

        private static void ShortTermAccommodationChoiceAdapterPropertiesWork()
        {
            var re = new ShortTermAccommodation();

            Assert.Null(re.Item);
            Assert.Null(re.ConstructionYear);
            re.ConstructionYear = 1900;
            Assert.Equal(1900, re.Item);
            Assert.Equal(1900, re.ConstructionYear);
            re.ConstructionYear = null;
            Assert.Null(re.ConstructionYear);
            Assert.True(re.ConstructionYearUnknown);
            re.ConstructionYearUnknown = true;
            Assert.IsType<bool>(re.Item);
            Assert.True((bool)re.Item);
            Assert.Null(re.ConstructionYear);

            Assert.Null(re.Item1);
            Assert.Null(re.HeatingType);
            Assert.Null(re.HeatingTypeEnev2014);
            re.HeatingType = HeatingType.STOVE_HEATING;
            Assert.IsType<HeatingType>(re.Item1);
            Assert.Equal(HeatingType.STOVE_HEATING, re.Item1);
            re.HeatingTypeEnev2014 = HeatingTypeEnev2014.FLOOR_HEATING;
            Assert.IsType<HeatingTypeEnev2014>(re.Item1);
            Assert.Equal(HeatingTypeEnev2014.FLOOR_HEATING, re.Item1);
            Assert.Null(re.HeatingType);

            Assert.Null(re.Item2);
            Assert.Null(re.FiringTypes);
            Assert.Null(re.EnergySourcesEnev2014);
            re.FiringTypes = new FiringTypes();
            Assert.IsType<FiringTypes>(re.Item2);
            Assert.Null(re.EnergySourcesEnev2014);
            re.EnergySourcesEnev2014 = new EnergySourcesEnev2014();
            Assert.IsType<EnergySourcesEnev2014>(re.Item2);
            Assert.Null(re.FiringTypes);
        }

        private static void SpecialPurposeChoiceAdapterPropertiesWork()
        {
            var re = new SpecialPurpose();

            Assert.Null(re.Item);
            Assert.Null(re.ConstructionYear);
            re.ConstructionYear = 1900;
            Assert.Equal(1900, re.Item);
            Assert.Equal(1900, re.ConstructionYear);
            re.ConstructionYear = null;
            Assert.Null(re.ConstructionYear);
            Assert.True(re.ConstructionYearUnknown);
            re.ConstructionYearUnknown = true;
            Assert.IsType<bool>(re.Item);
            Assert.True((bool)re.Item);
            Assert.Null(re.ConstructionYear);

            Assert.Null(re.Item1);
            Assert.Null(re.HeatingType);
            Assert.Null(re.HeatingTypeEnev2014);
            re.HeatingType = HeatingType.STOVE_HEATING;
            Assert.IsType<HeatingType>(re.Item1);
            Assert.Equal(HeatingType.STOVE_HEATING, re.Item1);
            re.HeatingTypeEnev2014 = HeatingTypeEnev2014.FLOOR_HEATING;
            Assert.IsType<HeatingTypeEnev2014>(re.Item1);
            Assert.Equal(HeatingTypeEnev2014.FLOOR_HEATING, re.Item1);
            Assert.Null(re.HeatingType);

            Assert.Null(re.Item2);
            Assert.Null(re.FiringTypes);
            Assert.Null(re.EnergySourcesEnev2014);
            re.FiringTypes = new FiringTypes();
            Assert.IsType<FiringTypes>(re.Item2);
            Assert.Null(re.EnergySourcesEnev2014);
            re.EnergySourcesEnev2014 = new EnergySourcesEnev2014();
            Assert.IsType<EnergySourcesEnev2014>(re.Item2);
            Assert.Null(re.FiringTypes);
        }

        private static void StoreChoiceAdapterPropertiesWork()
        {
            var re = new Store();

            Assert.Null(re.Item);
            Assert.Null(re.ConstructionYear);
            re.ConstructionYear = 1900;
            Assert.Equal(1900, re.Item);
            Assert.Equal(1900, re.ConstructionYear);
            re.ConstructionYear = null;
            Assert.Null(re.ConstructionYear);
            Assert.True(re.ConstructionYearUnknown);
            re.ConstructionYearUnknown = true;
            Assert.IsType<bool>(re.Item);
            Assert.True((bool)re.Item);
            Assert.Null(re.ConstructionYear);

            Assert.Null(re.Item1);
            Assert.Null(re.HeatingType);
            Assert.Null(re.HeatingTypeEnev2014);
            re.HeatingType = HeatingType.STOVE_HEATING;
            Assert.IsType<HeatingType>(re.Item1);
            Assert.Equal(HeatingType.STOVE_HEATING, re.Item1);
            re.HeatingTypeEnev2014 = HeatingTypeEnev2014.FLOOR_HEATING;
            Assert.IsType<HeatingTypeEnev2014>(re.Item1);
            Assert.Equal(HeatingTypeEnev2014.FLOOR_HEATING, re.Item1);
            Assert.Null(re.HeatingType);

            Assert.Null(re.Item2);
            Assert.Null(re.FiringTypes);
            Assert.Null(re.EnergySourcesEnev2014);
            re.FiringTypes = new FiringTypes();
            Assert.IsType<FiringTypes>(re.Item2);
            Assert.Null(re.EnergySourcesEnev2014);
            re.EnergySourcesEnev2014 = new EnergySourcesEnev2014();
            Assert.IsType<EnergySourcesEnev2014>(re.Item2);
            Assert.Null(re.FiringTypes);
        }
    }
}
