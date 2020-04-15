using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTests
{
    [TestClass]
    public class FeedMoneyTests
    {
        [TestMethod]

        
        public void FeedMoneyTest1()
        {
            VendingMachine testMachine = new VendingMachine();
             testMachine.FeedMoney(10);
            Assert.AreEqual(10M, testMachine.Balance);

        }


        [TestMethod]


        public void FeedMoneyTest2()
        {
            VendingMachine testMachine = new VendingMachine();
            testMachine.FeedMoney(100);
            Assert.AreEqual(100M, testMachine.Balance);

        }


        [TestMethod]


        public void FeedMoneyTest3()
        {
            VendingMachine testMachine = new VendingMachine();
            testMachine.FeedMoney(5);
            Assert.AreEqual(5M, testMachine.Balance);

        }


        [TestMethod]


        public void FeedMoneyTest4()
        {
            VendingMachine testMachine = new VendingMachine();
            testMachine.FeedMoney(17);
            Assert.AreEqual(17M, testMachine.Balance);

        }
    }
}
