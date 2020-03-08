using System.Collections.Generic;
using FluentValidation;

namespace Nozomi.Data.ViewModels.WebsocketCommandProperty
{
    public class UpdateWebsocketCommandPropertyInputModel : WebsocketCommandPropertyViewModel
    {
        public new bool IsValid()
        {
            var validator = new UpdateWebsocketCommandPropertyValidator();
            return validator.Validate(this).IsValid;
        }
        
        protected class UpdateWebsocketCommandPropertyValidator 
            : AbstractValidator<UpdateWebsocketCommandPropertyInputModel>
        {
            public UpdateWebsocketCommandPropertyValidator()
            {
                RuleFor(e => e.Type).IsInEnum();

                RuleFor(e => e.Id).GreaterThan(0)
                    .Unless(e => 
                        System.Guid.TryParse(e.Guid, out var g));
                RuleFor(e => e.Guid).NotNull().NotEmpty()
                    .Unless(e => e.Id > 0);
            }
        }
    }
}