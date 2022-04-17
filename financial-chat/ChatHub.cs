using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using financial_chat.Controllers;
using financial_chat.business.Services;
using System.Threading.Tasks;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
        }

        public async Task<string> SendAsync(string name, string message)
        {
            string[] splittedMessage = message.Split(' ');
            string result = string.Empty;
            foreach (var item in splittedMessage)
            {
                if (item.Contains("/stock"))
                {
                    var stockCode = item.Replace("/stock=", "");
                    if (string.IsNullOrEmpty(stockCode))
                    {
                        Clients.All.addNewMessageToPage(name, "The stock code is missing");
                    }
                    else
                    {
                        StockService service = new StockService();
                        result = await service.GetSymbol(stockCode);
                        Clients.All.addNewMessageToPage(name, result);
                    }                    
                }
            }
            return result;
        }


    }
}