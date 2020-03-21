using System;
using Microsoft.AspNetCore.Routing;

namespace Nozomi.Data.ViewModels.SubCompute
{
    public class SubComputeViewModel : CreateSubComputeViewModel
    {
        public SubComputeViewModel()
        {
        }

        public SubComputeViewModel(Guid guid, Guid parentComputeGuid, Guid childComputeGuid, bool isEnabled)
        {
            Guid = guid;
            ParentComputeGuid = parentComputeGuid;
            ChildComputeGuid = childComputeGuid;
        }
        
        public SubComputeViewModel(Guid guid, Guid parentComputeGuid, Guid childComputeGuid, Models.Web.Compute parentCompute, Models.Web.Compute childCompute, bool isEnabled)
        {
            Guid = guid;
            ParentComputeGuid = parentComputeGuid;
            ChildComputeGuid = childComputeGuid;

            ParentCompute = parentCompute;
            ChildCompute = childCompute;
        }

        public Guid Guid { get; set; }

        public bool IsEnabled { get; set; }
        
        public Models.Web.Compute ParentCompute { get; set; }
        
        public Models.Web.Compute ChildCompute { get; set; }
    }
}