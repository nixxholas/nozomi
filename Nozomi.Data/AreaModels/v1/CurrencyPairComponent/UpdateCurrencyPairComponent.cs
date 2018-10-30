namespace Nozomi.Data.AreaModels.v1.CurrencyPairComponent
{
    public class UpdateCurrencyPairComponent : CreateCurrencyPairComponent
    {
        public bool ToBeDeleted()
        {
            return RequestId.Equals(0);
        }
    }
}