using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Syncing.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Syncing.Services
{
    public class AnalysedHistoricItemService : BaseService<AnalysedHistoricItemService, NozomiDbContext>, 
        IAnalysedHistoricItemService
    {
        public AnalysedHistoricItemService(ILogger<AnalysedHistoricItemService> logger, 
            IUnitOfWork<NozomiDbContext> context) : base(logger, context)
        {
        }

        public long Create(AnalysedHistoricItem item, string userId = null)
        {
            if (item != null)
            {
                _context.GetRepository<AnalysedHistoricItem>().Add(item);
                _context.Commit(userId);
            }

            return -1; // Failed..
        }

        public bool Push(long analysedComponentId, string incomingValue, DateTime historicalTime, 
            bool ignoreSimilarityCheck = false, string userId = null)
        {
            if (analysedComponentId > 0 && !string.IsNullOrEmpty(incomingValue))
            {
                var lastHistoric = _context.GetRepository<AnalysedHistoricItem>()
                    .Get(ahi => ahi.AnalysedComponentId.Equals(analysedComponentId))
                    .OrderByDescending(ahi => ahi.CreatedAt)
                    .FirstOrDefault();

                if (lastHistoric != null && 
                    // Time check
                    historicalTime > lastHistoric?.HistoricDateTime)
                {
                    // Either we ignore the similarity check, or we check
                    if (ignoreSimilarityCheck || (lastHistoric.Value != incomingValue))
                    {
                        // Push it
                        _context.GetRepository<AnalysedHistoricItem>().Add(new AnalysedHistoricItem
                        {
                            AnalysedComponentId = analysedComponentId,
                            Value = incomingValue,
                            HistoricDateTime = historicalTime
                        });
                        _context.Commit(userId); // done
                    }

                    return true;
                }
                else
                {
                    // Push it
                    _context.GetRepository<AnalysedHistoricItem>().Add(new AnalysedHistoricItem
                    {
                        AnalysedComponentId = analysedComponentId,
                        Value = incomingValue,
                        HistoricDateTime = historicalTime
                    });
                    _context.Commit(userId); // done

                    return true;
                }
            }
            
            return false;
        }
    }
}