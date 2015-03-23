using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcutControllerTest;
using EcutController;

namespace EcutControllerTest
{
    [TestClass]
    public class UnitTest1
    {
        public IntPtr eCutHandler;
        IEcutService ecutService;

        [TestInitialize]
        public void TestInit()
        {
            ecutService = EcutEntity.GetCutServiceInstance();
        }

        /// <summary>
        /// This test should be run with cut connected
        /// </summary>
        [TestMethod]
        public void eCutOpen()
        {
            var handler = eCutDevice.eCutOpen(0);
            Assert.AreNotEqual(0, handler.ToInt32());
        }
    }
}
