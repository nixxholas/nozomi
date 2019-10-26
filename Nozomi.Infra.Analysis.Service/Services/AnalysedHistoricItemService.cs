using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Interfaces;
using Nozomi.Data.Models.Analytical;
using Nozomi.Infra.Analysis.Service.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Analysis.Service.Services
{
    public class AnalysedHistoricItemService : BaseService<AnalysedHistoricItemService, NozomiDbContext>, 
        IAnalysedHistoricItemService
    {
        public AnalysedHistoricItemService(ILogger<AnalysedHistoricItemService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public long Create(AnalysedHistoricItem item, long userId = 0)
        {
            if (item != null)
            {
                _unitOfWork.GetRepository<AnalysedHistoricItem>().Add(item);
                _unitOfWork.Commit(userId);
            }

            return -1; // Failed..
        }

        public bool Push(long analysedComponentId, string incomingValue, DateTime historicalTime, 
            bool ignoreSimilarityCheck = false, long userId = 0)
        {
            if (analysedComponentId > 0 && !string.IsNullOrEmpty(incomingValue))
            {
                var lastHistoric = _unitOfWork.GetRepository<AnalysedHistoricItem>()
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
                        _unitOfWork.GetRepository<AnalysedHistoricItem>().Add(new AnalysedHistoricItem
                        {
                            AnalysedComponentId = analysedComponentId,
                            Value = incomingValue,
                            HistoricDateTime = historicalTime
                        });
                        _unitOfWork.Commit(userId); // done
                    }

                    return true;
                }
                else
                {
                    // Push it
                    _unitOfWork.GetRepository<AnalysedHistoricItem>().Add(new AnalysedHistoricItem
                    {
                        AnalysedComponentId = analysedComponentId,
                        Value = incomingValue,
                        HistoricDateTime = historicalTime
                    });
                    _unitOfWork.Commit(userId); // done

                    return true;
                }
            }
            
            return false;
        }
    }
}