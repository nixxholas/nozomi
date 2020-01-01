using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.Models.Web
{
    [DataContract]
    public class Component : Entity
    {
        // Default Constructor
        public Component() {}
        
        // Hard creation constructor
        public Component(ComponentType componentType, string identifier, string queryComponent, bool anomalyIgnorance,
            bool isDenominated, bool storeHistoricals)
        {
            ComponentType = componentType;
            Identifier = identifier;
            QueryComponent = queryComponent;
            AnomalyIgnorance = anomalyIgnorance;
            IsDenominated = isDenominated;
            StoreHistoricals = storeHistoricals;
        }

        /// <summary>
        /// Fresh creation constructor
        /// </summary>
        /// <param name="componentType"></param>
        /// <param name="identifier"></param>
        /// <param name="queryComponent"></param>
        /// <param name="anomalyIgnorance"></param>
        /// <param name="isDenominated"></param>
        /// <param name="storeHistoricals"></param>
        /// <param name="requestId"></param>
        public Component(ComponentType componentType, string identifier, string queryComponent,
            bool anomalyIgnorance, bool isDenominated, bool storeHistoricals, long requestId)
        {
            ComponentType = componentType;
            Identifier = identifier;
            QueryComponent = queryComponent;
            AnomalyIgnorance = anomalyIgnorance;
            IsDenominated = isDenominated;
            StoreHistoricals = storeHistoricals;
            RequestId = requestId;
        }

        public Component(Component component, int historicIndex, int historicItemAmount)
        {
            if (component != null)
            {
                Id = component.Id;
                ComponentType = component.ComponentType;
                IsEnabled = component.IsEnabled;
                CreatedAt = component.CreatedAt;
                ModifiedAt = component.ModifiedAt;
                DeletedAt = component.DeletedAt;
                CreatedById = component.CreatedById;
                ModifiedById = component.ModifiedById;
                DeletedById = component.DeletedById;
                Identifier = component.Identifier;
                QueryComponent = component.QueryComponent;
                IsDenominated = component.IsDenominated;
                AnomalyIgnorance = component.AnomalyIgnorance;
                StoreHistoricals = component.StoreHistoricals;
                Value = component.Value;
                RequestId = component.RequestId;
                Request = component.Request;
            
                if (component.RcdHistoricItems != null)
                    RcdHistoricItems = component.RcdHistoricItems
                        .Where(rhi => rhi.DeletedAt == null)
                        .OrderBy(rhi => rhi.HistoricDateTime)
                        .Skip(historicIndex * historicItemAmount)
                        .Take(historicItemAmount)
                        .ToList();
            }
        }
        
        public long Id { get; set; }
        
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public ComponentType ComponentType { get; set; } = ComponentType.Unknown;
        
        /// <summary>
        /// This is another QueryComponent, used for traversing to the object/array in question.
        /// Once the object/array has been traversed to, the object will reverse by one tier for the
        /// QueryComponent will take over.
        ///
        /// Identifier = data/s/ethbtc
        /// i.e. data/
        /// </summary>
        [DataMember]
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the query component of the JSON data retrieved from the APIUrl.
        /// </summary>
        /// <value>The query component.</value>
        /// 
        /// Example: MainNest.Nest2.property3
        /// 
        /// If the component is actually an array, just toss a number. We'll attempt
        /// to pass thing parameter to an integer first anyway, so as to be able
        /// to distinguish if we're offloading data from an array or object.
        [DataMember]
        public string QueryComponent { get; set; }

        public bool IsDenominated { get; set; } = false;

        public bool AnomalyIgnorance { get; set; } = false;
        
        public bool StoreHistoricals { get; set; }

        [DataMember]
        public string Value { get; set; }

        public long RequestId { get; set; }
        public Request Request { get; set; }
        
        public ICollection<ComponentHistoricItem> RcdHistoricItems { get; set; }
        
        /// <summary>
        /// Does it carry an abnormal value?
        /// </summary>
        /// <param name="val"></param>
        /// <returns>true if the value is abnormal, false if not.</returns>
        public bool HasAbnormalNumericalValue(decimal val)
        {
            if (AnomalyIgnorance || string.IsNullOrEmpty(Value))
                return false;
        
            // Make sure the current value in the db is parse-able
            if (!decimal.TryParse(Value, out var currVal)) return true;
            
            // Return true if they match, 
            return currVal.Equals(val);
        }
    }
}
