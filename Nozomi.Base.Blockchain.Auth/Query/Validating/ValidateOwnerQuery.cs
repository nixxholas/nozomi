namespace Nozomi.Base.Blockchain.Auth.Query.Validating
{
    public class ValidateOwnerQuery
    {
        public string ClaimerAddress { get; set; }
        
        public string Signature { get; set; }
        
        public string RawMessage { get; set; }
    }
}