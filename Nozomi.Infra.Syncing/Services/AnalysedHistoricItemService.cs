using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Syncing.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Syncing.Services
{
    public class AnalysedHistoricItemService : BaseService<AnalysedHistoricItemService, NozomiDbContext>, 
        IAnalysedHistoricItemService
    {
        public AnalysedHistoricItemService(ILogger<AnalysedHistoricItemService> logger, 
            NozomiDbContext context) : base(logger, context)
        {
        }

        public long Create(AnalysedHistoricItem item, string userId = null)
        {
            if (item != null)
            {
                _context.AnalysedHistoricItems.Add(item);
                _context.SaveChanges(userId);
            }

            return -1; // Failed..
        }

        public bool Push(long analysedComponentId, string incomingValue, DateTime historicalTime, 
            bool ignoreSimilarityCheck = false, string userId = null)
        {
            if (analysedComponentId > 0 && !string.IsNullOrEmpty(incomingValue))
            {
                var lastHistoric = _context.AnalysedHistoricItems
                    .OrderByDescending(ahi => ahi.CreatedAt)
                    .FirstOrDefault(ahi => ahi.AnalysedComponentId.Equals(analysedComponentId));

                if (lastHistoric != null && 
                    // Time check
                    historicalTime > lastHistoric?.HistoricDateTime)
                {
                    // Either we ignore the similarity check, or we check
                    if (ignoreSimilarityCheck || (lastHistoric.Value != incomingValue))
                    {
                        // Push it
                        _context.AnalysedHistoricItems.Add(new AnalysedHistoricItem
                        {
                            AnalysedComponentId = analysedComponentId,
                            Value = incomingValue,
                            HistoricDateTime = historicalTime
                        });
                        _context.SaveChanges(userId); // done
                    }

                    return true;
                }
                else
                {
                    // Push it
                    _context.AnalysedHistoricItems.Add(new AnalysedHistoricItem
                    {
                        AnalysedComponentId = analysedComponentId,
                        Value = incomingValue,
                        HistoricDateTime = historicalTime
                    });
                    _context.SaveChanges(userId); // done

                    return true;
                }
            }
            
            return false;
        }
    }
}