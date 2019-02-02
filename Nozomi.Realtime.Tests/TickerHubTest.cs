using System;
using Microsoft.Extensions.Logging;
using Moq;
using Nozomi.Realtime.Infra.Service.Hubs;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;
using Xunit;

namespace Nozomi.Realtime.Tests
{
    public class TickerHubTest
    {
        [Fact]
        public void ConnectionTest()
        {
            var mockTickerEvent = new Mock<ITickerEvent>();
            var mockCurrencyPairService = new Mock<ICurrencyPairService>();
            var mockLogger = new Mock<ILogger<TickerHub>>(); 
                
            var hub = new TickerHub(mockTickerEvent.Object, mockCurrencyPairService.Object, mockLogger.Object);
            
        }
    }
}