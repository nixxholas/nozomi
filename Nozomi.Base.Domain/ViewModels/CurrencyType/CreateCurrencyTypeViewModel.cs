using FluentValidation;

namespace Nozomi.Data.ViewModels.CurrencyType
{
    public class CreateCurrencyTypeViewModel
    {        
        public string TypeShortForm { get; set; }
        
        public string Name { get; set; }

        public bool IsValid()
        {
            var validator = new CreateCurrencyTypeValidator();
            return validator.Validate(this).IsValid;
        }
        
        public class CreateCurrencyTypeValidator : AbstractValidator<CreateCurrencyTypeViewModel>
        {
            public CreateCurrencyTypeValidator()
            {
                RuleFor(st => st.TypeShortForm).NotNull().NotEmpty()
                    .Must(e => e.Length <= 12);
                RuleFor(st => st.Name).NotNull().NotEmpty();
            }
        }
    }
}