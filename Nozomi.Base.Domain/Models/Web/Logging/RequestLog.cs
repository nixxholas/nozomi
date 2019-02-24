using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Web.Logging
{
    public class RequestLog : BaseEntityModel
    {
        public long Id { get; set; }

        public RequestLogType Type { get; set; } = RequestLogType.Unknown;
        
        public string RawPayload { get; set; }
        
        // Bind to the request
        public long RequestId { get; set; }
        
        public Request Request { get; set; }
    }
}