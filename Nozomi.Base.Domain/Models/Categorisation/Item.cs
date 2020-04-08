using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.Models.Categorisation
{
    [DataContract]
    public class Item : Entity
    {
        public Item() {}

        public Item(long itemTypeId, string logoPath, string abbreviation, string slug, string name,
            string description, int denominations, string denominationName)
        {
            Guid = Guid.NewGuid();
            ItemTypeId = itemTypeId;
            LogoPath = logoPath;
            Abbreviation = abbreviation;
            Slug = slug;
            Name = name;
            Description = description;
            Denominations = denominations;
            DenominationName = denominationName;
        }
        
        public Item(long id, long itemTypeId, string logoPath, string abbreviation, string slug, string name,
            string description, int denominations, string denominationName)
        {
            Id = id;
            ItemTypeId = itemTypeId;
            LogoPath = logoPath;
            Abbreviation = abbreviation;
            Slug = slug;
            Name = name;
            Description = description;
            Denominations = denominations;
            DenominationName = denominationName;
        }
        
        [Key]
        public long Id { get; set; }
        
        public Guid Guid { get; set; }

        public long ItemTypeId { get; set; }
        public CurrencyType ItemType { get; set; }
        
        public string LogoPath { get; set; }

        [DataMember]
        public string Abbreviation { get; set; } // USD? MYR? IND?
        
        [DataMember]
        public string Slug { get; set; }

        [DataMember]
        public string Name { get; set; }
        
        public string Description { get; set; }

        [DataMember]
        public int Denominations { get; set; } = 0;
        
        [DataMember]
        public string DenominationName { get; set; }
        
        public ICollection<AnalysedComponent> AnalysedComponents { get; set; }
        
        public ICollection<CurrencyProperty> ItemProperties { get; set; }
        
        public ICollection<CurrencySource> ItemSources { get; set; }
        
        public ICollection<Request> Requests { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Abbreviation) && !String.IsNullOrEmpty(Name) && ItemTypeId > 0;
        }
    }
}
