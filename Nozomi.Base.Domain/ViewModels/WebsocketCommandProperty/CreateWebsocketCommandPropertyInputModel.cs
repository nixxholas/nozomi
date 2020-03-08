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

        public string CommandGuid { get; set; }

        public long CommandId { get; set; }
        
        public bool IsValid()
        {
            var validator = new CreateWebsocketCommandPropertyValidator();
            return validator.Validate(this).IsValid;
        }

        public bool IsValidDependant()
        {
            var validator = new CreateDependentWebsocketCommandPropertyValidator();
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

        protected class CreateDependentWebsocketCommandPropertyValidator :
            AbstractValidator<CreateWebsocketCommandPropertyInputModel>
        {
            public CreateDependentWebsocketCommandPropertyValidator()
            {
                RuleFor(e => e.Type).IsInEnum();
                
                RuleFor(e => e.CommandGuid).NotNull().NotEmpty()
                    .Unless(e => e.CommandId > 0);

                RuleFor(e => e.CommandId).GreaterThan(0)
                    .Unless(e => Guid.TryParse(e.CommandGuid, 
                        out var o));
            }
        }
    }
}