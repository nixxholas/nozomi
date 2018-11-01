using System.Collections.Generic;

namespace Nozomi.Data
{
    public class NozomiResult<TEntity> where TEntity : class
    {
        public NozomiResult(){}

        public NozomiResult(TEntity data)
        {
            ResultType = (data == null) ? NozomiResultType.Failed : NozomiResultType.Success;
            Data = data;
        }
        
        public NozomiResultType ResultType { get; set; } = NozomiResultType.Unknown;
        
        public TEntity Data { get; set; }
    }
}