using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuatroCleanUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuatroCleanUp.MSTests
{
    [TestClass()]
    public class EventTests
    {
        [TestInitialize]
        public void Setup()
        {
            Event testEvent = new Event("Title", "Description", 05/06/2025 01:00:00PM, );
        }

        [TestMethod()]
        public void EventTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ToStringTest()
        {

            Assert.Fail();
        }
    }
}