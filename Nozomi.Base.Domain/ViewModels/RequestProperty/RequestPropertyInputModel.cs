using FluentValidation;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.ViewModels.RequestProperty
{
    /// <summary>
    /// When you want to interact with request properties without any binding to a request.
    /// </summary>
    public class RequestPropertyInputModel
    {
        public RequestPropertyInputModel() {}

        public RequestPropertyInputModel(RequestPropertyType type, string key, string value)
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
        
        public bool IsValid()
        {
            return new RequestPropertyValidator().Validate(this).IsValid;
        }

        private class RequestPropertyValidator : AbstractValidator<RequestPropertyInputModel>
        {
            public RequestPropertyValidator()
            {
                RuleFor(e => e.Type).IsInEnum();
            }
        }
    }
}