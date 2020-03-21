using System;

namespace Nozomi.Data.ViewModels.ComputeValue
{
    public class ComputeValueViewModel : CreateComputeValueViewModel
    {
        public ComputeValueViewModel()
        {
        }

        public ComputeValueViewModel(Guid guid, string value, Guid computeGuid, bool isEnabled)
        {
            Guid = guid;
            Value = value;
            ComputeGuid = computeGuid;
            IsEnabled = isEnabled;
        }

        public ComputeValueViewModel(Guid guid, string value, Guid computeGuid, bool isEnabled, Models.Web.Compute compute)
        {
            Guid = guid;
            Value = value;
            ComputeGuid = computeGuid;
            IsEnabled = isEnabled;
            
            Compute = compute;
        }

        public Guid Guid { get; set; }
        
        public bool IsEnabled { get; set; }
        
        public Models.Web.Compute Compute { get; set; }
    }
}