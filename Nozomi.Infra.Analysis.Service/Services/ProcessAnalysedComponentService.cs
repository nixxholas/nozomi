using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Interfaces;
using Nozomi.Data.Models.Analytical;
using Nozomi.Infra.Analysis.Service.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Analysis.Service.Services
{
    public class ProcessAnalysedComponentService : BaseService<ProcessAnalysedComponentService, NozomiDbContext>, 
        IProcessAnalysedComponentService
    {
        private readonly IAnalysedHistoricItemService _analysedHistoricItemService;
        
        public ProcessAnalysedComponentService(ILogger<ProcessAnalysedComponentService> logger, 
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
                if (comp.StoreHistoricals && !string.IsNullOrEmpty(comp.Value))
                    _analysedHistoricItemService.Push(comp.Id, comp.Value, comp.ModifiedAt);
                
                if (comp.IsFailing)
                    comp.IsFailing = false;
                    
                // Make sure we update the datetime as well.. 
                comp.ModifiedAt = DateTime.UtcNow;
                comp.Value = value;
                
                _unitOfWork.Commit(userId);

                return true;
            }

            return false;
        }

        public bool Checked(long analysedComponentId, bool isFailing = false, long userId = 0)
        {
            var comp = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsTracking()
                .SingleOrDefault(ac => ac.Id.Equals(analysedComponentId) && ac.DeletedAt == null);

            if (comp != null)
            {
                comp.IsFailing = isFailing;
                
                _unitOfWork.Commit(userId);

                return true;
            }
            
            return false;
        }

        public bool Disable(long analysedComponentId, long userId = 0)
        {
            var comp = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsTracking()
                .SingleOrDefault(ac => ac.DeletedAt == null
                                       && ac.IsEnabled
                                       && ac.Id.Equals(analysedComponentId));

            if (comp != null)
            {
                comp.IsEnabled = false;
                comp.ModifiedAt = DateTime.UtcNow;

                _unitOfWork.Commit();
            }

            return false;
        }
    }
}