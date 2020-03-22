using System;

namespace Nozomi.Base.Auth.ViewModels.ApiKey
{
    public class ApiKeyViewModel
    {
        public Guid Guid { get; set; }
        
        public string Label { get; set; }
        
        public string ApiKeyMasked { get; set; }
    }
}