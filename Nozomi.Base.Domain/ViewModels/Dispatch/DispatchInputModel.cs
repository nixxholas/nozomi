using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.RequestProperty;
using Nozomi.Data.ViewModels.WebsocketCommand;

namespace Nozomi.Data.ViewModels.Dispatch
{
    /// <summary>
    /// Parameters required/used to obtain a payload from the target source.
    /// </summary>
    public class DispatchInputModel
    {
        /// <summary>
        /// A fixed duration on when the socket should be killed
        /// </summary>
        public long? SocketKillSwitchDelay { get; set; }
        
        /// <summary>
        /// Should you require multiple websocket data ingress,
        /// </summary>
        public int? SocketDataCount { get; set; }
        
        /// <summary>
        /// The protocol of the request.
        /// </summary>
        public RequestType RequestType { get; set; }
        
        /// <summary>
        /// The data structure type of the request's incoming payload
        /// </summary>
        public ResponseType ResponseType { get; set; }
        
        /// <summary>
        /// The delay between each request attempt, not used here.
        /// </summary>
        public long Delay { get; set; }
        
        /// <summary>
        /// The delay after a failed request attempt, not used here.
        /// </summary>
        public long FailureDelay { get; set; }
        
        /// <summary>
        /// Declares if this request is enabled immediately once created.
        /// </summary>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// The URL of the data source. 
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// The properties of this request. (i.e. HTTP Headers)
        /// </summary>
        public ICollection<RequestPropertyInputModel>? Properties { get; set; }
        
        /// <summary>
        /// Optional. The commands required to be invoked if this is a websocket dispatch.
        /// </summary>
        public ICollection<CreateWebsocketCommandInputModel>? WebsocketCommands { get; set; }

        public bool IsValid()
        {
            return new DispatchInputValidator().Validate(this).IsValid;
        }

        public class DispatchInputValidator : AbstractValidator<DispatchInputModel>
        {
            public DispatchInputValidator()
            {
                RuleFor(e => e.RequestType).IsInEnum();
                RuleFor(e => e.ResponseType).IsInEnum();
                RuleFor(e => e.Endpoint).NotNull().NotEmpty();
                // Must be empty unless its a websocket dispatch
                RuleFor(e => e.WebsocketCommands).Empty()
                    .Unless(e => !e.RequestType.Equals(RequestType.WebSocket));
                // Hard code some limits for now
                RuleFor(e => e.SocketKillSwitchDelay)
                    .LessThan(TimeSpan.FromSeconds(20).Ticks);
                RuleFor(e => e.SocketDataCount).LessThan(5);
            }
        }
    }
}