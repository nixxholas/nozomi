using System.Collections.Generic;
using System.Security.Claims;

namespace Nozomi.Base.Core.Interfaces
{
    public interface IUser
    {
        string UserName { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
    }
}