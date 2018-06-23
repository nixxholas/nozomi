using Counter.SDK.SharedModels;

namespace Nozomi.Data.WebModels.LoggingModels
{
    public class RequestLog : BaseEntityModel
    {
        public long Id { get; set; }
        
        public RequestLogType Type { get; set; }
        
        public string RawPayload { get; set; }
        
        // Bind to the request
        public long RequestId { get; set; }
        
        public Request Request { get; set; }
    }
}