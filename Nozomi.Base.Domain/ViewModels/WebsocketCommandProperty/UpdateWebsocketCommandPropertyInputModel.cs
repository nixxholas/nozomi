using System.Collections.Generic;
using FluentValidation;

namespace Nozomi.Data.ViewModels.WebsocketCommandProperty
{
    public class UpdateWebsocketCommandPropertyInputModel : WebsocketCommandPropertyViewModel
    {
        protected class UpdateWebsocketCommandPropertyValidator 
            : AbstractValidator<UpdateWebsocketCommandPropertyInputModel>
        {
            public UpdateWebsocketCommandPropertyValidator()
            {
                RuleFor(e => e.Type).IsInEnum();
            }
        }
    }
}