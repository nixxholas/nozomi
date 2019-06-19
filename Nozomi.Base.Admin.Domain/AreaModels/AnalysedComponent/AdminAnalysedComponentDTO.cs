using System;

namespace Nozomi.Data.AreaModels.v1.AnalysedComponent
{
    public class AdminAnalysedComponentDTO : AnalysedComponentDTO
    {
        public bool IsEnabled { get; set; }
        
        public long? DeletedBy { get; set; }
        
        public DateTime? DeletedAt { get; set; }
    }
}