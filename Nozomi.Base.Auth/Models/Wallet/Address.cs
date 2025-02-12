namespace Nozomi.Base.Auth.Models.Wallet
{
    public class Address
    {
        public string Id { get; set; }
        
        public string Hash { get; set; }
        
        public AddressType Type { get; set; }
        
        public string UserId { get; set; }
        
        public User User { get; set; }
    }
}