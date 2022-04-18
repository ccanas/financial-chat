using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using financial_chat.Controllers;
using financial_chat.business.Services;
using System.Threading.Tasks;
using System.Linq;
using financial_chat.business.Interfaces;
using financial_chat.business;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
        private IStockService _service;

        public ChatHub(IStockService service)
        {
            _service = service;
        }
        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
            SaveMessage(name, message);
        }

        public async Task<string> SendAsync(string name, string message)
        {
            string[] splittedMessage = message.Split(' ');
            foreach (var item in splittedMessage)
            {
                if (item.Contains("/stock"))
                {
                    var stockCode = item.Replace("/stock=", "");
                    if (string.IsNullOrEmpty(stockCode))
                    {
                        message = "The stock code is missing";
                    }
                    else
                    {
                        var result = await _service.GetSymbol(stockCode);
                        message = string.IsNullOrEmpty(result) ? stockCode + " is not a valid code" : FormatResponse(stockCode, result);
                    }
                }
            }
            return message;
        }

        public void SaveMessage(string name, string msg)
        {
            using(var context = new Entities())
            {
                var message = new Chatroom
                {
                    MessageText = msg,
                    UserName = name,
                    CreatedDate = DateTime.Now
                };
                context.Chatrooms.Add(message);
                context.SaveChanges();
            }
        }

        private string FormatResponse(string stockCode, string response)
        {
            var stockResponse = stockCode + " quote is ";
            string[] lines = response.Replace("\r", "").Split('\n');
            string[] keys = lines[0].Split(',');
            string[] values = lines[1].Split(',');
            for (int i = 0; i < keys.Count(); i++)
            {
                if (keys[i] == "High")
                {
                    if(values[i] == "N/D")
                        stockResponse += " not a valid code";
                    else
                        stockResponse += "$" + values[i] + " per share";
                }
            }

            return stockResponse;
        }
    }
}