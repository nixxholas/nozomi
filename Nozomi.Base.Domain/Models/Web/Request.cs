using System;
using System.Collections.Generic;
using System.Linq;
using Nozomi.Base.Core;
using Nozomi.Data.AreaModels.v1.AnalysedComponent;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Data.AreaModels.v1.RequestProperty;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.Models.Web.Logging;

namespace Nozomi.Data.Models.Web
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
        
        public long FailureDelay { get; set; }

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

        public RequestDTO ToDTO()
        {
            return new RequestDTO
            {
                Guid = Guid,
                RequestType = RequestType,
                ResponseType = ResponseType,
                DataPath = DataPath,
                Delay = Delay,
                FailureDelay = FailureDelay,
//                AnalysedComponents = AnalysedComponents
//                    .Select(ac => new AnalysedComponentDTO
//                    {
//                        ComponentType = ac.ComponentType,
//                        Delay = ac.Delay,
//                        Id = ac.Id,
//                        IsDenominated = ac.IsDenominated,
//                        Value = ac.Value
//                    })
//                    .ToList(),
                RequestComponents = RequestComponents
                    .Select(rc => new RequestComponentDTO
                    {
                        AnomalyIgnorance = rc.AnomalyIgnorance,
                        ComponentType = rc.ComponentType,
                        Id = rc.Id,
                        Identifier = rc.Identifier,
                        IsDenominated = rc.IsDenominated,
                        QueryComponent = rc.QueryComponent,
                        Value = rc.Value
                    })
                    .ToList(),
                RequestProperties = RequestProperties
                    .Select(rp => new RequestPropertyDTO
                    {
                        Id = rp.Id,
                        Key = rp.Key,
                        RequestPropertyType = rp.RequestPropertyType,
                        Value = rp.Value
                    })
                    .ToList()
            };
        }
    }
}
