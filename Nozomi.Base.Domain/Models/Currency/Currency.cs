using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.Models.Currency
{
    public class Currency : Entity
    {
        [Key]
        public long Id { get; set; }

        public long CurrencyTypeId { get; set; }
        public CurrencyType CurrencyType { get; set; }
        
        public string LogoPath { get; set; }

        public string Abbreviation { get; set; } // USD? MYR? IND?
        
        public string Slug { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public int Denominations { get; set; } = 0;
        
        public string DenominationName { get; set; }
        
        public ICollection<AnalysedComponent> AnalysedComponents { get; set; }
        
        public ICollection<CurrencyProperty> CurrencyProperties { get; set; }
        
        public ICollection<CurrencySource> CurrencySources { get; set; }
        
        public ICollection<Request> Requests { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Abbreviation) && !String.IsNullOrEmpty(Name) && CurrencyTypeId > 0;
        }
    }
}
