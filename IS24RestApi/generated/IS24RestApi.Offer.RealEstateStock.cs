//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 1.0.20.0.
namespace IS24RestApi.Offer.RealEstateStock
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    
    
    /// <summary>
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "1.0.20.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("realEstateStock", Namespace="http://rest.immobilienscout24.de/schema/offer/realestatestock/1.0", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("realEstateStock", Namespace="http://rest.immobilienscout24.de/schema/offer/realestatestock/1.0")]
    public partial class RealEstateStock
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<PriceRegionRealEstateStock> _priceRegionRealEstateStock;
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("priceRegionRealEstateStock", Namespace="http://rest.immobilienscout24.de/schema/offer/realestatestock/1.0")]
        public System.Collections.ObjectModel.Collection<PriceRegionRealEstateStock> PriceRegionRealEstateStock
        {
            get
            {
                return this._priceRegionRealEstateStock;
            }
        }
        
        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="RealEstateStock" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="RealEstateStock" /> class.</para>
        /// </summary>
        public RealEstateStock()
        {
            this._priceRegionRealEstateStock = new System.Collections.ObjectModel.Collection<PriceRegionRealEstateStock>();
        }
    }
    
    /// <summary>
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "1.0.20.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("priceRegionRealEstateStock", Namespace="http://rest.immobilienscout24.de/schema/offer/realestatestock/1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("priceRegionRealEstateStock", Namespace="http://rest.immobilienscout24.de/schema/offer/realestatestock/1.0")]
    public partial class PriceRegionRealEstateStock
    {
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("priceRegion", Namespace="http://rest.immobilienscout24.de/schema/offer/realestatestock/1.0")]
        public PriceRegion PriceRegion { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private int _realestateCount = 0;
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("realestateCount", Namespace="http://rest.immobilienscout24.de/schema/offer/realestatestock/1.0", DataType="int")]
        public int RealestateCount
        {
            get
            {
                return this._realestateCount;
            }
            set
            {
                this._realestateCount = value;
            }
        }
    }
    
    /// <summary>
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "1.0.20.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("priceRegion", Namespace="http://rest.immobilienscout24.de/schema/offer/realestatestock/1.0")]
    public enum PriceRegion
    {
        
        /// <summary>
        /// </summary>
        A,
        
        /// <summary>
        /// </summary>
        B,
        
        /// <summary>
        /// </summary>
        C,
        
        /// <summary>
        /// </summary>
        D,
        
        /// <summary>
        /// </summary>
        E,
    }
}
