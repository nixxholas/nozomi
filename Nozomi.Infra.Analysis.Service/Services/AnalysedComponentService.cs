using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Analysis.Service.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Analysis.Service.Services
{
    public class AnalysedComponentService : BaseService<AnalysedComponentService, NozomiDbContext>, 
        IAnalysedComponentService
    {
        private readonly IAnalysedHistoricItemService _analysedHistoricItemService;
        
        public AnalysedComponentService(ILogger<AnalysedComponentService> logger, 
            IAnalysedHistoricItemService analysedHistoricItemService, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
            _analysedHistoricItemService = analysedHistoricItemService;
        }

        public long Create(AnalysedComponent analysedComponent, long userId = 0)
        {
            if (analysedComponent != null)
            {
                _unitOfWork.GetRepository<AnalysedComponent>().Add(analysedComponent);
                _unitOfWork.Commit(userId);

                return analysedComponent.Id;
            }
            
            return -1;
        }

        public bool UpdateValue(long analysedComponentId, string value, long userId = 0)
        {
            if (string.IsNullOrEmpty(value)) return false;
            
            var comp = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsTracking()
                .Include(ac => ac.AnalysedHistoricItems)
                .SingleOrDefault(ac => ac.Id.Equals(analysedComponentId) &&
                                       ac.DeletedAt == null);

            if (comp != null)
            {
                if (_analysedHistoricItemService.Push(comp))
                {
                    comp.Value = value;
                
                    _unitOfWork.GetRepository<AnalysedComponent>().Update(comp);
                    _unitOfWork.Commit(userId);

                    return true;
                }
            }

            return false;
        }
    }
}