using System;

namespace Nozomi.Base.Identity.AreaModels.v1.ApiToken
{
    public class ApiTokenResult
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        
        public string Key { get; set; }
        
        public string Label { get; set; }
        
        public DateTime LastAccessed { get; set; }

        public bool Deleted { get; set; } = false;
    }
}