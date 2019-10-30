using FluentValidation;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.ViewModels.AnalysedComponent
{
    public class CreateAnalysedComponentViewModel
    {
        public AnalysedComponentType Type { get; set; }
        
        public int Delay { get; set; }
        
        public string UiFormatting { get; set; }
        
        public bool IsDenominated { get; set; }
        
        public bool StoreHistoricals { get; set; }
        
        public class CreateAnalysedComponentValidator : AbstractValidator<CreateAnalysedComponentViewModel>
        {
            public CreateAnalysedComponentValidator()
            {
                RuleFor(e => e.Type).IsInEnum();
                RuleFor(e => e.Delay).GreaterThanOrEqualTo(0);
                RuleFor(e => e.UiFormatting).NotNull().NotEmpty();
            }
        }
    }
}