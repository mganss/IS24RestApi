using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace IS24RestApi
{
    public partial class Message
    {
        /// <summary>
        /// The details of the message as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}: {1}", messageCode, message);
        }
    }

    /// <summary>
    /// ExtensionMethods extending the type <see cref="messages"/>
    /// </summary>
    public static class MessagesExtensions
    {
        /// <summary>
        /// Extracts the included resource id
        /// </summary>
        /// <param name="resp"></param>
        /// <returns></returns>
        public static long? ExtractCreatedResourceId(this messages resp)
        {
            if (resp.message != null 
                && resp.message.Any() 
                && resp.message[0].messageCode == MessageCode.MESSAGE_RESOURCE_CREATED)
            {
                var m = Regex.Match(resp.message[0].message, @"with id \[(\d+)\] has been created");
                if (m.Success) return long.Parse(m.Groups[1].Value);
            }

            return null;
        }

        /// <summary>
        /// Checks if the <see cref="messages"/> contains a successful content depending on the internal <see cref="MessageCode"/>
        /// </summary>
        /// <param name="resp"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsSuccessful(this messages resp, MessageCode code = MessageCode.MESSAGE_RESOURCE_UPDATED)
        {
            return (resp.message != null
                && resp.message.Any()
                && resp.message[0].messageCode == code);
        }

        /// <summary>
        /// Collects multiple <see cref="Message"/>s into a single string.
        /// </summary>
        /// <param name="msgs">The <see cref="Message"/>s</param>
        /// <returns>A single string containing all messages</returns>
        public static string ToMessage(this Message[] msgs)
        {
            return msgs == null ? "" : string.Join(Environment.NewLine, msgs.Select(m => m.ToString()).ToArray());
        }
    }

    class AdapterHelper
    {
        public static int? GetConstructionYear(object item)
        {
            return item as int?;
        }

        public static object SetConstructionYear(int? value)
        {
            if (value.HasValue) return value.Value; else return true;
        }

        public static bool GetConstructionYearUnknown(object item)
        {
            return item is bool && ((bool)item);
        }

        public static object SetConstructionYearUnknown(bool value)
        {
            return value;
        }

        public static HeatingType? GetHeatingType(object item)
        {
            return item as HeatingType?;
        }

        public static object SetHeatingType(HeatingType? value)
        {
            return value;
        }

        public static HeatingTypeEnev2014? GetHeatingTypeEnev2014(object item)
        {
            return item as HeatingTypeEnev2014?;
        }

        public static object SetHeatingTypeEnev2014(HeatingTypeEnev2014? value)
        {
            return value;
        }

        public static EnergySourcesEnev2014 GetEnergySourcesEnev2014(object item)
        {
            return item as EnergySourcesEnev2014;
        }

        public static object SetEnergySourcesEnev2014(EnergySourcesEnev2014 value)
        {
            return value;
        }

        public static FiringTypes GetFiringTypes(object item)
        {
            return item as FiringTypes;
        }

        public static object SetFiringTypes(FiringTypes value)
        {
            return value;
        }
    }

    public partial class HouseBuy
    {
        /// <summary>
        /// <para xml:lang="de-DE">Baujahr</para>
        /// <para xml:lang="en">construction year</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [XmlIgnore]
        public int? ConstructionYear
        {
            get { return AdapterHelper.GetConstructionYear(Item); }
            set { Item = AdapterHelper.SetConstructionYear(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Baujahr ist unbekannt</para>
        /// <para xml:lang="en">construction year unknown: true value only expected, instead of false set <see cref="ConstructionYear"/></para>
        /// </summary>
        [XmlIgnore]
        public bool ConstructionYearUnknown
        {
            get { return AdapterHelper.GetConstructionYearUnknown(Item); }
            set { Item = AdapterHelper.SetConstructionYearUnknown(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">DEPRECATED benutze heatingTypeEnev2014 Heizungsart</para>
        /// <para xml:lang="en">DEPRECATED use heatingTypeEnev2014 heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingType? HeatingType
        {
            get { return AdapterHelper.GetHeatingType(Item1); }
            set { Item1 = AdapterHelper.SetHeatingType(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Heizungsart</para>
        /// <para xml:lang="en">heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingTypeEnev2014? HeatingTypeEnev2014
        {
            get { return AdapterHelper.GetHeatingTypeEnev2014(Item1); }
            set { Item1 = AdapterHelper.SetHeatingTypeEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">wesentliche Energieträger</para>
        /// <para xml:lang="en">energy sources</para>
        /// </summary>
        [XmlIgnore]
        public EnergySourcesEnev2014 EnergySourcesEnev2014
        {
            get { return AdapterHelper.GetEnergySourcesEnev2014(Item2); }
            set { Item2 = AdapterHelper.SetEnergySourcesEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Befeuerungsarten</para>
        /// <para xml:lang="en">firing types</para>
        /// </summary>
        [XmlIgnore]
        public FiringTypes FiringTypes
        {
            get { return AdapterHelper.GetFiringTypes(Item2); }
            set { Item2 = AdapterHelper.SetFiringTypes(value); }
        }
    }

    public partial class HouseRent
    {
        /// <summary>
        /// <para xml:lang="de-DE">Baujahr</para>
        /// <para xml:lang="en">construction year</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [XmlIgnore]
        public int? ConstructionYear
        {
            get { return AdapterHelper.GetConstructionYear(Item); }
            set { Item = AdapterHelper.SetConstructionYear(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Baujahr ist unbekannt</para>
        /// <para xml:lang="en">construction year unknown: true value only expected, instead of false set <see cref="ConstructionYear"/></para>
        /// </summary>
        [XmlIgnore]
        public bool ConstructionYearUnknown
        {
            get { return AdapterHelper.GetConstructionYearUnknown(Item); }
            set { Item = AdapterHelper.SetConstructionYearUnknown(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">DEPRECATED benutze heatingTypeEnev2014 Heizungsart</para>
        /// <para xml:lang="en">DEPRECATED use heatingTypeEnev2014 heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingType? HeatingType
        {
            get { return AdapterHelper.GetHeatingType(Item1); }
            set { Item1 = AdapterHelper.SetHeatingType(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Heizungsart</para>
        /// <para xml:lang="en">heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingTypeEnev2014? HeatingTypeEnev2014
        {
            get { return AdapterHelper.GetHeatingTypeEnev2014(Item1); }
            set { Item1 = AdapterHelper.SetHeatingTypeEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">wesentliche Energieträger</para>
        /// <para xml:lang="en">energy sources</para>
        /// </summary>
        [XmlIgnore]
        public EnergySourcesEnev2014 EnergySourcesEnev2014
        {
            get { return AdapterHelper.GetEnergySourcesEnev2014(Item2); }
            set { Item2 = AdapterHelper.SetEnergySourcesEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Befeuerungsarten</para>
        /// <para xml:lang="en">firing types</para>
        /// </summary>
        [XmlIgnore]
        public FiringTypes FiringTypes
        {
            get { return AdapterHelper.GetFiringTypes(Item2); }
            set { Item2 = AdapterHelper.SetFiringTypes(value); }
        }
    }

    public partial class AssistedLiving
    {
        /// <summary>
        /// <para xml:lang="de-DE">Baujahr</para>
        /// <para xml:lang="en">construction year</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [XmlIgnore]
        public int? ConstructionYear
        {
            get { return AdapterHelper.GetConstructionYear(Item); }
            set { Item = AdapterHelper.SetConstructionYear(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Baujahr ist unbekannt</para>
        /// <para xml:lang="en">construction year unknown: true value only expected, instead of false set <see cref="ConstructionYear"/></para>
        /// </summary>
        [XmlIgnore]
        public bool ConstructionYearUnknown
        {
            get { return AdapterHelper.GetConstructionYearUnknown(Item); }
            set { Item = AdapterHelper.SetConstructionYearUnknown(value); }
        }
    }

    public partial class ApartmentRent
    {
        /// <summary>
        /// <para xml:lang="de-DE">Baujahr</para>
        /// <para xml:lang="en">construction year</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [XmlIgnore]
        public int? ConstructionYear
        {
            get { return AdapterHelper.GetConstructionYear(Item); }
            set { Item = AdapterHelper.SetConstructionYear(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Baujahr ist unbekannt</para>
        /// <para xml:lang="en">construction year unknown: true value only expected, instead of false set <see cref="ConstructionYear"/></para>
        /// </summary>
        [XmlIgnore]
        public bool ConstructionYearUnknown
        {
            get { return AdapterHelper.GetConstructionYearUnknown(Item); }
            set { Item = AdapterHelper.SetConstructionYearUnknown(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">DEPRECATED benutze heatingTypeEnev2014 Heizungsart</para>
        /// <para xml:lang="en">DEPRECATED use heatingTypeEnev2014 heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingType? HeatingType
        {
            get { return AdapterHelper.GetHeatingType(Item1); }
            set { Item1 = AdapterHelper.SetHeatingType(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Heizungsart</para>
        /// <para xml:lang="en">heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingTypeEnev2014? HeatingTypeEnev2014
        {
            get { return AdapterHelper.GetHeatingTypeEnev2014(Item1); }
            set { Item1 = AdapterHelper.SetHeatingTypeEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">wesentliche Energieträger</para>
        /// <para xml:lang="en">energy sources</para>
        /// </summary>
        [XmlIgnore]
        public EnergySourcesEnev2014 EnergySourcesEnev2014
        {
            get { return AdapterHelper.GetEnergySourcesEnev2014(Item2); }
            set { Item2 = AdapterHelper.SetEnergySourcesEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Befeuerungsarten</para>
        /// <para xml:lang="en">firing types</para>
        /// </summary>
        [XmlIgnore]
        public FiringTypes FiringTypes
        {
            get { return AdapterHelper.GetFiringTypes(Item2); }
            set { Item2 = AdapterHelper.SetFiringTypes(value); }
        }
    }

    public partial class ApartmentBuy
    {
        /// <summary>
        /// <para xml:lang="de-DE">Baujahr</para>
        /// <para xml:lang="en">construction year</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [XmlIgnore]
        public int? ConstructionYear
        {
            get { return AdapterHelper.GetConstructionYear(Item); }
            set { Item = AdapterHelper.SetConstructionYear(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Baujahr ist unbekannt</para>
        /// <para xml:lang="en">construction year unknown: true value only expected, instead of false set <see cref="ConstructionYear"/></para>
        /// </summary>
        [XmlIgnore]
        public bool ConstructionYearUnknown
        {
            get { return AdapterHelper.GetConstructionYearUnknown(Item); }
            set { Item = AdapterHelper.SetConstructionYearUnknown(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">DEPRECATED benutze heatingTypeEnev2014 Heizungsart</para>
        /// <para xml:lang="en">DEPRECATED use heatingTypeEnev2014 heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingType? HeatingType
        {
            get { return AdapterHelper.GetHeatingType(Item1); }
            set { Item1 = AdapterHelper.SetHeatingType(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Heizungsart</para>
        /// <para xml:lang="en">heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingTypeEnev2014? HeatingTypeEnev2014
        {
            get { return AdapterHelper.GetHeatingTypeEnev2014(Item1); }
            set { Item1 = AdapterHelper.SetHeatingTypeEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">wesentliche Energieträger</para>
        /// <para xml:lang="en">energy sources</para>
        /// </summary>
        [XmlIgnore]
        public EnergySourcesEnev2014 EnergySourcesEnev2014
        {
            get { return AdapterHelper.GetEnergySourcesEnev2014(Item2); }
            set { Item2 = AdapterHelper.SetEnergySourcesEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Befeuerungsarten</para>
        /// <para xml:lang="en">firing types</para>
        /// </summary>
        [XmlIgnore]
        public FiringTypes FiringTypes
        {
            get { return AdapterHelper.GetFiringTypes(Item2); }
            set { Item2 = AdapterHelper.SetFiringTypes(value); }
        }
    }

    public partial class CompulsoryAuction
    {
        /// <summary>
        /// <para xml:lang="de-DE">Baujahr</para>
        /// <para xml:lang="en">construction year</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [XmlIgnore]
        public int? ConstructionYear
        {
            get { return AdapterHelper.GetConstructionYear(Item); }
            set { Item = AdapterHelper.SetConstructionYear(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Baujahr ist unbekannt</para>
        /// <para xml:lang="en">construction year unknown: true value only expected, instead of false set <see cref="ConstructionYear"/></para>
        /// </summary>
        [XmlIgnore]
        public bool ConstructionYearUnknown
        {
            get { return AdapterHelper.GetConstructionYearUnknown(Item); }
            set { Item = AdapterHelper.SetConstructionYearUnknown(value); }
        }
    }

    public partial class ShortTermAccommodation
    {
        /// <summary>
        /// <para xml:lang="de-DE">Baujahr</para>
        /// <para xml:lang="en">construction year</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [XmlIgnore]
        public int? ConstructionYear
        {
            get { return AdapterHelper.GetConstructionYear(Item); }
            set { Item = AdapterHelper.SetConstructionYear(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Baujahr ist unbekannt</para>
        /// <para xml:lang="en">construction year unknown: true value only expected, instead of false set <see cref="ConstructionYear"/></para>
        /// </summary>
        [XmlIgnore]
        public bool ConstructionYearUnknown
        {
            get { return AdapterHelper.GetConstructionYearUnknown(Item); }
            set { Item = AdapterHelper.SetConstructionYearUnknown(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">DEPRECATED benutze heatingTypeEnev2014 Heizungsart</para>
        /// <para xml:lang="en">DEPRECATED use heatingTypeEnev2014 heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingType? HeatingType
        {
            get { return AdapterHelper.GetHeatingType(Item1); }
            set { Item1 = AdapterHelper.SetHeatingType(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Heizungsart</para>
        /// <para xml:lang="en">heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingTypeEnev2014? HeatingTypeEnev2014
        {
            get { return AdapterHelper.GetHeatingTypeEnev2014(Item1); }
            set { Item1 = AdapterHelper.SetHeatingTypeEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">wesentliche Energieträger</para>
        /// <para xml:lang="en">energy sources</para>
        /// </summary>
        [XmlIgnore]
        public EnergySourcesEnev2014 EnergySourcesEnev2014
        {
            get { return AdapterHelper.GetEnergySourcesEnev2014(Item2); }
            set { Item2 = AdapterHelper.SetEnergySourcesEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Befeuerungsarten</para>
        /// <para xml:lang="en">firing types</para>
        /// </summary>
        [XmlIgnore]
        public FiringTypes FiringTypes
        {
            get { return AdapterHelper.GetFiringTypes(Item2); }
            set { Item2 = AdapterHelper.SetFiringTypes(value); }
        }
    }

    public partial class Investment
    {
        /// <summary>
        /// <para xml:lang="de-DE">Baujahr</para>
        /// <para xml:lang="en">construction year</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [XmlIgnore]
        public int? ConstructionYear
        {
            get { return AdapterHelper.GetConstructionYear(Item); }
            set { Item = AdapterHelper.SetConstructionYear(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Baujahr ist unbekannt</para>
        /// <para xml:lang="en">construction year unknown: true value only expected, instead of false set <see cref="ConstructionYear"/></para>
        /// </summary>
        [XmlIgnore]
        public bool ConstructionYearUnknown
        {
            get { return AdapterHelper.GetConstructionYearUnknown(Item); }
            set { Item = AdapterHelper.SetConstructionYearUnknown(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">DEPRECATED benutze heatingTypeEnev2014 Heizungsart</para>
        /// <para xml:lang="en">DEPRECATED use heatingTypeEnev2014 heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingType? HeatingType
        {
            get { return AdapterHelper.GetHeatingType(Item1); }
            set { Item1 = AdapterHelper.SetHeatingType(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Heizungsart</para>
        /// <para xml:lang="en">heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingTypeEnev2014? HeatingTypeEnev2014
        {
            get { return AdapterHelper.GetHeatingTypeEnev2014(Item1); }
            set { Item1 = AdapterHelper.SetHeatingTypeEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">wesentliche Energieträger</para>
        /// <para xml:lang="en">energy sources</para>
        /// </summary>
        [XmlIgnore]
        public EnergySourcesEnev2014 EnergySourcesEnev2014
        {
            get { return AdapterHelper.GetEnergySourcesEnev2014(Item2); }
            set { Item2 = AdapterHelper.SetEnergySourcesEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Befeuerungsarten</para>
        /// <para xml:lang="en">firing types</para>
        /// </summary>
        [XmlIgnore]
        public FiringTypes FiringTypes
        {
            get { return AdapterHelper.GetFiringTypes(Item2); }
            set { Item2 = AdapterHelper.SetFiringTypes(value); }
        }
    }

    public partial class Office
    {
        /// <summary>
        /// <para xml:lang="de-DE">Baujahr</para>
        /// <para xml:lang="en">construction year</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [XmlIgnore]
        public int? ConstructionYear
        {
            get { return AdapterHelper.GetConstructionYear(Item); }
            set { Item = AdapterHelper.SetConstructionYear(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Baujahr ist unbekannt</para>
        /// <para xml:lang="en">construction year unknown: true value only expected, instead of false set <see cref="ConstructionYear"/></para>
        /// </summary>
        [XmlIgnore]
        public bool ConstructionYearUnknown
        {
            get { return AdapterHelper.GetConstructionYearUnknown(Item); }
            set { Item = AdapterHelper.SetConstructionYearUnknown(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">DEPRECATED benutze heatingTypeEnev2014 Heizungsart</para>
        /// <para xml:lang="en">DEPRECATED use heatingTypeEnev2014 heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingType? HeatingType
        {
            get { return AdapterHelper.GetHeatingType(Item1); }
            set { Item1 = AdapterHelper.SetHeatingType(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Heizungsart</para>
        /// <para xml:lang="en">heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingTypeEnev2014? HeatingTypeEnev2014
        {
            get { return AdapterHelper.GetHeatingTypeEnev2014(Item1); }
            set { Item1 = AdapterHelper.SetHeatingTypeEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">wesentliche Energieträger</para>
        /// <para xml:lang="en">energy sources</para>
        /// </summary>
        [XmlIgnore]
        public EnergySourcesEnev2014 EnergySourcesEnev2014
        {
            get { return AdapterHelper.GetEnergySourcesEnev2014(Item2); }
            set { Item2 = AdapterHelper.SetEnergySourcesEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Befeuerungsarten</para>
        /// <para xml:lang="en">firing types</para>
        /// </summary>
        [XmlIgnore]
        public FiringTypes FiringTypes
        {
            get { return AdapterHelper.GetFiringTypes(Item2); }
            set { Item2 = AdapterHelper.SetFiringTypes(value); }
        }
    }

    public partial class Store
    {
        /// <summary>
        /// <para xml:lang="de-DE">Baujahr</para>
        /// <para xml:lang="en">construction year</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [XmlIgnore]
        public int? ConstructionYear
        {
            get { return AdapterHelper.GetConstructionYear(Item); }
            set { Item = AdapterHelper.SetConstructionYear(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Baujahr ist unbekannt</para>
        /// <para xml:lang="en">construction year unknown: true value only expected, instead of false set <see cref="ConstructionYear"/></para>
        /// </summary>
        [XmlIgnore]
        public bool ConstructionYearUnknown
        {
            get { return AdapterHelper.GetConstructionYearUnknown(Item); }
            set { Item = AdapterHelper.SetConstructionYearUnknown(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">DEPRECATED benutze heatingTypeEnev2014 Heizungsart</para>
        /// <para xml:lang="en">DEPRECATED use heatingTypeEnev2014 heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingType? HeatingType
        {
            get { return AdapterHelper.GetHeatingType(Item1); }
            set { Item1 = AdapterHelper.SetHeatingType(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Heizungsart</para>
        /// <para xml:lang="en">heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingTypeEnev2014? HeatingTypeEnev2014
        {
            get { return AdapterHelper.GetHeatingTypeEnev2014(Item1); }
            set { Item1 = AdapterHelper.SetHeatingTypeEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">wesentliche Energieträger</para>
        /// <para xml:lang="en">energy sources</para>
        /// </summary>
        [XmlIgnore]
        public EnergySourcesEnev2014 EnergySourcesEnev2014
        {
            get { return AdapterHelper.GetEnergySourcesEnev2014(Item2); }
            set { Item2 = AdapterHelper.SetEnergySourcesEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Befeuerungsarten</para>
        /// <para xml:lang="en">firing types</para>
        /// </summary>
        [XmlIgnore]
        public FiringTypes FiringTypes
        {
            get { return AdapterHelper.GetFiringTypes(Item2); }
            set { Item2 = AdapterHelper.SetFiringTypes(value); }
        }
    }

    public partial class Gastronomy
    {
        /// <summary>
        /// <para xml:lang="de-DE">Baujahr</para>
        /// <para xml:lang="en">construction year</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [XmlIgnore]
        public int? ConstructionYear
        {
            get { return AdapterHelper.GetConstructionYear(Item); }
            set { Item = AdapterHelper.SetConstructionYear(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Baujahr ist unbekannt</para>
        /// <para xml:lang="en">construction year unknown: true value only expected, instead of false set <see cref="ConstructionYear"/></para>
        /// </summary>
        [XmlIgnore]
        public bool ConstructionYearUnknown
        {
            get { return AdapterHelper.GetConstructionYearUnknown(Item); }
            set { Item = AdapterHelper.SetConstructionYearUnknown(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">DEPRECATED benutze heatingTypeEnev2014 Heizungsart</para>
        /// <para xml:lang="en">DEPRECATED use heatingTypeEnev2014 heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingType? HeatingType
        {
            get { return AdapterHelper.GetHeatingType(Item1); }
            set { Item1 = AdapterHelper.SetHeatingType(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Heizungsart</para>
        /// <para xml:lang="en">heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingTypeEnev2014? HeatingTypeEnev2014
        {
            get { return AdapterHelper.GetHeatingTypeEnev2014(Item1); }
            set { Item1 = AdapterHelper.SetHeatingTypeEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">wesentliche Energieträger</para>
        /// <para xml:lang="en">energy sources</para>
        /// </summary>
        [XmlIgnore]
        public EnergySourcesEnev2014 EnergySourcesEnev2014
        {
            get { return AdapterHelper.GetEnergySourcesEnev2014(Item2); }
            set { Item2 = AdapterHelper.SetEnergySourcesEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Befeuerungsarten</para>
        /// <para xml:lang="en">firing types</para>
        /// </summary>
        [XmlIgnore]
        public FiringTypes FiringTypes
        {
            get { return AdapterHelper.GetFiringTypes(Item2); }
            set { Item2 = AdapterHelper.SetFiringTypes(value); }
        }
    }

    public partial class Industry
    {
        /// <summary>
        /// <para xml:lang="de-DE">Baujahr</para>
        /// <para xml:lang="en">construction year</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [XmlIgnore]
        public int? ConstructionYear
        {
            get { return AdapterHelper.GetConstructionYear(Item); }
            set { Item = AdapterHelper.SetConstructionYear(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Baujahr ist unbekannt</para>
        /// <para xml:lang="en">construction year unknown: true value only expected, instead of false set <see cref="ConstructionYear"/></para>
        /// </summary>
        [XmlIgnore]
        public bool ConstructionYearUnknown
        {
            get { return AdapterHelper.GetConstructionYearUnknown(Item); }
            set { Item = AdapterHelper.SetConstructionYearUnknown(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">DEPRECATED benutze heatingTypeEnev2014 Heizungsart</para>
        /// <para xml:lang="en">DEPRECATED use heatingTypeEnev2014 heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingType? HeatingType
        {
            get { return AdapterHelper.GetHeatingType(Item1); }
            set { Item1 = AdapterHelper.SetHeatingType(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Heizungsart</para>
        /// <para xml:lang="en">heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingTypeEnev2014? HeatingTypeEnev2014
        {
            get { return AdapterHelper.GetHeatingTypeEnev2014(Item1); }
            set { Item1 = AdapterHelper.SetHeatingTypeEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">wesentliche Energieträger</para>
        /// <para xml:lang="en">energy sources</para>
        /// </summary>
        [XmlIgnore]
        public EnergySourcesEnev2014 EnergySourcesEnev2014
        {
            get { return AdapterHelper.GetEnergySourcesEnev2014(Item2); }
            set { Item2 = AdapterHelper.SetEnergySourcesEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Befeuerungsarten</para>
        /// <para xml:lang="en">firing types</para>
        /// </summary>
        [XmlIgnore]
        public FiringTypes FiringTypes
        {
            get { return AdapterHelper.GetFiringTypes(Item2); }
            set { Item2 = AdapterHelper.SetFiringTypes(value); }
        }
    }

    public partial class SpecialPurpose
    {
        /// <summary>
        /// <para xml:lang="de-DE">Baujahr</para>
        /// <para xml:lang="en">construction year</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [XmlIgnore]
        public int? ConstructionYear
        {
            get { return AdapterHelper.GetConstructionYear(Item); }
            set { Item = AdapterHelper.SetConstructionYear(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Baujahr ist unbekannt</para>
        /// <para xml:lang="en">construction year unknown: true value only expected, instead of false set <see cref="ConstructionYear"/></para>
        /// </summary>
        [XmlIgnore]
        public bool ConstructionYearUnknown
        {
            get { return AdapterHelper.GetConstructionYearUnknown(Item); }
            set { Item = AdapterHelper.SetConstructionYearUnknown(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">DEPRECATED benutze heatingTypeEnev2014 Heizungsart</para>
        /// <para xml:lang="en">DEPRECATED use heatingTypeEnev2014 heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingType? HeatingType
        {
            get { return AdapterHelper.GetHeatingType(Item1); }
            set { Item1 = AdapterHelper.SetHeatingType(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Heizungsart</para>
        /// <para xml:lang="en">heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingTypeEnev2014? HeatingTypeEnev2014
        {
            get { return AdapterHelper.GetHeatingTypeEnev2014(Item1); }
            set { Item1 = AdapterHelper.SetHeatingTypeEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">wesentliche Energieträger</para>
        /// <para xml:lang="en">energy sources</para>
        /// </summary>
        [XmlIgnore]
        public EnergySourcesEnev2014 EnergySourcesEnev2014
        {
            get { return AdapterHelper.GetEnergySourcesEnev2014(Item2); }
            set { Item2 = AdapterHelper.SetEnergySourcesEnev2014(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Befeuerungsarten</para>
        /// <para xml:lang="en">firing types</para>
        /// </summary>
        [XmlIgnore]
        public FiringTypes FiringTypes
        {
            get { return AdapterHelper.GetFiringTypes(Item2); }
            set { Item2 = AdapterHelper.SetFiringTypes(value); }
        }
    }

    public partial class FlatShareRoom
    {
        /// <summary>
        /// <para xml:lang="de-DE">Baujahr</para>
        /// <para xml:lang="en">construction year</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [XmlIgnore]
        public int? ConstructionYear
        {
            get { return AdapterHelper.GetConstructionYear(Item); }
            set { Item = AdapterHelper.SetConstructionYear(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Baujahr ist unbekannt</para>
        /// <para xml:lang="en">construction year unknown: true value only expected, instead of false set <see cref="ConstructionYear"/></para>
        /// </summary>
        [XmlIgnore]
        public bool ConstructionYearUnknown
        {
            get { return AdapterHelper.GetConstructionYearUnknown(Item); }
            set { Item = AdapterHelper.SetConstructionYearUnknown(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">DEPRECATED benutze heatingTypeEnev2014 Heizungsart</para>
        /// <para xml:lang="en">DEPRECATED use heatingTypeEnev2014 heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingType? HeatingType
        {
            get { return AdapterHelper.GetHeatingType(Item1); }
            set { Item1 = AdapterHelper.SetHeatingType(value); }
        }

        /// <summary>
        /// <para xml:lang="de-DE">Heizungsart</para>
        /// <para xml:lang="en">heating type</para>
        /// </summary>
        [XmlIgnore]
        public HeatingTypeEnev2014? HeatingTypeEnev2014
        {
            get { return AdapterHelper.GetHeatingTypeEnev2014(Item1); }
            set { Item1 = AdapterHelper.SetHeatingTypeEnev2014(value); }
        }
    }
}
