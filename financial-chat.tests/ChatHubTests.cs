using financial_chat.business.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace SignalRChat.Tests
{
    [TestClass()]
    public class ChatHubTests
    {
        [TestMethod()]
        public async Task SendAsyncTest()
        {
            var stockCode = "/stock=AAPL.US";
            var user = "JohnDoe";
            var expectedResponse = stockCode.Replace("/stock=", "") + " quote is $171.27 per share";
            var responseFromClient = "Symbol,Date,Time,Open,High,Low,Close,Volume\r\nAAPL.US,2022-04-14,22:00:06,170.62,171.27,165.04,165.29,75329376\r\n";
            var serviceMock = new Mock<IStockService>();
            serviceMock.Setup(x => x.GetSymbol(It.IsAny<string>())).ReturnsAsync(responseFromClient).Verifiable();


            var chat = new ChatHub(serviceMock.Object);
            var response = await chat.SendAsync(user, stockCode);

            Assert.IsNotNull(response);
            Assert.AreEqual(expectedResponse, response);
        }

        [TestMethod()]
        public async Task SendAsyncFailTest()
        {
            var stockCode = "/stock=AAPL.US";
            var user = "JohnDoe";
            var expectedResponse = stockCode.Replace("/stock=", "") + " is not a valid code";
            var responseFromClient = "";
            var serviceMock = new Mock<IStockService>();
            serviceMock.Setup(x => x.GetSymbol(It.IsAny<string>())).ReturnsAsync(responseFromClient).Verifiable();


            var chat = new ChatHub(serviceMock.Object);
            var response = await chat.SendAsync(user, stockCode);

            Assert.IsNotNull(response);
            Assert.AreEqual(expectedResponse, response);
        }
    }
}