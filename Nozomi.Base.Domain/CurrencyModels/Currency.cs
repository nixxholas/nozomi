using Counter.SDK.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nozomi.Data.CurrencyModels
{
    public class Currency : BaseEntityModel
    {
        [Key]
        public long Id { get; set; }

        public long CurrencyTypeId { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public string Abbrv { get; set; } // USD? MYR? IND?

        public string Name { get; set; }

        public long CurrencySourceId { get; set; }
        public Source CurrencySource { get; set; }

        // This will have a number if it is a crypto pair to peg to proper entities
        public long WalletTypeId { get; set; } = 0;

        public ICollection<PartialCurrencyPair> PartialCurrencyPairs { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Abbrv) && !String.IsNullOrEmpty(Name) && CurrencyTypeId > 0 && CurrencySourceId > 0;
        }
    }
}
