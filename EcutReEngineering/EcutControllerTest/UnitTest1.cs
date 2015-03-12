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
            //eCutDevice.eCutSetAccelarationMaxSpeed(handler, new double[9] { 12, 0, 0, 0, 0, 0, 0, 0, 0 }, new double[9] { 50, 0, 0, 0, 0, 0, 0, 0, 0 });
            var pos = new eCutPosition() { x = 500, y = 50, z = 50 };
            eCutDevice.eCutAddLine(handler,ref pos, 5.0, 5.0, 5.0);
            eCutDevice.eCutEStop(handler);
        }
    }
}
