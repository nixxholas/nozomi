using FluentValidation;
using Nozomi.Base.Core.Helpers;
using Nozomi.Data.Commands;

namespace Nozomi.Data.Validations
{
    public abstract class RequestValidation<T> : AbstractValidator<T> where T : RequestCommand
    {
        protected void ValidateDataPath()
        {
            RuleFor(c => c.DataPath)
                .NotEmpty().WithMessage("Hey! Don't leave this empty :(")
                .Must(URLHelper.IsUrl).WithMessage("Please ensure you have entered a valid URL.");
        }
    }
}