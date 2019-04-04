using System.Linq;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
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

        public bool Push(AnalysedComponent analysedComponent, long userId = 0)
        {
            if (analysedComponent != null)
            {
                var lastHistoric = _unitOfWork.GetRepository<AnalysedHistoricItem>()
                    .Get(ahi => ahi.AnalysedComponentId.Equals(analysedComponent.Id))
                    .OrderByDescending(ahi => ahi.CreatedAt)
                    .FirstOrDefault();

                if (lastHistoric != null)
                {
                    var lastHistoricVal = decimal.Parse(lastHistoric.Value);
                    var existingVal = decimal.Parse(analysedComponent.Value);

                    if (lastHistoricVal != existingVal)
                    {
                        // Push it
                        _unitOfWork.GetRepository<AnalysedHistoricItem>().Add(new AnalysedHistoricItem
                        {
                            AnalysedComponentId = analysedComponent.Id,
                            Value = analysedComponent.Value,
                            HistoricDateTime = analysedComponent.ModifiedAt
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
                        AnalysedComponentId = analysedComponent.Id,
                        Value = analysedComponent.Value,
                        HistoricDateTime = analysedComponent.ModifiedAt
                    });
                    _unitOfWork.Commit(userId); // done

                    return true;
                }
            }
            
            return false;
        }
    }
}