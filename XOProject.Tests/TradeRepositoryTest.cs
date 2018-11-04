using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace XOProject.Tests
{
    public class TradeRepositoryTest
    {
        private Mock<ITradeRepository> _iTradeRepositoryMock = new Mock<ITradeRepository>();
        [TestCase("REL")]
        public void TradeAnalysis_Does_Not_Return_Null(string symbol)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<ExchangeContext>()
            .UseInMemoryDatabase(databaseName: "GetTradeAnalysis")
            .Options;
            using (var context = new ExchangeContext(options))
            {

                context.Trades.Add(new Trade
                {
                    Symbol = "REL",
                    Action = "BUY",
                    Id = 1,
                    NoOfShares = 200,
                    PortfolioId = 3,
                    Price = 343
                });
                context.Trades.Add(
                    new Trade
                    {
                        Symbol = "KEL",
                        Action = "SELL",
                        Id = 2,
                        NoOfShares = 100,
                        PortfolioId = 2,
                        Price = 232
                    }
                    );
                context.SaveChanges();
                //Act
                ITradeRepository repo = new TradeRepository(context);
                var result = repo.GetTradeAnalysis(symbol);
                //Assert
                Assert.NotNull(result);


            }
        }

    }
}
