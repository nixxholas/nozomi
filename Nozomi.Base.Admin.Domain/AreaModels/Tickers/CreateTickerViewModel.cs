namespace Nozomi.Base.Admin.Domain.AreaModels.Tickers
{
    public class CreateTickerViewModel : CreateTickerInputModel
    {
        public CreateTickerViewModel(){}

        public CreateTickerViewModel(CreateTickerInputModel vm)
        {
            CurrencyPairType = vm.CurrencyPairType;
            RequestType = vm.RequestType;
            ResponseType = vm.ResponseType;
            DataPath = vm.DataPath;
            Delay = vm.Delay;
            CurrencySourceId = vm.CurrencySourceId;
            MainCurrencyTypeId = vm.MainCurrencyTypeId;
            MainCurrencyAbbrv = vm.MainCurrencyAbbrv;
            MainCurrencySlug = vm.MainCurrencySlug;
            MainCurrencyName = vm.MainCurrencyName;
            CounterCurrencyTypeId = vm.CounterCurrencyTypeId;
            CounterCurrencyAbbrv = vm.CounterCurrencyAbbrv;
            CounterCurrencySlug = vm.CounterCurrencySlug;
            CounterCurrencyName = vm.CounterCurrencyName;
            QueryComponents = vm.QueryComponents;
            RequestProperties = vm.RequestProperties;
        }
    }
}