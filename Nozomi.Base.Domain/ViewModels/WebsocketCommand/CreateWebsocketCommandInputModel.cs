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
        
        public string RequestGuid { get; set; }

        public bool IsEnabled { get; set; }
        
        public List<CreateWebsocketCommandPropertyInputModel> Properties { get; set; }

        public bool IsValid()
        {
            var validator = new CreateWebsocketCommandValidator();
            return validator.Validate(this).IsValid;
        }
        
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