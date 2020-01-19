using FluentValidation;

namespace Nozomi.Base.Auth.ViewModels.Account
{
    public class RetryEmailConfirmationInputModel
    {
        public string Email { get; set; }
        
        public class RetryEmailConfirmationValidator : AbstractValidator<RetryEmailConfirmationInputModel>
        {
            public RetryEmailConfirmationValidator()
            {
                RuleFor(e => e.Email).NotEmpty().NotNull();
            }
        }
    }
}