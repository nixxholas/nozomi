using System;
using FluentValidation;

namespace Nozomi.Base.Auth.ViewModels.Account
{
    public class ResendEmailInputModel
    {
        public string ReturnUrl { get; set; }

        public bool IsValid()
        {
            var validator = new ResendEmailInputValidator().Validate((this));
            return validator.IsValid;
        }
        
        public class ResendEmailInputValidator : AbstractValidator<ResendEmailInputModel>
        {
            public ResendEmailInputValidator()
            {
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