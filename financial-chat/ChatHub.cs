﻿using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using financial_chat.Controllers;
using financial_chat.business.Services;
using System.Threading.Tasks;
using System.Linq;
using financial_chat.business.Interfaces;

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
                        Clients.All.addNewMessageToPage(name, message);
                    }
                    else
                    {
                        var result = await _service.GetSymbol(stockCode);
                        message = FormatResponse(stockCode, result);
                        Clients.All.addNewMessageToPage(name, message);
                    }
                }
            }
            return message;
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
    }
}