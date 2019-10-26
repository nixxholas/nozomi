using AutoMapper;
using Nozomi.Data.Commands;
using Nozomi.Service.ViewModels;

namespace Nozomi.Service.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<CreateRequestViewModel, CreateRequestCommand>()
                .ConstructUsing(c => new CreateRequestCommand(c.Type, c.ResponseType, c.DataPath, c.Delay,
                    c.FailureDelay, c.CurrencySlug, c.CurrencyPair.Id, c.CurrencyTypeId));
        }
    }
}