using System;
using Nozomi.Base.Core;

namespace Nozomi.Base.Identity.Models.Identity
{
    public class ApiToken : BaseEntityModel
    {
        public Guid Guid { get; set; }
        
        public string Label { get; set; }
        
        public DateTime LastAccessed { get; set; }
        
        public string PublicKey { get; set; }
        
        public string ApiKey { get; set; }
        
        public long UserId { get; set; }
        
        public User User { get; set; }
    }
}