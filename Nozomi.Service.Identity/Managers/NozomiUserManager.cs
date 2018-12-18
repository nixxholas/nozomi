using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Service.Identity.Managers.Interfaces;
using Nozomi.Service.Identity.Stores.Interfaces;

namespace Nozomi.Service.Identity.Managers
{
    public class NozomiUserManager : UserManager<User>, INozomiUserManager
    {
        private new readonly INozomiUserStore Store;
        
        public NozomiUserManager(INozomiUserStore store, IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, 
                errors, services, logger)
        {
        }

        public async Task<User> FindAsync(string id, string password)
        {
            var user = await FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            return await CheckPasswordAsync(user, password) ? user : null;
        }

        public Task ForceConfirmEmail(long userId)
        {
            return Store.ForceConfirmEmailAsync(userId);
        }

        public Task ForceConfirmEmail(User user)
        {
            return Store.ForceConfirmEmailAsync(user);
        }
    }
}