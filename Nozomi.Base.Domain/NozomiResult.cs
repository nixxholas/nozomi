
namespace Nozomi.Data
{
    public class NozomiResult<TEntity> where TEntity : class
    {
        // Manual Initialization
        public NozomiResult(){}

        // Data-driven Initialization
        public NozomiResult(TEntity data)
        {
            ResultType = (data == null) ? NozomiResultType.Failed : NozomiResultType.Success;
            Data = data;
        }

        // Result-driven Initialization
        public NozomiResult(NozomiResultType resultType, string msg)
        {
            ResultType = resultType;
            Message = msg;
        }
        
        public NozomiResult(NozomiResultType resultType, string msg, object item)
        {
            ResultType = resultType;
            Message = msg;
            Item = item;
        }

        public NozomiResultType ResultType { get; set; } = NozomiResultType.Unknown;
        
        public string Message { get; set; }
        
        public TEntity Data { get; set; }
        
        public object Item { get; set; }
    }
}