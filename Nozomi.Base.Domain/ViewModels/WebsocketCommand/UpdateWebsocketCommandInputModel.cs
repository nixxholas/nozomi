using System.Data;
using FluentValidation;

namespace Nozomi.Data.ViewModels.WebsocketCommand
{
    public class UpdateWebsocketCommandInputModel : WebsocketCommandViewModel
    {
        protected class UpdateWebsocketCommandValidator : AbstractValidator<UpdateWebsocketCommandInputModel>
        {
            public UpdateWebsocketCommandValidator()
            {
                RuleFor(c => c.Type).IsInEnum();
                RuleFor(c => c.Name).NotNull().NotEmpty();
                RuleFor(c => c.Delay).GreaterThanOrEqualTo(-1);
                RuleFor(c => c.RequestGuid).NotEmpty().NotNull();
            }
        }
    }
}