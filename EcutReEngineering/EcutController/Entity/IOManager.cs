using System;

namespace EcutController
{
    internal class IOManager
    {
        internal static UInt64 GetIO(IntPtr cutHandler)
        {
            int IOValue = 0;
            if ((cutHandler.ToInt64() != 0) && (eCutError.eCut_True == eCutDevice.eCutGetInputIO(cutHandler, ref IOValue)))
            {
                return (UInt64)IOValue;
            }
            return 0;
        }

        internal static bool SetIO(IntPtr cutHandler, UInt64 IOOutPut)
        {
            if ((cutHandler.ToInt64() != 0) && (eCutError.eCut_True == eCutDevice.eCutSetOutput(cutHandler, 0, new Int16[9], new UInt16[4] { (UInt16)IOOutPut, 0, 0, 0 })))
            {
                return true;
            }
            return false;
        }
    }
}
