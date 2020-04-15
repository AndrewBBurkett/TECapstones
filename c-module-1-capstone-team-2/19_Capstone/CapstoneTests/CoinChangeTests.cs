using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTests
{
    [TestClass]
    class CoinChangeTests
    {
        
        [TestMethod]

        public void CoinChangeTest()
        {
            VendingMachine testMachine = new VendingMachine();
            testMachine.CoinChange();
            Assert.AreEqual(100M, testMachine.Balance);

        }

    }
}
