using System;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.AreaModels.v1.Requests
{
    public class RequestDTO
    {
        public Guid Guid { get; set; }
        
        public RequestType RequestType { get; set; }
        
        public ResponseType ResponseType { get; set; }
        
        public string DataPath { get; set; }
        
        public int Delay { get; set; }
        
        public long FailureDelay { get; set; }
    }
}