using System.Collections.Generic;

namespace Nozomi.Data
{
    public class NozomiResult<TEntity> where TEntity : class
    {
        public bool Success { get; set; } = false;
        
        public IEnumerable<TEntity> Data { get; set; }
    }
}