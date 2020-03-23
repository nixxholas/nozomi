using System;

namespace Nozomi.Base.Auth.Models
{
    public class ApiKey
    {
        public Guid Guid { get; set; }
        
        public string Label { get; set; }
        
        public string Value { get; set; }
        
        public bool Revealed { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
        
        public User User { get; set; }
    }
}