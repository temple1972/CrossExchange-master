using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace XOProject.Tests
{
    public class ShareRepositoryTest
    {
        private Mock<IShareRepository> _iShareRepositoryMock = new Mock<IShareRepository>();
        [Test]
        public void TestTradeModel
        {
            TradeModel tester = new ClassTester(new TradeModel);
            tester.IgnoredProperties.Add("NonsensicalProperty");
            tester.TestProperties();
        }
    }
}
