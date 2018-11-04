using System;
using System.Threading.Tasks;
using XOProject.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace XOProject.Tests
{
    public class ShareControllerTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();

        private readonly ShareController _shareController;

        public ShareControllerTests()
        {
            _shareController = new ShareController(_shareRepositoryMock.Object);
        }

        [Test]
        public async Task Post_ShouldInsertHourlySharePrice()
        {
            // Arrange
            var hourRate = new HourlyShareRate
            {
                Symbol = "CBI",
                Rate = 330.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
            };



            // Act
            var result = await _shareController.Post(hourRate);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task Post_Should_Return_Share_Created()
        {
            // Arrange
            var hourRate = new HourlyShareRate
            {
                Symbol = "CBI",
                Rate = 330.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
            };



            // Act
            var result = await _shareController.Post(hourRate);

            // Assert
            var createdResult = (result as CreatedResult).Value as HourlyShareRate;

            Assert.NotNull(createdResult);
            Assert.AreEqual(hourRate, createdResult);
        }

        [TestCase("CBI", true)]
        [TestCase("DBI", true)]
        public async Task Get_ShouldReturn_value(string symbol, bool expectedResult)
        {
            bool isObjectReturned = false;
            //Arrange
            IQueryable<HourlyShareRate> share = new List<HourlyShareRate>()
            {
                new HourlyShareRate()
                {
                Symbol = "CBI",
                Rate = 330.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
                },new HourlyShareRate()
                {
                Symbol = "DBI",
                Rate = 330.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
                }
            }.AsQueryable();

            _shareRepositoryMock.Setup(x => x.Query()).Returns(share);
            //Act 
            var result = await _shareController.Get(symbol) as ObjectResult;

            var objectReturned = result.Value as List<HourlyShareRate>;
            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(objectReturned);

            if (objectReturned.Count() > 0)
            {
                isObjectReturned = true;
            }



            Assert.AreEqual(expectedResult, isObjectReturned);

        }
        [Test]
        public async Task Post_ShouldValidateModelStates()
        {
            //Arrange
            var hourRate = new HourlyShareRate
            {
                //Symbol = "CBI",
                //Rate = 330.0M,
                //TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
            };


            _shareController.ModelState.AddModelError("Error", "Error");
            //Act 
            var result = await _shareController.Post(hourRate) as ObjectResult;

            // Assert
            Assert.AreEqual(400, result.StatusCode);
        }
        [TestCase("CBI", true)]
        [TestCase("DBI", false)]
        public void Get_LatestPrice_should_Return_Ok(string symbol, bool expectedResult)
        {

            //Arrange
            IQueryable<HourlyShareRate> share = new List<HourlyShareRate>()
            {
                new HourlyShareRate()
                {
                Symbol = "CBI",
                Rate = 30.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
                },new HourlyShareRate()
                {
                Symbol = "DBI",
                Rate = 330.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
                }
            }.AsQueryable();

            _shareRepositoryMock.Setup(x => x.Query()).Returns(share);
            //Act 
            var result = _shareController.GetLatestPrice(symbol).Result as ObjectResult;

            //Assert
            Assert.AreEqual(200, result.StatusCode);
        }

    }
}
