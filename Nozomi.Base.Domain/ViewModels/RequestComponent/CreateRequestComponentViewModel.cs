using System.Data;
using FluentValidation;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.ViewModels.RequestComponent
{
    public class CreateRequestComponentViewModel
    {
        public ComponentType Type { get; set; }
    
        public string Identifier { get; set; }
        
        public string QueryComponent { get; set; }
        
        public bool IsDenominated { get; set; }
        
        public bool AnomalyIgnorance { get; set; }
        
        public bool StoreHistoricals { get; set; }
        
        public string RequestId { get; set; }

        public bool IsValid()
        {
            var validator = new CreateRequestComponentValidator();
            return validator.Validate(this).IsValid;
        }
        
        protected class CreateRequestComponentValidator : AbstractValidator<CreateRequestComponentViewModel>
        {
            public CreateRequestComponentValidator()
            {
                RuleFor(e => e.Type).IsInEnum();
                // RuleFor(e => e.Identifier); // No rule for identifier, it's not a requirement
                // RuleFor(e => e.QueryComponent); // No rule for query component, it's not a requirement
                RuleFor(e => e.IsDenominated).NotNull();
                RuleFor(e => e.AnomalyIgnorance).NotNull();
                RuleFor(e => e.StoreHistoricals).NotNull();
                RuleFor(e => e.RequestId).NotNull().NotEmpty();
            }
        }
    }
}