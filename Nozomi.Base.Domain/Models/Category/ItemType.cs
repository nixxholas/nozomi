using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.Models.Category
{
    public class ItemType : Entity
    {
        [Key]
        public Guid Guid { get; set; }

        [DataMember]
        [MaxLength(12)]
        [Display(Name = "Slug", Prompt = "Enter a short form for the type.",
            Description = "The abbreviated form of the type's name.")]
        public string Slug { get; set; }

        [DataMember]
        [Display(Name = "Name", Prompt = "Enter a name.",
            Description = "Name of the Currency Type.")]
        public string Name { get; set; }
        
        public ICollection<AnalysedComponent> AnalysedComponents { get; set; }

        public ICollection<Item> Items { get; set; }
        
        public ICollection<Request> Requests { get; set; }
    }
}