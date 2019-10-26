using System;
using System.Collections.Generic;
using System.Linq;
using Nozomi.Base.Core.Models;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Data.AreaModels.v1.RequestProperty;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Websocket;
using Swashbuckle.AspNetCore.Swagger;

namespace Nozomi.Data.Models
{
    public class Request : Entity
    {
        // Empty constructor for EF
        public Request() {}

        public Request(RequestType type, ResponseType responseType, string dataPath, int delay, int failureDelay,
            long currencyId, long currencyPairId, long currencyTypeId)
        {
            RequestType = type;
            ResponseType = responseType;
            DataPath = dataPath;
            Delay = delay;
            FailureDelay = failureDelay;
            CurrencyId = currencyId;
            CurrencyPairId = currencyPairId;
            CurrencyTypeId = currencyTypeId;
        }
        
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
        
        public long? CurrencyId { get; set; }
        
        public Currency.Currency Currency { get; set; }
        
        public long? CurrencyPairId { get; set; }
        
        public CurrencyPair CurrencyPair { get; set; }
        
        public long? CurrencyTypeId { get; set; }
        
        public CurrencyType CurrencyType { get; set; }

        public ICollection<RequestComponent> RequestComponents { get; set; }
        public ICollection<RequestProperty> RequestProperties { get; set; }
        
        // Websocket-based Entities
        
        public ICollection<WebsocketCommand> WebsocketCommands { get; set; }

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
                Id = Id,
                Guid = Guid,
                RequestType = RequestType,
                ResponseType = ResponseType,
                DataPath = DataPath,
                Delay = Delay,
                FailureDelay = FailureDelay,
                IsEnabled = IsEnabled,
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
                    .Where(rc => rc.DeletedAt == null)
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
                    .Where(rp => rp.DeletedAt == null)
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