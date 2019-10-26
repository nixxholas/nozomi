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
                .ForMember(dest => dest.RequestType, 
                    opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.ResponseType, 
                    opt => opt.MapFrom(src => src.ResponseType))
                .ForMember(dest => dest.DataPath, 
                    opt => opt.MapFrom(src => src.DataPath))
                .ForMember(dest => dest.Delay, 
                    opt => opt.MapFrom(src => src.Delay))
                .ForMember(dest => dest.FailureDelay, 
                    opt => opt.MapFrom(src => src.FailureDelay))
                .ForMember(dest => dest.CurrencySlug, 
                    opt => opt.MapFrom(src => src.CurrencySlug))
                .ForMember(dest => dest.CurrencyPairId, 
                    opt => opt.MapFrom(src => src.CurrencyPair.Id))
                .ForMember(dest => dest.CurrencyTypeId, 
                    opt => opt.MapFrom(src => src.CurrencyTypeId))
                .ConstructUsing(c => new CreateRequestCommand(c.Type, c.ResponseType, c.DataPath, c.Delay,
                    c.FailureDelay, c.CurrencySlug, c.CurrencyPair.Id, c.CurrencyTypeId));
        }
    }
}