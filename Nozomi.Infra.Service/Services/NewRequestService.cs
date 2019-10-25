using AutoMapper;
using Nozomi.Base.Core.Bus;
using Nozomi.Data.Commands;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.ViewModels;

namespace Nozomi.Service.Services
{
    public class NewRequestService : INewRequestService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        
        public NewRequestService(IMapper mapper, IMediatorHandler bus)
        {
            _mapper = mapper;
            _bus = bus;
        }
        
        public void Create(CreateRequestViewModel vm)
        {
            var createCommand = _mapper.Map<CreateRequestCommand>(vm);
            _bus.SendCommand(createCommand);
        }
    }
}