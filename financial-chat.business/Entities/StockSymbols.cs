using System.Collections.Generic;

namespace financial_chat.business.Entities
{
    public class StockSymbols
    {
        public List<StockSymbol> Symbols { get; set; }
    }

    public class StockSymbol
    {
        public string Symbol { get; set; }

        public string Date { get; set; }

        public double Open { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Close { get; set; }

        public int Volume { get; set; }
    }
}
