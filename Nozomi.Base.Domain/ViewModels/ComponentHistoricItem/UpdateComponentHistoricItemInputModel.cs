using System;
using FluentValidation;

namespace Nozomi.Data.ViewModels.ComponentHistoricItem
{
    public class UpdateComponentHistoricItemInputModel
    {
        /// <summary>
        /// The timestamp of this historical value.
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// The value of this historical item.
        /// </summary>
        public string Value { get; set; }

        public bool IsValid()
        {
            return new UpdateComponentHistoricItemInputValidator().Validate(this).IsValid;
        }
        
        public class UpdateComponentHistoricItemInputValidator : AbstractValidator<UpdateComponentHistoricItemInputModel>
        {
            public UpdateComponentHistoricItemInputValidator()
            {
                RuleFor(e => e.Timestamp).NotNull();
                RuleFor(e => e.Value).NotNull();
            }
        }
    }
}