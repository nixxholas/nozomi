using System;
using FluentValidation;
using Nozomi.Data.Models.Web.Websocket;

namespace Nozomi.Data.ViewModels.WebsocketCommandProperty
{
    public class CreateWebsocketCommandPropertyInputModel
    {
        public CommandPropertyType Type { get; set; }
        
        public string Key { get; set; }
        
        public string Value { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsValid()
        {
            var validator = new CreateWebsocketCommandPropertyValidator();
            return validator.Validate(this).IsValid;
        }

        protected class CreateWebsocketCommandPropertyValidator 
            : AbstractValidator<CreateWebsocketCommandPropertyInputModel>
        {
            public CreateWebsocketCommandPropertyValidator()
            {
                RuleFor(e => e.Type).IsInEnum();
            }
        }
    }
}