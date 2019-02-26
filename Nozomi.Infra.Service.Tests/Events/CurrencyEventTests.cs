using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Nozomi.Data.Models.Currency;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events;
using Xunit;

namespace Nozomi.Infra.Service.Tests.Events
{
    public class CurrencyEventTests
    {
        private Mock<IUnitOfWork<NozomiDbContext>> _uow;
        
        public CurrencyEventTests()
        {
            _uow = new Mock<IUnitOfWork<NozomiDbContext>>();

//            _uow.SetupGet(uw => uw.GetRepository<Currency>())
//                .Returns(new DbSet<Currency>());
        }
        
        [Fact]
        public async Task GetDetailedSuccessfully()
        {
            // Arrange
            var _mockCurrencyEventLogger = new Mock<ILogger<CurrencyEvent>>();
            
            // Act.
            var currencyEvent = new CurrencyEvent(
                _mockCurrencyEventLogger.Object,
                _uow.Object);
            
            var res = currencyEvent.GetDetailed(1,
                new List<ComponentType> {ComponentType.Ask});
            
            // Assert
            Assert.NotNull(res);
        }
    }
}