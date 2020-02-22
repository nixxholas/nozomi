using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nozomi.Infra.Payment.Services.Interfaces
{
    public interface IQuotaService
    {
        Task ResetQuota();

        Task UpgradeQuota();

        Task DowngradeQuota();
    }
}
