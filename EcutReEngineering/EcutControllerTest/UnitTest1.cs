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
        }

        /// <summary>
        /// This test should be run with cut connected
        /// </summary>
        [TestMethod]
        public void eCutOpen()
        {
        }
    }
}
