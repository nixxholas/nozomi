namespace Nozomi.Infra.Auth.Services.QuotaClaim
{
    public interface IQuotaClaimService
    {
        void SetQuota(string userId, int quotaAmt);

        void AddUsage(string userId, int usageAmt = 1);

        void ResetUsage(string userId, int usageAmt = 0);
    }
}