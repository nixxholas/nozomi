using System.Threading.Tasks;

namespace Nozomi.Infra.Auth.Services.QuotaClaims
{
    public interface IQuotaClaimsService
    {
        Task SetQuota(Base.Auth.Models.User user);

        Task AddUsage(Base.Auth.Models.User user);
        
        Task RestUsage(Base.Auth.Models.User user);
        
    }
}