﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.Models.Categorisation
{
    [DataContract]
    public class CurrencyType : Entity
    {
        public CurrencyType() {}

        public CurrencyType(string typeShortForm, string name)
        {
            Guid = Guid.NewGuid();
            Name = name;
            TypeShortForm = typeShortForm;
        }

        public CurrencyType(long id, Guid guid, string name, 
            string typeShortForm)
        {
            Id = id;
            Guid = guid;
            Name = name;
            TypeShortForm = typeShortForm;
        }
        
        [Key]
        public long Id { get; set; }

        [DataMember]
        [MaxLength(12)]
        [Display(Name = "Abbreviation", Prompt = "Enter a short form for the name.",
            Description = "The abbreviated form of the currency type's name.")]
        public string TypeShortForm { get; set; }

        [DataMember]
        [Display(Name = "Name", Prompt = "Enter a name.",
            Description = "Name of the Currency Type.")]
        public string Name { get; set; }
        
        [DataMember]
        public Guid Guid { get; set; }
        
        public ICollection<AnalysedComponent> AnalysedComponents { get; set; }

        public ICollection<Currency> Currencies { get; set; }
        
        public ICollection<Request> Requests { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(TypeShortForm) && !string.IsNullOrEmpty(Name));
        }
    }
}
