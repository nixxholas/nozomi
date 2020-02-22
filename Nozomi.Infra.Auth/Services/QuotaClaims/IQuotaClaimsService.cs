using System.Threading.Tasks;

namespace Nozomi.Infra.Auth.Services.QuotaClaims
{
    public interface IQuotaClaimsService
    {
        bool SetQuota(Base.Auth.Models.User user, int quotaAmt);

        bool AddUsage(Base.Auth.Models.User user, int usageAmt);

        bool RestUsage(Base.Auth.Models.User user, int usageAmt = 0);

    }
}