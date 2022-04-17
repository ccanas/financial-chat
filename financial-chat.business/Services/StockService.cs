using financial_chat.business.Interfaces;
using System;
using System.Threading.Tasks;
using financial_chat.business.Entities;
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
            var response = await RequestSymbol(stockCode);
            var result = "";

            if (!string.IsNullOrEmpty(response))
            {
                result = FormatResponse(stockCode, response);
            }
            else
            {
                result = stockCode + " is not a valid code";
            }
            return result;
        }

        private HttpRequestMessage GetHttpRequest(string url)
        {
            var httpRequesMessage = new HttpRequestMessage(HttpMethod.Get, url);
            return httpRequesMessage;
        }

        private string FormatResponse(string stockCode, string response)
        {
            var stockResponse = stockCode + " quote is $";
            string[] lines = response.Replace("\r", "").Split('\n');
            string[] keys = lines[0].Split(',');
            string[] values = lines[1].Split(',');
            for (int i = 0; i < keys.Count(); i++)
            {
                if (keys[i] == "High")
                {
                    stockResponse += values[i] + " per share";
                }
            }

            return stockResponse;
        }

        public async Task<string> RequestSymbol(string stockCode)
        {
            var requestPath = GetRequestPath(stockCode);
            var url = baseUrl + requestPath;
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);

            return response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : "";
            

        }
    }
}
