using System.Linq;

namespace XOProject
{
    public interface ITradeRepository : IGenericRepository<Trade>
    {
        IQueryable<TradeAnalysis> GetTradeAnalysis(string symbol);
    }
}