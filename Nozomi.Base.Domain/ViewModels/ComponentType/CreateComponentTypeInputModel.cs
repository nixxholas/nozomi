using System.Data;
using FluentValidation;

namespace Nozomi.Data.ViewModels.ComponentType
{
    public class CreateComponentTypeInputModel
    {
        public string Slug { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public bool IsValid()
        {
            return new CreateComponentTypeInputModelValidator()
                .Validate(this)
                .IsValid;
        }

        private class CreateComponentTypeInputModelValidator : AbstractValidator<CreateComponentTypeInputModel>
        {
            public CreateComponentTypeInputModelValidator()
            {
                RuleFor(e => e.Slug).NotNull().NotEmpty();
                RuleFor(e => e.Name).NotNull().NotEmpty();
                RuleFor(e => e.Description).MaximumLength(1000);
            }
        }
    }
}