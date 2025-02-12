using System.Collections.Generic;
using FluentValidation;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Data.ViewModels.RequestProperty;
using Nozomi.Data.ViewModels.WebsocketCommand;

namespace Nozomi.Data.ViewModels.Request
{
    public class UpdateRequestInputModel : RequestViewModel
    {
        public new bool? IsEnabled { get; set; }
        
        public new ICollection<UpdateComponentInputModel> Components { get; set; }
        
        public new ICollection<UpdateRequestPropertyInputModel> Properties { get; set; }
        
        public new ICollection<UpdateWebsocketCommandInputModel> WebsocketCommands { get; set; }
        
        public new bool IsValid()
        {
            var validator = new UpdateRequestValidator();
            return validator.Validate(this).IsValid;
        }

        protected class UpdateRequestValidator : AbstractValidator<UpdateRequestInputModel>
        {
            public UpdateRequestValidator()
            {
                RuleFor(r => r.Guid).NotEmpty().NotNull();
                // RuleFor(r => r.IsEnabled).NotNull();
                RuleFor(r => r.RequestType).IsInEnum();
                RuleFor(r => r.ResponseType).IsInEnum();
                RuleFor(r => r.Endpoint).NotEmpty();
                RuleFor(r => r.Delay).GreaterThan(-1);
                RuleFor(r => r.FailureDelay).GreaterThan(-1);
                RuleFor(r => r.ParentType).IsInEnum();
                // // Safetynet for Currencies
                // RuleFor(r => r.CurrencySlug).NotNull().NotEmpty()
                //     // Ignore the check if a currency pair or currency type is selected.
                //     .Unless(r => 
                //         !string.IsNullOrEmpty(r.CurrencyPairGuid) || !string.IsNullOrEmpty(r.CurrencyTypeGuid));
                // // Safetynet for Currency Pairs
                // RuleFor(r => r.CurrencyPairGuid).NotEmpty().NotNull()
                //     // Ignore the check if a currency or currency type is selected.
                //     .Unless(r => 
                //         !string.IsNullOrEmpty(r.CurrencyTypeGuid) || !string.IsNullOrEmpty(r.CurrencySlug));
                // // Safetynet for Currency types
                // RuleFor(r => r.CurrencyTypeGuid).NotEmpty().NotNull()
                //     .Unless(r =>
                //         // Ignore the check if a currency or currency pair is selected
                //         !string.IsNullOrEmpty(r.CurrencySlug) || !string.IsNullOrEmpty(r.CurrencyPairGuid));
            }
        }
    }
}