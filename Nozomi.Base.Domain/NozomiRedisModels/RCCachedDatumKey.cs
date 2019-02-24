using System;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.NozomiRedisModels
{
    public class RCCachedDatumKey
    {
        public long RequestId { get; set; }
        
        public ComponentType ComponentType { get; set; }
    }
}