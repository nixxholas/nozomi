using System.Threading.Tasks;

namespace Nozomi.Infra.Auth.Services.QuotaClaims
{
    public interface IQuotaClaimsService
    {
        void SetQuota(string userId, int quotaAmt);

        void AddUsage(string userId, int usageAmt = 1);

        void ResetUsage(string userId, int usageAmt = 0);
    }
}