using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nozomi.Base.Core;
using Nozomi.Base.Core.Models;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.Models.Currency
{
    public class CurrencyType : Entity
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(12)]
        [Display(Name = "Abbreviation", Prompt = "Enter a short form for the name.",
            Description = "The abbreviated form of the currency type's name.")]
        public string TypeShortForm { get; set; }

        [Display(Name = "Name", Prompt = "Enter a name.",
            Description = "Name of the Currency Type.")]
        public string Name { get; set; }
        
        public ICollection<AnalysedComponent> AnalysedComponents { get; set; }

        public ICollection<Currency> Currencies { get; set; }
        
        public ICollection<Request> Requests { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(TypeShortForm) && !string.IsNullOrEmpty(Name));
        }
    }
}
