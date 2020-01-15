// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentValidation;

namespace Nozomi.Base.Auth.ViewModels.Account
{
    public class LoginInputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
        
        /// <summary>
        /// The user-claimed address
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// The raw hash of the user-signed message
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// The raw message
        /// </summary>
        public string Message { get; set; }

        public bool IsValid()
        {
            var validator = new LoginInputValidator();
            return validator.Validate(this).IsValid;
        }
        
        protected class LoginInputValidator : AbstractValidator<LoginInputModel>
        {
            public LoginInputValidator()
            {
                // Password authentication validation
                RuleFor(e => e.Username).NotNull().NotEmpty().MaximumLength(200)
                    .Unless(e => !string.IsNullOrEmpty(e.Signature) 
                                 && !string.IsNullOrEmpty(e.Address)
                                 && !string.IsNullOrEmpty(e.Message));
                RuleFor(e => e.Password).NotNull().NotEmpty()
                    .MinimumLength(4).MaximumLength(200)
                    .Unless(e => !string.IsNullOrEmpty(e.Signature) 
                                 && !string.IsNullOrEmpty(e.Address)
                                 && !string.IsNullOrEmpty(e.Message));
                // Web3 authentication validation
                RuleFor(e => e.Message).NotNull().NotEmpty().MaximumLength(500)
                    .Unless(e => !string.IsNullOrEmpty(e.Username) 
                                 && !string.IsNullOrEmpty(e.Password));
                RuleFor(e => e.Address).NotNull().NotEmpty().MaximumLength(200)
                    .Unless(e => !string.IsNullOrEmpty(e.Username) 
                                 && !string.IsNullOrEmpty(e.Password));
                RuleFor(e => e.Signature).NotNull().NotEmpty().MaximumLength(1000)
                    .Unless(e => !string.IsNullOrEmpty(e.Username) 
                                 && !string.IsNullOrEmpty(e.Password));

                RuleFor(e => e.ReturnUrl).NotNull().NotEmpty().MaximumLength(1000);
            }
        }
    }
}