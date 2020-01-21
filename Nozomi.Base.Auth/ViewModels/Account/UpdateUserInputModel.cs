using System.Collections.Generic;
using FluentValidation;

namespace Nozomi.Base.Auth.ViewModels.Account
{
    public class UpdateUserInputModel
    {
        public string PreviousPassword { get; set; }
        
        public string Password { get; set; }
        
        public ICollection<KeyValuePair<string, string>> UserClaims { get; set; }

        public bool IsValid()
        {
            var validator = new UpdateUserValidator().Validate(this);
            return validator.IsValid;
        }

        protected class UpdateUserValidator : AbstractValidator<UpdateUserInputModel>
        {
            public UpdateUserValidator()
            {
                RuleFor(e => e.Password).Must(PasswordsMatch)
                    .Unless(e => string.IsNullOrEmpty(e.PreviousPassword)
                                 || string.IsNullOrWhiteSpace(e.PreviousPassword));
            }
            
            private bool PasswordsMatch(UpdateUserInputModel instance, string password)
            {
                return instance.PreviousPassword.Equals(password);
            }
        }
    }
}