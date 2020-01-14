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
                RuleFor(e => e.Type).IsInEnum();
                // RuleFor(e => e.Identifier); // No rule for identifier, it's not a requirement
                // RuleFor(e => e.QueryComponent); // No rule for query component, it's not a requirement
                RuleFor(e => e.IsDenominated).NotNull();
                RuleFor(e => e.AnomalyIgnorance).NotNull();
                RuleFor(e => e.StoreHistoricals).NotNull();
                RuleFor(e => e.RequestId).NotNull().NotEmpty();
            }
        }
    }
}