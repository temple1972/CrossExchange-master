using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XOProject.Controller;

namespace XOProject.Tests
{
    public class PortfolioControllerTests
    {
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock = new Mock<IPortfolioRepository>();
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();

        private readonly PortfolioController _portfolioController;

        public PortfolioControllerTests()
        {
            _portfolioController = new PortfolioController(_shareRepositoryMock.Object, _tradeRepositoryMock.Object, _portfolioRepositoryMock.Object);
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        public async Task GetPortfolioInfo_ShouldReturnPortfolio(int id, bool expectedResult)
        {
            // Arrange
            var isObjectReturned = false;
            var trade = new List<Trade>();
            trade.Add(new Trade
            {
                Action = "Buy",
                NoOfShares = 302,
                Id = 2,
                Price = 2332

            });
            IQueryable<Portfolio> portfolios = new List<Portfolio>()
            {
                new Portfolio()
                {
                    Id = 1,
                    Name = "Adewale Johnson",
                    Trade = trade
                }
            }.AsQueryable();
            _portfolioRepositoryMock.Setup(x => x.GetAll()).Returns(portfolios);

            // Act
            var result = await _portfolioController.GetPortfolioInfo(id) as ObjectResult;
            var objectReturned = result.Value as IQueryable<Portfolio>;
            if (objectReturned.Count() > 0)
            {
                isObjectReturned = true;
            }


            // Assert
            Assert.AreEqual(expectedResult, isObjectReturned);
        }

        [Test]
        public async Task Post_ShouldValidateModelStates()
        {
            //Arrange
            var portfolio = new Portfolio
            {
                Name = "",
                Trade = new List<Trade> { }
            };
            _portfolioController.ModelState.AddModelError("Error", "Error");
            //Act 
            var result = await _portfolioController.Post(portfolio) as ObjectResult;
            var createdResult = result as CreatedResult;
            // Assert
            Assert.AreEqual(400, result.StatusCode);
        }
        [Test]
        public async Task Post_ShouldReturnValue()
        {

            //Arrange
            var trade = new List<Trade>();
            trade.Add(new Trade
            {
                Action = "Buy",
                NoOfShares = 302,
                Id = 2,
                Price = 2332

            });
            var portfolio = new Portfolio
            {
                Name = "Owoeye Femi",
                Trade = trade
            };

            //Act 
            var result = await _portfolioController.Post(portfolio) as ObjectResult;

            // Assert
            Assert.NotNull(portfolio.Name);

            Assert.NotNull(result);
            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            var objectReturned = result.Value as Portfolio;
            Assert.AreEqual(portfolio, objectReturned);
        }



    }
}
