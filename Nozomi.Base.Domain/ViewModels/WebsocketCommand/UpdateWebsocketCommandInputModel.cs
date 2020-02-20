using System.Collections.Generic;
using System.Data;
using FluentValidation;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;

namespace Nozomi.Data.ViewModels.WebsocketCommand
{
    public class UpdateWebsocketCommandInputModel : WebsocketCommandViewModel
    {
        public new IEnumerable<UpdateWebsocketCommandPropertyInputModel> Properties { get; set; }
        
        protected class UpdateWebsocketCommandValidator : AbstractValidator<UpdateWebsocketCommandInputModel>
        {
            public UpdateWebsocketCommandValidator()
            {
                RuleFor(c => c.Type).IsInEnum();
                RuleFor(c => c.Name).NotNull().NotEmpty();
                RuleFor(c => c.Delay).GreaterThanOrEqualTo(-1);

                RuleFor(c => c.Id).GreaterThan(0)
                    .Unless(e => 
                        System.Guid.TryParse(e.Guid, out var parsedGuid));
                RuleFor(c => c.Guid).Must(guid => 
                    System.Guid.TryParse(guid, out var parsedGuid))
                    .Unless(c => c.Id > 0);
            }
        }

        public new bool IsValid()
        {
            var validator = new UpdateWebsocketCommandValidator();
            return validator.Validate(this).IsValid;
        }
    }
}