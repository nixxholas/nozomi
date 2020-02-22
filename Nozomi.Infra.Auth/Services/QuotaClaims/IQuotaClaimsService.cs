using System.Threading.Tasks;

namespace Nozomi.Infra.Auth.Services.QuotaClaims
{
    public interface IQuotaClaimsService
    {
        void SetQuota(Base.Auth.Models.User user, int quotaAmt);

        void AddUsage(Base.Auth.Models.User user, int usageAmt = 1);

        void RestUsage(Base.Auth.Models.User user, int usageAmt = 0);
    }
}