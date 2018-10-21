using System;

namespace Nozomi.Data.NozomiRedisModels
{
    public class RCCachedDatumKey
    {
        public long Id { get; set; }
        
        public DateTime DatumTime { get; set; }
    }
}