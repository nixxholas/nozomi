using Nozomi.Data.Models;
using Nozomi.Data.Validations;

namespace Nozomi.Data.Commands
{
    public class CreateRequestCommand : RequestCommand
    {
        public CreateRequestCommand(RequestType requestType, ResponseType responseType, string dataPath, int delay,
            int failureDelay, long currencyId, long currencyPairId, long currencyTypeId)
        {
            RequestType = requestType;
            ResponseType = responseType;
            DataPath = dataPath;
            Delay = delay;
            FailureDelay = failureDelay;
            CurrencyId = currencyId;
            CurrencyPairId = currencyPairId;
            CurrencyTypeId = currencyTypeId;
        }
        
        public override bool IsValid()
        {
            ValidationResult = new CreateRequestValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}