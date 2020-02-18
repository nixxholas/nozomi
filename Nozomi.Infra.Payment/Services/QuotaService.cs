using Nozomi.Infra.Payment.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nozomi.Infra.Payment.Services
{
    class QuotaService : IQuotaService
    {
        public Task DowngradeQuota()
        {
            throw new NotImplementedException();
        }

        public Task ResetQuota()
        {
            throw new NotImplementedException();
        }

        public Task UpgradeQuota()
        {
            throw new NotImplementedException();
        }
    }
}
