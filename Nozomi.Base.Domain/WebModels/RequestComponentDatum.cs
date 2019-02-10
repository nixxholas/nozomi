using System.Collections;
using System.Collections.Generic;
using Nozomi.Base.Core;

namespace Nozomi.Data.WebModels
{
    public class RequestComponentDatum : BaseEntityModel
    {
        public long Id { get; set; }
        
        public long RequestComponentId { get; set; }
        public RequestComponent RequestComponent { get; set; }

        public string Value { get; set; }

        public bool HasAbnormalValue(decimal val)
        {
            if (decimal.TryParse(Value, out var currVal))
            {
                // Always return true if the value has not been propagated yet.
                if (currVal.Equals(0)) return true;
                
                // If the difference is > 50% or if the difference is less than -50%
                return !((val / (currVal / 100)) - 100 > 50) || !((val / (currVal / 100)) - 100 < -50);
            }

            return false;
        }
        
        public ICollection<RcdHistoricItem> RcdHistoricItems { get; set; }
    }
}