using Nozomi.Data.Models.Web;

namespace Nozomi.Infra.Compute.Services.Interfaces
{
    public interface IComputeValueService
    {
        /// <summary>
        /// Just add a new compute value, no hassle.
        /// </summary>
        /// <param name="computeValue">The compute value to add.</param>
        void Add(ComputeValue computeValue);

        /// <summary>
        /// Conditional Add
        ///
        /// Ensures that this value will only be added following the properties in its parent.
        /// </summary>
        /// <param name="computeValue">The compute value to add.</param>
        void Push(ComputeValue computeValue);
    }
}