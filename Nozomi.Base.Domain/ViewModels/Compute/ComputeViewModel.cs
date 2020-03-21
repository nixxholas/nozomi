using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.ViewModels.Compute
{
    public class ComputeViewModel : CreateComputeViewModel
    {
        public ComputeViewModel(Guid guid, string key, string formula, int delay, bool isEnabled )
        {
            Guid = guid;
            Key = key;
            Formula = formula;
            Delay = delay;
            IsEnabled = isEnabled;
        }

        public ComputeViewModel(Guid guid, string key, string formula, int delay, bool isEnabled,
            ICollection<ComputeExpression> expressions, ICollection<SubCompute> childComputes,
            ICollection<SubCompute> parentComputes, ICollection<ComputeValue> values)
        {
            Guid = guid;
            Key = key;
            Formula = formula;
            Delay = delay;
            IsEnabled = isEnabled;

            Expressions = expressions;
            ChildComputes = childComputes;
            ParentComputes = parentComputes;
            Values = values;
        }


        public Guid Guid { get; set; }
        
        public bool IsEnabled { get; set; }
        
        public ICollection<ComputeExpression> Expressions { get; set; }
        
        public ICollection<SubCompute> ChildComputes { get; set; }
        

        public ICollection<SubCompute> ParentComputes { get; set; }
        
        public ICollection<ComputeValue> Values { get; set; }
    }
}