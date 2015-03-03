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
            ecutService = new EcutEntity();
        }

        [TestMethod]
        public void eCutMoveAbsolute()
        {
            var handler = eCutDevice.eCutOpen(0);
            eCutDevice.eCutConfigDeviceDefault(handler);
            eCutDevice.eCutSetStepsPerUnitSmoothCoff(handler, 50, new int[9] { 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000 }, new int[9], 16);
            eCutDevice.eCutSetAccelarationMaxSpeed(handler, new double[9] { 12, 0, 0, 0, 0, 0, 0, 0, 0 }, new double[9] { 50, 0, 0, 0, 0, 0, 0, 0, 0 });
            var doubleArr = new double[9];
            var res = eCutDevice.eCutGetMaxSpeed(handler, doubleArr);
            eCutDevice.eCutMoveAbsolute(handler, 1, new double[4] { 100000, 0, 0, 0 });
            eCutDevice.eCutMoveAbsolute(handler, 1, new double[4] { -100000, 0, 0, 0 });
        }
    }
}
