using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.Models.Currency
{
    public class CurrencyType : BaseEntityModel
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(12)]
        public string TypeShortForm { get; set; }

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
