using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.SourceType;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class SourceTypeService : BaseService<SourceTypeService, NozomiDbContext>, ISourceTypeService
    {
        private readonly ISourceTypeEvent _sourceTypeEvent;
        
        public SourceTypeService(ILogger<SourceTypeService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork, ISourceTypeEvent sourceTypeEvent) 
            : base(logger, unitOfWork)
        {
            _sourceTypeEvent = sourceTypeEvent;
        }

        public void Create(CreateSourceTypeViewModel vm, string userId)
        {
            if (vm.IsValid() && !_sourceTypeEvent.Exists(vm.Abbreviation))
            {
                var sourceType = new SourceType(vm.Abbreviation, vm.Name);
                
                _unitOfWork.GetRepository<SourceType>().Add(sourceType);
                _unitOfWork.Commit(userId);

                return;
            }
            
            throw new ArgumentException("Invalid properties or a source type with the same abbreviation " +
                                        "already exists.");
        }

        public bool Update(UpdateSourceTypeViewModel vm, string userId)
        {
            if (vm.IsValid())
            {
                
            }
            
            throw new ArgumentException("Invalid source type.");
        }
    }
}