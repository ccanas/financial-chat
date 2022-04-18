using financial_chat.business.Interfaces;
using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Linq;
using System.Text;

namespace financial_chat.business.Services
{
    public class StockService : IStockService
    {
        private readonly string baseUrl = ConfigurationManager.AppSettings["SooqBaseUrl"];

        private string GetRequestPath(string stockCode) =>
            $"s={stockCode}&f=sd2t2ohlcv&h&e=csv";

        public async Task<string> GetSymbol(string stockCode)
        {
            var requestPath = GetRequestPath(stockCode);
            var url = baseUrl + requestPath;
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);

            return response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : "";
        }
    }
}
