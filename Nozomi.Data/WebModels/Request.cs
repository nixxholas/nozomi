using Counter.SDK.SharedModels;
using System;
using System.Collections.Generic;
using System.Text;
using Nozomi.Data.WebModels.LoggingModels;

namespace Nozomi.Data.WebModels
{
    public class Request : BaseEntityModel
    {
        public long Id { get; set; }

        public Guid Guid { get; set; }

        public RequestType RequestType { get; set; }

        /// <summary>
        /// URL.
        /// </summary>
        public string DataPath { get; set; }

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
                && (RequestComponents != null) && RequestComponents.Count > 0
                && (RequestProperties != null) && RequestProperties.Count > 0; 
        }
    }
}
