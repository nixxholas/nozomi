using FluentValidation;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.ViewModels.RequestProperty
{
    public class CreateRequestPropertyInputModel
    {
        public CreateRequestPropertyInputModel() {}

        public CreateRequestPropertyInputModel(RequestPropertyType type, string key, string value)
        {
            Type = type;
            Key = key;
            Value = value;
        }
        
        /// <summary>
        /// The type of the request property.
        /// </summary>
        public RequestPropertyType Type { get; set; }
        
        /// <summary>
        /// The key of the request property, if any.
        /// </summary>
        public string Key { get; set; }
        
        /// <summary>
        /// The value of the request property.
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// The request this property is binded to.
        /// </summary>
        public string RequestGuid { get; set; }
        
        public bool IsValid(bool ignoreRelations = true)
        {
            var validator = new CreateRequestPropertyValidator(ignoreRelations);
            return validator.Validate(this).IsValid;
        }

        protected class CreateRequestPropertyValidator : AbstractValidator<CreateRequestPropertyInputModel>
        {
            public CreateRequestPropertyValidator(bool ignoreRelations)
            {
                RuleFor(e => e.Type).IsInEnum();
                if (!ignoreRelations)
                    RuleFor(e => e.RequestGuid).NotEmpty().NotNull();
            }
        }
    }
}