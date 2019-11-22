using System.Collections.Generic;
using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Currency
{
    /// <summary>
    /// The best way to peg currencies to sources.
    /// </summary>
    public class CurrencySource : Entity
    {
        public CurrencySource() {}

        /// <summary>
        /// Constructor for Currency-based seeding
        /// </summary>
        /// <param name="sourceId"></param>
        public CurrencySource(long sourceId)
        {
            SourceId = sourceId;
        }
        
        public long Id { get; set; }
        
        public long CurrencyId { get; set; }
        
        public Currency Currency { get; set; }
        
        public long SourceId { get; set; }
        
        public Source Source { get; set; }
    }
}