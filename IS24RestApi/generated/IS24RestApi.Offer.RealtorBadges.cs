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
namespace IS24RestApi.Offer.RealtorBadges
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    
    
    /// <summary>
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "1.0.20.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("Badge", Namespace="http://rest.immobilienscout24.de/schema/offer/realtorbadges/1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("badge", Namespace="http://rest.immobilienscout24.de/schema/offer/realtorbadges/1.0")]
    public partial class Badge
    {
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("type", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="string")]
        public string Type { get; set; }
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("description", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="string")]
        public string Description { get; set; }
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("pictureUrl", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="string")]
        public string PictureUrl { get; set; }
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("active", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="boolean")]
        public bool Active { get; set; }
    }
    
    /// <summary>
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "1.0.20.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("RealtorBadges", Namespace="http://rest.immobilienscout24.de/schema/offer/realtorbadges/1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("realtorBadges", Namespace="http://rest.immobilienscout24.de/schema/offer/realtorbadges/1.0")]
    public partial class RealtorBadges
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<Badge> _badges;
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("badges", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("badge", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.ObjectModel.Collection<Badge> Badges
        {
            get
            {
                return this._badges;
            }
        }
        
        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="RealtorBadges" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="RealtorBadges" /> class.</para>
        /// </summary>
        public RealtorBadges()
        {
            this._badges = new System.Collections.ObjectModel.Collection<Badge>();
        }
    }
    
    /// <summary>
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "1.0.20.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("RealtorBadgesBadges", Namespace="http://rest.immobilienscout24.de/schema/offer/realtorbadges/1.0", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RealtorBadgesBadges
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<Badge> _badge;
        
        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("badge", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.ObjectModel.Collection<Badge> Badge
        {
            get
            {
                return this._badge;
            }
        }
        
        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Badge-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Badge collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BadgeSpecified
        {
            get
            {
                return (this.Badge.Count != 0);
            }
        }
        
        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="RealtorBadgesBadges" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="RealtorBadgesBadges" /> class.</para>
        /// </summary>
        public RealtorBadgesBadges()
        {
            this._badge = new System.Collections.ObjectModel.Collection<Badge>();
        }
    }
}
