using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace XOProject.Tests
{
    public class PortfolioRepositoryTest
    {
        private Mock<IPortfolioRepository> _portfolioRepository = new Mock<IPortfolioRepository>();
        [Test]
        public void GetAll_Return_Valu()
        {

            //Arrange
            var options = new DbContextOptionsBuilder<ExchangeContext>()
            .UseInMemoryDatabase(databaseName: "GetPortfolio")
            .Options;
            using (var context = new ExchangeContext(options))
            {
                var trade = new List<Trade>
                {
                    new Trade
                    {
                        Action = "Buy",
                        NoOfShares = 302,
                        Id = 2,
                        Price = 2332

                    }
                };

                context.Portfolios.Add(new Portfolio
                {

                    Id = 1,
                    Name = "Adewale Johnson",
                    Trade = trade

                });
                context.SaveChanges();
                //Act
                IPortfolioRepository repo = new PortfolioRepository(context);
                var result = repo.GetAll();
                //Assert
                Assert.NotNull(result);
            }
        }
    }
}
