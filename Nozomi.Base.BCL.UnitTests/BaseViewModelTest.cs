using Xunit;

namespace Nozomi.Base.BCL.UnitTests
{
    public class BaseViewModelTest
    {
        [Fact]
        public void initialise_baseviewmodel_success()
        {
            var bvm = new BaseViewModel
            {
                StatusMessage = "Testing"
            };
            
            Assert.True(bvm != null && !string.IsNullOrEmpty(bvm.StatusMessage));
        }
    }
}