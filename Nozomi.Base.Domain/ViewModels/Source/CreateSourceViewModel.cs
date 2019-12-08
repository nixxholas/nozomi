using FluentValidation;

namespace Nozomi.Data.ViewModels.Source
{
    public class CreateSourceViewModel
    {
        public string Abbreviation { get; set; }
        
        public string Name { get; set; }
        
        public string ApiDocsUrl { get; set; }
        
        public string SourceType { get; set; }

        public bool IsValid()
        {
            var validator = new CreateSourceValidator();
            return validator.Validate(this).IsValid;
        }
        
        public class CreateSourceValidator : AbstractValidator<CreateSourceViewModel>
        {
            public CreateSourceValidator()
            {
                RuleFor(e => e.Abbreviation).NotNull().NotEmpty();
                RuleFor(e => e.Name).NotNull().NotEmpty();
                RuleFor(e => e.SourceType).NotNull().NotEmpty();
            }
        }
    }
}