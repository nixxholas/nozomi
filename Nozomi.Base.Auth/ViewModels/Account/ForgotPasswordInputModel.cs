using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Auth.ViewModels.Account
{
    public class ForgotPasswordInputModel
    {
        [Required] [EmailAddress] public string Email { get; set; }
        public string ReturnUrl { get; set; }

        public bool IsValid()
        {
            var validator = new ForgotPasswordInputValidator().Validate(this);
            return validator.IsValid;
        }

        public class ForgotPasswordInputValidator : AbstractValidator<ForgotPasswordInputModel>
        {
            public ForgotPasswordInputValidator()
            {
                RuleFor(e => e.Email).NotEmpty().NotNull().EmailAddress();
                RuleFor(e => e.ReturnUrl).NotEmpty().NotNull()
                    .Custom((url, context) =>
                    {
                        if (!string.IsNullOrEmpty(url) && !Uri.TryCreate(url, UriKind.Absolute, out var uriResult) 
                                                       && uriResult.Scheme == Uri.UriSchemeHttp)
                        {
                            context.AddFailure("Invalid URL.");
                        }
                    });
            }
        }
    }
}