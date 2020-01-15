using System;
using System.Data;
using FluentValidation;

namespace Nozomi.Base.Auth.ViewModels.Account
{
    public class RegisterInputModel
    {
        public string Username { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public bool IsValid()
        {
            var validator = new RegisterInputValidator().Validate(this);
            return validator.IsValid;
        }
        
        public class RegisterInputValidator : AbstractValidator<RegisterInputModel>
        {
            public RegisterInputValidator()
            {
                RuleFor(e => e.Username).NotNull().NotEmpty().MaximumLength(100);
                RuleFor(e => e.Email).EmailAddress();
                RuleFor(e => e.Password).NotNull().NotEmpty();
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