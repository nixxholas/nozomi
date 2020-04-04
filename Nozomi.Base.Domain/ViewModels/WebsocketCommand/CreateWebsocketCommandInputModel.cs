using System;
using System.Collections.Generic;
using FluentValidation;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;

namespace Nozomi.Data.ViewModels.WebsocketCommand
{
    public class CreateWebsocketCommandInputModel
    {
        /// <summary>
        /// The type of the command.
        /// </summary>
        public CommandType Type { get; set; }
        
        /// <summary>
        /// Name of the command
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The delay of the command in milliseconds. Can be greater or equal to -1; Where -1 equals
        /// to a self-repeating command.
        /// </summary>
        public long Delay { get; set; }
        
        /// <summary>
        /// The unique identifier of the request this command is linked to.
        /// </summary>
        public Guid RequestGuid { get; set; }

        /// <summary>
        /// Is this enabled?
        /// </summary>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// The properties of the command.
        /// </summary>
        public ICollection<CreateWebsocketCommandPropertyInputModel> Properties { get; set; }

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