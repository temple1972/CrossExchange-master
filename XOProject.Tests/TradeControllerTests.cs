using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XOProject.Controller;

namespace XOProject.Tests
{
    class TradeControllerTest
    {
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock = new Mock<IPortfolioRepository>();
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();

        private readonly Controller.TradeController _tradeController;
        public TradeControllerTest()
        {
            _tradeController = new Controller.TradeController(_shareRepositoryMock.Object,
                _tradeRepositoryMock.Object,
                _portfolioRepositoryMock.Object);

        }
        [TestCase(3, true)]
        [TestCase(2, true)]
        public void GetAllTradings_Should_Return_Value(int id, bool expectedResult)
        {
            bool isObjectReturned = false;
            //Arrange 
            List<Trade> trade = new List<Trade>()
            {
              new Trade
              {
                  Symbol = "REL" , Action = "Buy" , Id = 1 , NoOfShares = 200,
                  PortfolioId = 3
              },
                new Trade
              {
                  Symbol = "CIB" , Action = "Buy" , Id = 2 , NoOfShares = 200,
                  PortfolioId = 2
              }
            };
            //Act
            _tradeRepositoryMock.Setup(a => a.Query()).Returns(trade.AsQueryable());
            var result = _tradeController.GetAllTradings(id).Result as ObjectResult;
            var objectReturned = result.Value as IQueryable<Trade>;
            //Assert
            Assert.IsNotNull(objectReturned);
            Assert.NotNull(result);
            if (objectReturned.Count() > 0)
            {
                isObjectReturned = true;
            }

            Assert.AreEqual(isObjectReturned, expectedResult);
        }

        [TestCase(3)]
        [TestCase(2)]
        public void GetAllTradings_Should_Return_Ok(int id)
        {

            //Arrange 
            List<Trade> trade = new List<Trade>()
            {
              new Trade
              {
                  Symbol = "REL" , Action = "Buy" , Id = 1 , NoOfShares = 200,
                  PortfolioId = 3
              },
                new Trade
              {
                  Symbol = "CIB" , Action = "Buy" , Id = 2 , NoOfShares = 200,
                  PortfolioId = 2
              }
            };
            //Act
            _tradeRepositoryMock.Setup(a => a.Query()).Returns(trade.AsQueryable());
            var result = _tradeController.GetAllTradings(id).Result as ObjectResult;

            //Assert


            Assert.AreEqual(200, result.StatusCode);
        }
        [TestCase("REL")]
        public void GetAnalysis_Should_Return_Ok(string symbol)
        {
            bool isObjectReturned = false;
            //Arrange 
            List<Trade> trade = new List<Trade>()
            {
              new Trade
              {
                  Symbol = "REL" , Action = "BUY" , Id = 1 , NoOfShares = 200,
                  PortfolioId = 3 , Price = 343
              },
                new Trade
              {
                  Symbol = "REL" , Action = "SELL" , Id = 2 , NoOfShares = 100,
                  PortfolioId = 2 , Price = 232
              }
            };
            //var tradeAnalysis = new List<TradeAnalysis>();
            var tradeAnalysis = trade.Where(s => s.Symbol == symbol).GroupBy(a => a.Action).Select
                (g => new TradeAnalysis
                {
                    Maximum = g.Max(a => a.NoOfShares),
                    Minimum = g.Min(a => a.NoOfShares),
                    Average = g.Average(a => a.NoOfShares),
                    Sum = g.Sum(a => a.NoOfShares),
                    Action = g.Key
                }).ToList();
            //Act
            _tradeRepositoryMock.Setup(a => a.GetTradeAnalysis(symbol)).Returns(tradeAnalysis.AsQueryable());
            var result = _tradeController.GetAnalysis(symbol).Result as ObjectResult;
            var objectReturned = result.Value as List<TradeAnalysis>;
            //Assert
            Assert.IsNotNull(result);

            if (result.StatusCode == 200)
            {
                isObjectReturned = true;
            }

            Assert.AreEqual(isObjectReturned, true);
        }
        [TestCase("REL")]
        public void GetAnalysis_Should_Return_Valid_Result(string symbol)
        {
            bool isObjectReturned = false;
            //Arrange 
            List<Trade> trade = new List<Trade>()
            {
              new Trade
              {
                  Symbol = "REL" , Action = "BUY" , Id = 1 , NoOfShares = 200,
                  PortfolioId = 3 , Price = 343
              },
                new Trade
              {
                  Symbol = "REL" , Action = "BUY" , Id = 2 , NoOfShares = 100,
                  PortfolioId = 2 , Price = 232
              }
            };
            var tradeAnalysis = trade.Where(s => s.Symbol == symbol).GroupBy(a => a.Action).Select
                (g => new TradeAnalysis
                {
                    Maximum = g.Max(a => a.NoOfShares),
                    Minimum = g.Min(a => a.NoOfShares),
                    Average = g.Average(a => a.NoOfShares),
                    Sum = g.Sum(a => a.NoOfShares),
                    Action = g.Key
                }).ToList();
            //Act
            _tradeRepositoryMock.Setup(a => a.GetTradeAnalysis(symbol)).Returns(tradeAnalysis.AsQueryable());
            var result = _tradeController.GetAnalysis(symbol).Result as ObjectResult;
            var objectReturned = result.Value as List<TradeAnalysis>;
            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(objectReturned);
            if (objectReturned.Count() == tradeAnalysis.Count())
            {
                isObjectReturned = true;
            }
            else
            {

            }

            Assert.AreEqual(isObjectReturned, true);
        }
        [TestCase("REL")]
        public void GetAnalysis_Should_Return_Valid_Max(string symbol)
        {
            bool isObjectReturned = false;
            //Arrange 
            List<Trade> trade = new List<Trade>()
            {
              new Trade
              {
                  Symbol = "REL" , Action = "BUY" , Id = 1 , NoOfShares = 200,
                  PortfolioId = 3 , Price = 343
              },
                new Trade
              {
                  Symbol = "REL" , Action = "BUY" , Id = 2 , NoOfShares = 100,
                  PortfolioId = 2 , Price = 232
              }
            };
            //var tradeAnalysis = new List<TradeAnalysis>();
            var tradeAnalysis = trade.Where(s => s.Symbol == symbol).GroupBy(a => a.Action).Select
                (g => new TradeAnalysis
                {
                    Maximum = g.Max(a => a.NoOfShares),
                    Minimum = g.Min(a => a.NoOfShares),
                    Average = g.Average(a => a.NoOfShares),
                    Sum = g.Sum(a => a.NoOfShares),
                    Action = g.Key
                }).ToList();
            //Act
            _tradeRepositoryMock.Setup(a => a.GetTradeAnalysis(symbol)).Returns(tradeAnalysis.AsQueryable());
            var result = _tradeController.GetAnalysis(symbol).Result as ObjectResult;
            var objectReturned = result.Value as List<TradeAnalysis>;
            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(objectReturned);
            if (objectReturned.FirstOrDefault().Sum == tradeAnalysis.FirstOrDefault().Sum)
            {
                isObjectReturned = true;
            }
            else
            {

            }

            Assert.AreEqual(isObjectReturned, true);
        }
    }
}
