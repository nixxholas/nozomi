using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels.SourceType;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class SourceTypeService : BaseService<SourceTypeService, NozomiDbContext>, ISourceTypeService
    {
        public SourceTypeService(ILogger<SourceTypeService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public void Create(CreateSourceTypeViewModel vm, string userId)
        {
            if (vm.IsValid() && !_unitOfWork.GetRepository<SourceType>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Any(st => st.Abbreviation.Equals(vm.Abbreviation)))
            {
                var sourceType = new SourceType(vm.Abbreviation, vm.Name);
                
                _unitOfWork.GetRepository<SourceType>().Add(sourceType);
                _unitOfWork.Commit(userId);

                return;
            }
            
            throw new ArgumentException("Invalid properties or a source type with the same abbreviation " +
                                        "already exists.");
        }
    }
}