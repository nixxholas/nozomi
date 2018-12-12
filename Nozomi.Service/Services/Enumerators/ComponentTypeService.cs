using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.WebModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;
using Nozomi.Service.Services.Enumerators.Interfaces;

namespace Nozomi.Service.Services.Enumerators
{
    public class ComponentTypeService : BaseService<ComponentTypeService, NozomiDbContext>, IComponentTypeService
    {
        private readonly ICollection<KeyValuePair<string, int>> _componentTypeMap;

        public ComponentTypeService(ILogger<ComponentTypeService> logger,
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
            _componentTypeMap = new List<KeyValuePair<string, int>>();

            foreach (var name in Enum.GetNames(typeof(ComponentType)))
            {
                _componentTypeMap.Add(new KeyValuePair<string, int>(name,
                    (int) Enum.Parse(typeof(ComponentType), name)));
            }
        }

        public ICollection<KeyValuePair<string, int>> All()
        {
            return _componentTypeMap;
        }
    }
}