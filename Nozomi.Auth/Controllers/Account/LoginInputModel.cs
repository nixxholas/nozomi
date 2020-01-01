// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace Nozomi.Auth.Controllers.Account
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
    }
}