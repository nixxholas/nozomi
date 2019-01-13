using System;
using Nozomi.Base.Core;
using Nozomi.Base.Identity.AreaModels.v1.ApiToken;

namespace Nozomi.Base.Identity.ViewModels.Identity
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

        public ApiTokenResult ToApiTokenResult(bool returnSecret = false)
        {
            return new ApiTokenResult
            {
                Id = Guid.ToString(),
                Secret = returnSecret ? Secret : null,
                Key = Key,
                Label = Label,
                LastAccessed = ModifiedAt,
                Deleted = (DeletedAt == null)
            };
        }
    }
}