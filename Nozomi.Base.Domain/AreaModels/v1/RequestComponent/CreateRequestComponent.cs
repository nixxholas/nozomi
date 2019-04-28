using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.AreaModels.v1.RequestComponent
{
    public class CreateRequestComponent
    {
        public long RequestId { get; set; }

        public ComponentType ComponentType { get; set; } = ComponentType.Unknown;
        
        public string Identifier { get; set; }
        
        public string QueryComponent { get; set; }

        public bool IsDenominated { get; set; } = false;

        public bool AnomalyIgnorance { get; set; } = false;
    }
}