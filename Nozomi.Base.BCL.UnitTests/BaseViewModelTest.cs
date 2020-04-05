using Xunit;

namespace Nozomi.Base.BCL.UnitTests
{
    public class BaseViewModelTest
    {
        [Fact]
        public void InitialBaseViewModelSuccess()
        {
            var bvm = new BaseViewModel
            {
                StatusMessage = "Testing"
            };

            bvm.StatusMessage = "work";
            
            Assert.True(bvm != null 
                        && !string.IsNullOrEmpty(bvm.StatusMessage)
                        && bvm.StatusMessage == "work");
        }
    }
}