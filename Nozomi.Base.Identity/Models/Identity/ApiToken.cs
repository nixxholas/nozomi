using System;
using Nozomi.Base.Core;

namespace Nozomi.Base.Identity.Models.Identity
{
    public class ApiToken : BaseEntityModel
    {
        public Guid Guid { get; set; }
        
        public string Label { get; set; }
        
        public DateTime LastAccessed { get; set; }
        
        /// <summary>
        /// HMAC generated
        /// </summary>
        public string Secret { get; set; }
        
        /// <summary>
        /// RNG Generated
        /// </summary>
        public string Key { get; set; }
        
        public long UserId { get; set; }
        
        public User User { get; set; }
    }
}