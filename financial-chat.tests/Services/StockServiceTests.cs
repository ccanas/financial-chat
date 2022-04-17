using Microsoft.VisualStudio.TestTools.UnitTesting;
using financial_chat.business.Interfaces;
using Moq;
using System.Net.Http;

namespace financial_chat.business.Services.Tests
{
    [TestClass()]
    public class StockServiceTests
    {
        [TestMethod()]
        public async void GetSymbolTest()
        {
            var stockCode = "AAPL.US";
            var expectedResponse = stockCode + " quote is $102 per share";
            var serviceMock = new Mock<IStockService>();
            var responseFromClient = "Symbol,Date,Time,Open,High,Low,Close,Volume\r\nAAPL.US,2022-04-14,22:00:06,170.62,171.27,165.04,165.29,75329376\r\n";
            serviceMock.Setup(x => x.RequestSymbol(It.IsAny<string>())).ReturnsAsync(responseFromClient).Verifiable();


            var service = new StockService();
            var response = await service.GetSymbol(stockCode);

            Assert.IsNotNull(response);
            Assert.Equals(expectedResponse, response);
        }
    }
}