using financial_chat.business.Entities;
using System.Threading.Tasks;

namespace financial_chat.business.Interfaces
{
    public interface IStockService
    {
        Task<string> GetSymbol(string stockCode);
    }
}
