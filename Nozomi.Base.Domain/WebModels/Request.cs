using System;
using System.Collections.Generic;
using System.Text;
using Nozomi.Base.Core;
using Nozomi.Data.WebModels.LoggingModels;

namespace Nozomi.Data.WebModels
{
    public class Request : BaseEntityModel
    {
        public long Id { get; set; }

        public Guid Guid { get; set; }

        public RequestType RequestType { get; set; }
        
        public ResponseType ResponseType { get; set; }

        /// <summary>
        /// URL.
        /// </summary>
        public string DataPath { get; set; }
        
        /// <summary>
        /// Defines the delay of repeating in milliseconds.
        /// </summary>
        public int Delay { get; set; }

        public ICollection<RequestComponent> RequestComponents { get; set; }
        public ICollection<RequestLog> RequestLogs { get; set; }
        public ICollection<RequestProperty> RequestProperties { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(DataPath) && !string.IsNullOrWhiteSpace(DataPath)
                    && RequestType >= 0);
        }

        public bool IsValidForPolling()
        {
            return (!string.IsNullOrEmpty(DataPath) && !string.IsNullOrWhiteSpace(DataPath)
                                                    && RequestType >= 0)
                && (RequestComponents != null) && RequestComponents.Count > 0; 
        }
    }
}
