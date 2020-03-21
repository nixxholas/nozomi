using System;
using FluentValidation;
using Nozomi.Data.Models.Web.Websocket;

namespace Nozomi.Data.ViewModels.WebsocketCommandProperty
{
    public class CreateWebsocketCommandPropertyInputModel
    {
        /// <summary>
        /// The type of the command property.
        /// </summary>
        public CommandPropertyType Type { get; set; }
        
        /// <summary>
        /// The key of where the property's value should be.
        /// </summary>
        public string Key { get; set; }
        
        /// <summary>
        /// The value of the command property.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Enabled for use?
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// The unique GUID identifier of the command this property is linked to..
        /// </summary>
        public string CommandGuid { get; set; }

        /// <summary>
        /// The unique identifier of the command this property is linked to..
        /// </summary>
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