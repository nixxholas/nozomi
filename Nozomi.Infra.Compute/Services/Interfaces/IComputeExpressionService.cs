using System;

namespace Nozomi.Infra.Compute.Services.Interfaces
{
    public interface IComputeExpressionService
    {
        void UpdateValue(string expressionGuid, string value);

        void UpdateValue(Guid expressionGuid, string value);
    }
}