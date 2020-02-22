using System.Threading.Tasks;

namespace Nozomi.Infra.Auth.Services.QuotaClaims
{
    public interface IQuotaClaimsService
    {
        Task SetQuota(Base.Auth.Models.User user, int quotaAmt);

        Task AddUsage(Base.Auth.Models.User user, int usageAmt);

        Task RestUsage(Base.Auth.Models.User user, int usageAmt = 0);

    }
}