//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 0.6.5640.32393.
namespace IS24RestApi.Offer.RealEstateProject
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    
    
    /// <summary>
    /// <para xml:lang="de-DE">Liste von Bauprojekte</para>
    /// <para xml:lang="de-DE">Bauprojekteliste</para>
    /// <para xml:lang="en">List of real estate projects</para>
    /// <para xml:lang="en">list of the real estate projects</para>
    /// </summary>
    [System.SerializableAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "0.6.5640.32393")]
    [System.Xml.Serialization.XmlTypeAttribute("RealEstateProjects", Namespace="http://rest.immobilienscout24.de/schema/offer/realestateproject/1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("realEstateProjects", Namespace="http://rest.immobilienscout24.de/schema/offer/realestateproject/1.0")]
    public partial class RealEstateProjects
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<RealEstateProject> _realEstateProject;
        
        /// <summary>
        /// <para xml:lang="de-DE">Bauprojekt</para>
        /// <para xml:lang="en">an real estate project</para>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("realEstateProject", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.ObjectModel.Collection<RealEstateProject> RealEstateProject
        {
            get
            {
                return this._realEstateProject;
            }
        }
        
        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die RealEstateProject-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the RealEstateProject collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RealEstateProjectSpecified
        {
            get
            {
                return (this.RealEstateProject.Count != 0);
            }
        }
        
        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="RealEstateProjects" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="RealEstateProjects" /> class.</para>
        /// </summary>
        public RealEstateProjects()
        {
            this._realEstateProject = new System.Collections.ObjectModel.Collection<RealEstateProject>();
        }
    }
    
    /// <summary>
    /// <para xml:lang="de-DE">realestateproject</para>
    /// <para xml:lang="de-DE">Realestateproject</para>
    /// <para xml:lang="en">realestateproject</para>
    /// <para xml:lang="en">real estate project</para>
    /// </summary>
    [System.SerializableAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "0.6.5640.32393")]
    [System.Xml.Serialization.XmlTypeAttribute("RealEstateProject", Namespace="http://rest.immobilienscout24.de/schema/offer/realestateproject/1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("realEstateProject", Namespace="http://rest.immobilienscout24.de/schema/offer/realestateproject/1.0")]
    public partial class RealEstateProject
    {
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("title", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="string")]
        public string Title { get; set; }
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("price", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public IS24RestApi.Common.PriceRangeMandatory Price { get; set; }
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("space", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public IS24RestApi.Common.AreaRangeMandatory Space { get; set; }
        
        /// <summary>
        /// <para xml:lang="de-DE">READONLY: minimaler Quadratmeterprice</para>
        /// <para xml:lang="de-DE">Typ zur Validierung</para>
        /// <para xml:lang="en">READONLY: minimal price pro qm</para>
        /// <para xml:lang="en">type for validation</para>
        /// <para xml:lang="en">Total number of digits: 15.</para>
        /// <para xml:lang="en">Total number of digits in fraction: 2.</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum exclusive value: 10000000000000.</para>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("minPriceProQm", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal MinPriceProQmValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die MinPriceProQm-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the MinPriceProQm property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool MinPriceProQmValueSpecified { get; set; }
        
        /// <summary>
        /// <para xml:lang="de-DE">READONLY: minimaler Quadratmeterprice</para>
        /// <para xml:lang="de-DE">Typ zur Validierung</para>
        /// <para xml:lang="en">READONLY: minimal price pro qm</para>
        /// <para xml:lang="en">type for validation</para>
        /// <para xml:lang="en">Total number of digits: 15.</para>
        /// <para xml:lang="en">Total number of digits in fraction: 2.</para>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum exclusive value: 10000000000000.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<decimal> MinPriceProQm
        {
            get
            {
                if (this.MinPriceProQmValueSpecified)
                {
                    return this.MinPriceProQmValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.MinPriceProQmValue = value.GetValueOrDefault();
                this.MinPriceProQmValueSpecified = value.HasValue;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 1.</para>
        /// <para xml:lang="en">Maximum inclusive value: 1000.</para>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("numberOfHousingUnit", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="int")]
        public int NumberOfHousingUnit { get; set; }
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("interiorQuality", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public IS24RestApi.Common.InteriorQuality InteriorQuality { get; set; }
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("freeFrom", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="string")]
        public string FreeFrom { get; set; }
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("address", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public IS24RestApi.Common.Address Address { get; set; }
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("relaEstateProjectEntries", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RealEstateProjectEntries RelaEstateProjectEntries { get; set; }
        
        /// <summary>
        /// <para xml:lang="de-DE">READONLY:Homepage des Projekets</para>
        /// <para xml:lang="en">READONLY:URL of the project page</para>
        /// <para xml:lang="en">Maximum length: 255.</para>
        /// <para xml:lang="en">Pattern: (http|https)://\w.*[.]\w.*.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.StringLengthAttribute(255)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(http|https)://\\w.*[.]\\w.*")]
        [System.Xml.Serialization.XmlElementAttribute("homepageUrl", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string HomepageUrl { get; set; }
        
        /// <summary>
        /// <para xml:lang="de-DE">Id des Elements.</para>
        /// <para xml:lang="en">Id of entity entry.</para>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlAttributeAttribute("id", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="long")]
        public long IdValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Id-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Id property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool IdValueSpecified { get; set; }
        
        /// <summary>
        /// <para xml:lang="de-DE">Id des Elements.</para>
        /// <para xml:lang="en">Id of entity entry.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<long> Id
        {
            get
            {
                if (this.IdValueSpecified)
                {
                    return this.IdValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.IdValue = value.GetValueOrDefault();
                this.IdValueSpecified = value.HasValue;
            }
        }
    }
    
    /// <summary>
    /// <para xml:lang="de-DE">Liste von Realestateprojectseinträgen</para>
    /// <para xml:lang="de-DE">Realestateprojekteintragsliste</para>
    /// <para xml:lang="en">List of Realestateproject-Entries</para>
    /// <para xml:lang="en">list of the real estate project entry</para>
    /// </summary>
    [System.SerializableAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "0.6.5640.32393")]
    [System.Xml.Serialization.XmlTypeAttribute("RealEstateProjectEntries", Namespace="http://rest.immobilienscout24.de/schema/offer/realestateproject/1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("realEstateProjectEntries", Namespace="http://rest.immobilienscout24.de/schema/offer/realestateproject/1.0")]
    public partial class RealEstateProjectEntries
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<RealEstateProjectEntry> _realEstateProjectEntry;
        
        /// <summary>
        /// <para xml:lang="de-DE">Eintrag eines RealEstateprojektes</para>
        /// <para xml:lang="en">an real estate project entry</para>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("realEstateProjectEntry", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.ObjectModel.Collection<RealEstateProjectEntry> RealEstateProjectEntry
        {
            get
            {
                return this._realEstateProjectEntry;
            }
        }
        
        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die RealEstateProjectEntry-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the RealEstateProjectEntry collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RealEstateProjectEntrySpecified
        {
            get
            {
                return (this.RealEstateProjectEntry.Count != 0);
            }
        }
        
        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="RealEstateProjectEntries" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="RealEstateProjectEntries" /> class.</para>
        /// </summary>
        public RealEstateProjectEntries()
        {
            this._realEstateProjectEntry = new System.Collections.ObjectModel.Collection<RealEstateProjectEntry>();
        }
        
        /// <summary>
        /// <para xml:lang="de-DE">Link zum Abruf der Eintragsliste.</para>
        /// <para xml:lang="en">Link to request the list of entries.</para>
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute("href", Namespace="http://www.w3.org/1999/xlink", DataType="anyURI")]
        public string Href { get; set; }
    }
    
    /// <summary>
    /// <para xml:lang="de-DE">Eintrag in Realestate-Project</para>
    /// <para xml:lang="de-DE">Realestateprojekteintrag</para>
    /// <para xml:lang="en">realestateproject entry.</para>
    /// <para xml:lang="en">real estate project entry</para>
    /// </summary>
    [System.SerializableAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "0.6.5640.32393")]
    [System.Xml.Serialization.XmlTypeAttribute("RealEstateProjectEntry", Namespace="http://rest.immobilienscout24.de/schema/offer/realestateproject/1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("realEstateProjectEntry", Namespace="http://rest.immobilienscout24.de/schema/offer/realestateproject/1.0")]
    public partial class RealEstateProjectEntry
    {
        
        /// <summary>
        /// <para xml:lang="de-DE">Id des Immobilienobjektes.</para>
        /// <para xml:lang="en">Id of realestate.</para>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("realEstateId", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="long")]
        public long RealEstateIdValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die RealEstateId-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the RealEstateId property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool RealEstateIdValueSpecified { get; set; }
        
        /// <summary>
        /// <para xml:lang="de-DE">Id des Immobilienobjektes.</para>
        /// <para xml:lang="en">Id of realestate.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<long> RealEstateId
        {
            get
            {
                if (this.RealEstateIdValueSpecified)
                {
                    return this.RealEstateIdValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.RealEstateIdValue = value.GetValueOrDefault();
                this.RealEstateIdValueSpecified = value.HasValue;
            }
        }
        
        /// <summary>
        /// <para xml:lang="de-DE">Id des Immobilienobjektes.</para>
        /// <para xml:lang="en">Id of realestate.</para>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("realEstateExternalId", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="string")]
        public string RealEstateExternalId { get; set; }
        
        /// <summary>
        /// <para xml:lang="de-DE">Id des Elements.</para>
        /// <para xml:lang="en">Id of entity entry.</para>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlAttributeAttribute("id", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="long")]
        public long IdValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Id-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Id property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool IdValueSpecified { get; set; }
        
        /// <summary>
        /// <para xml:lang="de-DE">Id des Elements.</para>
        /// <para xml:lang="en">Id of entity entry.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<long> Id
        {
            get
            {
                if (this.IdValueSpecified)
                {
                    return this.IdValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.IdValue = value.GetValueOrDefault();
                this.IdValueSpecified = value.HasValue;
            }
        }
    }
}
