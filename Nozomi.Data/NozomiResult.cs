using System.Collections.Generic;

namespace Nozomi.Data
{
    public class NozomiResult<TEntity> where TEntity : class
    {
        public NozomiResultType ResultType { get; set; } = NozomiResultType.Unknown;
        
        public TEntity Data { get; set; }
    }
}