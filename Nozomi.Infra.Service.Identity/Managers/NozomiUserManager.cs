using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Service.Identity.Managers.Interfaces;
using Nozomi.Service.Identity.Services.Interfaces;
using Nozomi.Service.Identity.Stores;
using Nozomi.Service.Identity.Stores.Interfaces;
using Stripe;

namespace Nozomi.Service.Identity.Managers
{
    public class NozomiUserManager : UserManager<User>, INozomiUserManager
    {
        public NozomiUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, 
                errors, services, logger)
        {
        }

        public override async Task<IdentityResult> CreateAsync(User user)
        {
            var createUserResult = await base.CreateAsync(user);
            if (createUserResult.Succeeded)
                return createUserResult;
            
            return IdentityResult.Failed(new IdentityError
            {
                Code = IdentityErrorType.InvalidCredentials.ToString(),
                Description = IdentityErrorType.InvalidCredentials.GetDescription()
            });
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

        /// <summary>
        /// Gets the user identifier for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose identifier should be retrieved.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the identifier for the specified <paramref name="user" />.</returns>
        public override async Task<string> GetUserIdAsync(User user)
        {
            ThrowIfDisposed();
            return await Store.GetUserIdAsync(user, CancellationToken);
        }
    }
}