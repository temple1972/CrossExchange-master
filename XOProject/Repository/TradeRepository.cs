using System.Linq;

namespace XOProject
{
    public class TradeRepository : GenericRepository<Trade>, ITradeRepository
    {
        public TradeRepository(ExchangeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TradeAnalysis> GetTradeAnalysis(string symbol)
        {
            var trades = Query().Where(a => a.Symbol.Equals(symbol)).GroupBy(a => a.Action).Select(s => new TradeAnalysis
            {
                Maximum = s.Max(x => x.NoOfShares),
                Minimum = s.Min(x => x.NoOfShares),
                Average = s.Average(x => x.NoOfShares),
                Sum = s.Sum(x => x.NoOfShares),
                Action = s.Key
            });
            return trades;
        }
    }
}