using System;
using FluentValidation;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.ViewModels.RequestProperty
{
    public class UpdateRequestPropertyInputModel : RequestPropertyViewModel
    {
        public UpdateRequestPropertyInputModel(Guid guid, RequestPropertyType type, string key, string value) 
            : base(guid, type, key, value)
        {
        }
        
        public bool IsValid()
        {
            var validator = new UpdateRequestPropertyValidator();
            return validator.Validate(this).IsValid;
        }

        protected class UpdateRequestPropertyValidator : AbstractValidator<UpdateRequestPropertyInputModel>
        {
            public UpdateRequestPropertyValidator()
            {
                RuleFor(e => e.Guid).NotNull().NotEmpty();
                RuleFor(e => e.Type).IsInEnum();
            }
        }
    }
}