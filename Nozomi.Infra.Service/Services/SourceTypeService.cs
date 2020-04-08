using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.SourceType;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class SourceTypeService : BaseService<SourceTypeService, NozomiDbContext>, ISourceTypeService
    {
        private readonly ISourceTypeEvent _sourceTypeEvent;
        
        public SourceTypeService(ILogger<SourceTypeService> logger, 
            NozomiDbContext context, ISourceTypeEvent sourceTypeEvent) 
            : base(logger, context)
        {
            _sourceTypeEvent = sourceTypeEvent;
        }

        public void Create(CreateSourceTypeViewModel vm, string userId)
        {
            if (vm.IsValid() && !_sourceTypeEvent.Exists(vm.Abbreviation))
            {
                var sourceType = new SourceType(vm.Abbreviation, vm.Name);
                
                _context.SourceTypes.Add(sourceType);
                _context.SaveChanges(userId);

                return;
            }
            
            throw new ArgumentException("Invalid properties or a source type with the same abbreviation " +
                                        "already exists.");
        }

        public bool Update(UpdateSourceTypeViewModel vm, string userId)
        {
            if (vm.IsValid() && _sourceTypeEvent.Exists(vm.Guid))
            {
                var sourceType = _sourceTypeEvent.Get(vm.Guid);

                if (sourceType != null)
                {
                    if (vm.DeletedAt != null)
                    {
                        sourceType.DeletedAt = DateTime.UtcNow;
                        sourceType.DeletedById = userId;
                        
                        _context.SourceTypes.Update(sourceType);
                        _context.SaveChanges(userId);

                        return true;
                    }
                    
                    if (vm.IsEnabled != null)
                        sourceType.IsEnabled = (bool)vm.IsEnabled;
                    
                    if (!string.IsNullOrEmpty(vm.Name))
                        sourceType.Name = vm.Name;

                    if (!string.IsNullOrEmpty(vm.Abbreviation) 
                        && !_sourceTypeEvent.Exists(vm.Abbreviation))
                        sourceType.Abbreviation = vm.Abbreviation;
                    
                    _context.SourceTypes.Update(sourceType);
                    _context.SaveChanges(userId);

                    return true;
                }
            }
            
            throw new ArgumentException("Invalid source type.");
        }
    }
}