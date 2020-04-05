using System;
using System.Collections.Generic;
using FluentValidation;
using Nozomi.Data.ViewModels.ComponentHistoricItem;

namespace Nozomi.Data.ViewModels.Component
{
    public class UpdateComponentInputModel
    {
        public Guid Guid { get; set; }
        
        public long ComponentTypeId { get; set; }

        public string Identifier { get; set; }
        
        public string QueryComponent { get; set; }
        
        public bool IsDenominated { get; set; }
        
        public bool AnomalyIgnorance { get; set; }
        
        public bool StoreHistoricals { get; set; }
        
        public ICollection<UpdateComponentHistoricItemInputModel> History { get; set; }

        public bool IsValid()
        {
            return new UpdateComponentInputValidator().Validate(this).IsValid;
        }
        
        public class UpdateComponentInputValidator : AbstractValidator<UpdateComponentInputModel>
        {
            public UpdateComponentInputValidator()
            {
                RuleFor(e => e.ComponentTypeId).GreaterThan(0);
                RuleFor(e => e.IsDenominated).NotNull();
                RuleFor(e => e.AnomalyIgnorance).NotNull();
                RuleFor(e => e.StoreHistoricals).NotNull();
            }
        }
    }
}