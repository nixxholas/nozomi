using FluentValidation;

namespace Nozomi.Data.ViewModels.CurrencySource
{
    public class CreateCurrencySourceViewModel
    {
        public string SourceGuid { get; set; }
        
        public string CurrencySlug { get; set; }

        public bool IsValid()
        {
            var validator = new CreateCurrencySourceValidator();
            return validator.Validate(this).IsValid;
        }
        
        public class CreateCurrencySourceValidator : AbstractValidator<CreateCurrencySourceViewModel>
        {
            public CreateCurrencySourceValidator()
            {
                RuleFor(e => e.SourceGuid).NotNull().NotEmpty();
                RuleFor(e => e.CurrencySlug).NotNull().NotEmpty();
            }
        }
    }
}