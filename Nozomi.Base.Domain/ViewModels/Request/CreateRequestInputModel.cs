using System.Collections.Generic;
using System.Runtime.Serialization;
using FluentValidation;
using Newtonsoft.Json;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Data.ViewModels.RequestProperty;
using Nozomi.Data.ViewModels.WebsocketCommand;

namespace Nozomi.Data.ViewModels.Request
{
    public class CreateRequestInputModel
    {
        /// <summary>
        /// The protocol type of this request.
        /// </summary>
        public RequestType RequestType { get; set; }

        /// <summary>
        /// The response type (or the payload type) of this request.
        /// </summary>
        public ResponseType ResponseType { get; set; }

        /// <summary>
        /// The URL to the endpoint
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// The delay between each request, in milliseconds.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// The delay after a failed request attempt, in milliseconds.
        /// </summary>
        public long FailureDelay { get; set; }

        /// <summary>
        /// This will deduce what type of request this is for
        /// i.e. CurrencyType, CurrencyPair or Currency.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public RequestParentType ParentType { get; set; } = RequestParentType.None; // Force defaults

        public enum RequestParentType
        {
            None = -1,
            Currency = 0,
            CurrencyPair = 1,
            CurrencyType = 2
        }

        /// <summary>
        /// The unique slug identifier of the currency linked to this request.
        /// </summary>
        public string? CurrencySlug { get; set; }

        /// <summary>
        /// The unique GUID identifier of the currency pair linked to this request.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string? CurrencyPairGuid { get; set; }

        /// <summary>
        /// The unique GUID identifier of the Currency Type linked to this request.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string? CurrencyTypeGuid { get; set; }
        
        /// <summary>
        /// The collection of components for this request.
        /// </summary>
        public ICollection<CreateComponentInputModel> Components { get; set; }
        
        /// <summary>
        /// The collection of request properties for this request.
        /// </summary>
        public ICollection<CreateRequestPropertyInputModel> Properties { get; set; }
        
        /// <summary>
        /// The collection of websocket commands for this request.
        /// </summary>
        public ICollection<CreateWebsocketCommandInputModel> WebsocketCommands { get; set; }
        
        // These properties were for Dispatching, don't touch these.
        [IgnoreDataMember]
        public long SocketDataCount { get; set; }
        [IgnoreDataMember]
        public long SocketKillSwitchDelay { get; set; }

        public bool IsValid()
        {
            var validator = new CreateRequestValidator();
            return validator.Validate(this).IsValid;
        }

        protected class CreateRequestValidator : AbstractValidator<CreateRequestInputModel>
        {
            public CreateRequestValidator()
            {
                RuleFor(r => r.RequestType).IsInEnum();
                RuleFor(r => r.ResponseType).IsInEnum();
                RuleFor(r => r.Endpoint).NotEmpty();
                RuleFor(r => r.Delay).GreaterThan(-1);
                RuleFor(r => r.FailureDelay).GreaterThan(-1);
                RuleFor(r => r.ParentType).IsInEnum();
                // // Safetynet for Currencies
                // RuleFor(r => r.CurrencySlug).NotNull().NotEmpty()
                //     // Ignore the check if a currency pair or currency type is selected.
                //     .Unless(r => 
                //         !string.IsNullOrEmpty(r.CurrencyPairGuid) || !string.IsNullOrEmpty(r.CurrencyTypeGuid));
                // // Safetynet for Currency Pairs
                // RuleFor(r => r.CurrencyPairGuid).NotEmpty().NotNull()
                //     // Ignore the check if a currency or currency type is selected.
                //     .Unless(r => 
                //         !string.IsNullOrEmpty(r.CurrencyTypeGuid) || !string.IsNullOrEmpty(r.CurrencySlug));
                // // Safetynet for Currency types
                // RuleFor(r => r.CurrencyTypeGuid).NotEmpty().NotNull()
                //     .Unless(r =>
                //         // Ignore the check if a currency or currency pair is selected
                //         !string.IsNullOrEmpty(r.CurrencySlug) || !string.IsNullOrEmpty(r.CurrencyPairGuid));

                // Rule for WebsocketCommands to be empty unless its a websocket request type
                RuleFor(e => e.WebsocketCommands).Empty()
                    .Unless(e => e.RequestType
                        .Equals(Models.Web.RequestType.WebSocket));
            }
        }
    }
}