using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Nozomi.Base.Core.Interfaces;

namespace Nozomi.Base.Core.Models
{
    public class WebUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public WebUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string UserName => _accessor.HttpContext.User.Identity.Name;

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return  _accessor.HttpContext.User.Claims;
        }
    }
}