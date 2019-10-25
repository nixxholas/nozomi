using System;
using Nozomi.Data.Models.Analytical;

namespace Nozomi.Infra.Analysis.Service.Services.Interfaces
{
    public interface IAnalysedHistoricItemService
    {
        long Create(AnalysedHistoricItem item, long userId = 0);

        bool Push(long analysedComponentId, string incomingValue, DateTime historicalTime, 
            bool ignoreSimilarityCheck = false, long userId = 0);
    }
}