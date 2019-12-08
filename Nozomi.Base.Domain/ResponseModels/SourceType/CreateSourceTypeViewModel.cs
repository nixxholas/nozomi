using FluentValidation;

namespace Nozomi.Data.ResponseModels.SourceType
{
    public class CreateSourceTypeViewModel
    {
        public string Abbreviation { get; set; }
        
        public string Name { get; set; }

        public bool IsValid()
        {
            var validator = new CreateSourceTypeValidator();
            return validator.Validate(this).IsValid;
        }
        
        public class CreateSourceTypeValidator : AbstractValidator<CreateSourceTypeViewModel>
        {
            public CreateSourceTypeValidator()
            {
                RuleFor(st => st.Abbreviation).NotNull().NotEmpty();
                RuleFor(st => st.Name).NotNull().NotEmpty();
            }
        }
    }
}