using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Analysis.Interfaces;

namespace Nozomi.Service.Events.Analysis
{
    public class AnalysedComponentTypeEvent : BaseService<AnalysedComponentTypeEvent, NozomiDbContext>, 
        IAnalysedComponentTypeEvent
    {
        public AnalysedComponentTypeEvent(ILogger<AnalysedComponentTypeEvent> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public ICollection<KeyValuePair<string, int>> GetAllKeyValuePairs()
        {
            return NozomiServiceConstants.analysedComponentTypes;
        }
    }
}