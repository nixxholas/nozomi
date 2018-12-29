namespace Nozomi.Data.AreaModels.v1.CurrencyPairComponent
{
    public class UpdateCurrencyPairComponent : CreateCurrencyPairComponent
    {
        public long Id { get; set; }
        
        public bool ToBeDeleted()
        {
            return RequestId.Equals(0);
        }
    }
}