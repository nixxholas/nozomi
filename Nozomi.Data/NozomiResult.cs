using System.Collections.Generic;

namespace Nozomi.Data
{
    public class NozomiResult<TEntity> where TEntity : class
    {
        public bool Success { get; set; } = false;

        public NozomiResultType ResultType { get; set; } = NozomiResultType.Unknown;
        
        public TEntity Data { get; set; }
    }
}