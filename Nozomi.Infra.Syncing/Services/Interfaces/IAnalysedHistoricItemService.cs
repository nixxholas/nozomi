using System;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Infra.Syncing.Services.Interfaces
{
    public interface IAnalysedHistoricItemService
    {
        long Create(AnalysedHistoricItem item, string userId = null);

        bool Push(long analysedComponentId, string incomingValue, DateTime historicalTime, 
            bool ignoreSimilarityCheck = false, string userId = null);
    }
}