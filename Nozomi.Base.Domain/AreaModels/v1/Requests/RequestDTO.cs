using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Nozomi.Data.AreaModels.v1.AnalysedComponent;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Data.AreaModels.v1.RequestProperty;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.AreaModels.v1.Requests
{
    public class RequestDTO
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        
        public RequestType RequestType { get; set; }
        
        public ResponseType ResponseType { get; set; }
        
        public string DataPath { get; set; }
        
        public int Delay { get; set; }
        
        public long FailureDelay { get; set; }
        
        public bool IsEnabled { get; set; }

        public ICollection<RequestComponentDTO> RequestComponents { get; set; }
        
        public ICollection<RequestPropertyDTO> RequestProperties { get; set; }
    }
}