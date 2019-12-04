using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Nozomi.Base.Core;
using Nozomi.Data.AreaModels.v1.AnalysedComponent;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Data.AreaModels.v1.RequestProperty;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.Models.Web.Websocket;

namespace Nozomi.Data.Models.Web
{
    [DataContract]
    public class Request : Entity
    {
        public Request() {}

        public Request(Request r)
        {
            Id = r.Id;
            Guid = r.Guid;
            RequestType = r.RequestType;
            ResponseType = r.ResponseType;
            DataPath = r.DataPath;
            Delay = r.Delay;
            FailureDelay = r.FailureDelay;
            CurrencyId = r.CurrencyId;
            Currency = r.Currency;
            CurrencyPairId = r.CurrencyPairId;
            CurrencyPair = r.CurrencyPair;
            CurrencyTypeId = r.CurrencyTypeId;
            CurrencyType = r.CurrencyType;
            CreatedAt = r.CreatedAt;
            CreatedById = r.CreatedById;
            ModifiedAt = r.ModifiedAt;
            ModifiedById = r.ModifiedById;
            IsEnabled = r.IsEnabled;
            RequestComponents = r.RequestComponents;
            RequestProperties = r.RequestProperties;
            WebsocketCommands = r.WebsocketCommands;
        }

        /// <summary>
        /// Top to bottom constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="guid"></param>
        /// <param name="requestType"></param>
        /// <param name="responseType"></param>
        /// <param name="dataPath"></param>
        /// <param name="delay"></param>
        /// <param name="failureDelay"></param>
        /// <param name="currencyId"></param>
        /// <param name="currency"></param>
        /// <param name="currencyPairId"></param>
        /// <param name="currencyPair"></param>
        /// <param name="currencyTypeId"></param>
        /// <param name="currencyType"></param>
        /// <param name="createdAt"></param>
        /// <param name="createdById"></param>
        /// <param name="modifiedAt"></param>
        /// <param name="modifiedById"></param>
        /// <param name="isEnabled"></param>
        /// <param name="components"></param>
        /// <param name="requestProperties"></param>
        /// <param name="websocketCommands"></param>
        public Request(long id, Guid guid, RequestType requestType, ResponseType responseType, string dataPath,
            int delay, long failureDelay, long currencyId, Currency.Currency currency, long currencyPairId,
            CurrencyPair currencyPair, long currencyTypeId, CurrencyType currencyType, DateTime createdAt,
            string createdById, DateTime modifiedAt, string modifiedById, bool isEnabled,
            ICollection<Component> components, ICollection<RequestProperty> requestProperties,
            ICollection<WebsocketCommand> websocketCommands)
        {
            Id = id;
            Guid = guid;
            RequestType = requestType;
            ResponseType = responseType;
            DataPath = dataPath;
            Delay = delay;
            FailureDelay = failureDelay;
            CurrencyId = currencyId;
            Currency = currency;
            CurrencyPairId = currencyPairId;
            CurrencyPair = currencyPair;
            CurrencyTypeId = currencyTypeId;
            CurrencyType = currencyType;
            CreatedAt = createdAt;
            CreatedById = createdById;
            ModifiedAt = modifiedAt;
            ModifiedById = modifiedById;
            IsEnabled = isEnabled;
            RequestComponents = components;
            RequestProperties = requestProperties;
            WebsocketCommands = websocketCommands;
        }

        public Request(RequestType requestType, ResponseType responseType, string dataPath, int delay,
            long failureDelay)
        {
            RequestType = requestType;
            ResponseType = responseType;
            DataPath = dataPath;
            Delay = delay;
            FailureDelay = failureDelay;
        }
        
        public long Id { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public RequestType RequestType { get; set; }

        [DataMember]
        public ResponseType ResponseType { get; set; }

        /// <summary>
        /// URL.
        /// </summary>
        [DataMember]
        public string DataPath { get; set; }

        /// <summary>
        /// Defines the delay of repeating in milliseconds.
        /// </summary>
        [DataMember]
        public int Delay { get; set; }

        [DataMember]
        public long FailureDelay { get; set; }
        
        public long? CurrencyId { get; set; }
        
        public Currency.Currency Currency { get; set; }
        
        public long? CurrencyPairId { get; set; }
        
        public CurrencyPair CurrencyPair { get; set; }
        
        public long? CurrencyTypeId { get; set; }
        
        public CurrencyType CurrencyType { get; set; }

        public ICollection<Component> RequestComponents { get; set; }
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