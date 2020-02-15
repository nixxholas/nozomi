using System;
using System.Collections.Generic;
using FluentValidation;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;

namespace Nozomi.Data.ViewModels.WebsocketCommand
{
    public class CreateWebsocketCommandInputModel
    {
        public CommandType Type { get; set; }
        
        public string Name { get; set; }
        
        public long Delay { get; set; }
        
        public Guid RequestGuid { get; set; }
        
        public ICollection<CreateWebsocketCommandPropertyInputModel> Properties { get; set; }
        
        protected class CreateWebsocketCommandValidator : AbstractValidator<CreateWebsocketCommandInputModel>
        {
            public CreateWebsocketCommandValidator()
            {
                RuleFor(c => c.Type).IsInEnum();
                RuleFor(c => c.Name).NotNull().NotEmpty();
                RuleFor(c => c.Delay).GreaterThanOrEqualTo(-1);
                RuleFor(c => c.RequestGuid).NotEmpty().NotNull();
            }
        }
    }
}