using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.AreaModels.v1.RequestComponent
{
    public class RequestComponentDTO
    {
        public long Id { get; set; }

        public long ComponentType { get; set; }
        
        public string Identifier { get; set; }
        
        public string QueryComponent { get; set; }

        public bool IsDenominated { get; set; } = false;

        public bool AnomalyIgnorance { get; set; } = false;

        public string Value { get; set; }
    }
}