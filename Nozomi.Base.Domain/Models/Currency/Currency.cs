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
    public class Currency : BaseEntityModel
    {
        public Currency () {}

        public Currency(ICollection<Currency> currencies)
        {
            if (currencies.Any())
            {
                var firstCurr = currencies.FirstOrDefault();

                if (firstCurr != null)
                {
                    // Doesn't matter...
                    Id = firstCurr.Id;
                    CurrencyTypeId = firstCurr.Id;
                    CurrencyType = firstCurr.CurrencyType;
                    Abbreviation = firstCurr.Abbreviation;
                    Name = firstCurr.Name;
                    WalletTypeId = firstCurr.WalletTypeId;
                    CurrencyPairs = CurrencyPairs;
                }
            }
        }
        
        [Key]
        public long Id { get; set; }

        public long CurrencyTypeId { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public string Abbreviation { get; set; } // USD? MYR? IND?

        public string Name { get; set; }
        
        public string Description { get; set; }

        public int Denominations { get; set; } = 0;
        
        public string DenominationName { get; set; }

        // This will have a number if it is a crypto pair to peg to proper entities
        public long WalletTypeId { get; set; } = 0;
        
        public ICollection<AnalysedComponent> AnalysedComponents { get; set; }
        
        public ICollection<CurrencyProperty> CurrencyProperties { get; set; }
        
        public ICollection<CurrencyPair> CurrencyPairs { get; set; }
        
        public ICollection<CurrencyRequest> CurrencyRequests { get; set; }
        
        public ICollection<CurrencySource> CurrencySources { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Abbreviation) && !String.IsNullOrEmpty(Name) && CurrencyTypeId > 0;
        }
    }
}
