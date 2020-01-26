using System.Collections.Generic;
using System.Text.Json;
using FluentValidation;
using Nozomi.Base.BCL;

namespace Nozomi.Base.Auth.ViewModels.Account
{
    public class UpdateUserInputModel
    {
        public string PreviousPassword { get; set; }
        
        public string Password { get; set; }
        
        public IDictionary<string, JsonElement> UserClaims { get; set; }

        public bool IsValid()
        {
            var validator = new UpdateUserValidator().Validate(this);
            return validator.IsValid;
        }

        protected class UpdateUserValidator : AbstractValidator<UpdateUserInputModel>
        {
            public UpdateUserValidator()
            {
                RuleFor(e => e.Password).NotEmpty().NotNull()
                    .Unless(e => string.IsNullOrEmpty(e.PreviousPassword));
                RuleFor(e => e.PreviousPassword).NotEmpty().NotNull()
                    .Unless(e => string.IsNullOrEmpty(e.Password));
                // RuleFor(e => e.Password).Must(PasswordsMatch)
                //     .Unless(e => string.IsNullOrEmpty(e.PreviousPassword)
                //                  || string.IsNullOrWhiteSpace(e.PreviousPassword));

                RuleFor(e => e.UserClaims).NotNull();
            }
            
            // private bool PasswordsMatch(UpdateUserInputModel instance, string password)
            // {
            //     return instance.PreviousPassword.Equals(password);
            // }
        }
    }
}