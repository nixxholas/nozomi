using FluentValidation;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.ViewModels.Component
{
    public class CreateComponentInputModel
    {
        public long? ComponentTypeId { get; set; }

        public string Identifier { get; set; }
        
        public string QueryComponent { get; set; }
        
        public bool IsDenominated { get; set; }
        
        public bool AnomalyIgnorance { get; set; }
        
        public bool StoreHistoricals { get; set; }
        
        public string RequestId { get; set; }

        public bool IsValid()
        {
            var validator = new CreateComponentValidator();
            return validator.Validate(this).IsValid;
        }
        
        protected class CreateComponentValidator : AbstractValidator<CreateComponentInputModel>
        {
            public CreateComponentValidator()
            {
                // RuleFor(e => e.ComponentTypeId).GreaterThan(-1);
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