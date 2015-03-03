using System;

namespace EcutController
{
    internal class IOManager
    {
        internal static int? GetIO(IntPtr cutHandler)
        {
            int IOValue = 0;
            if ((cutHandler.ToInt64() != 0) && (eCutError.eCut_True == eCutDevice.eCutGetInputIO(cutHandler, ref IOValue)))
            {
                return IOValue;
            }
            return null;
        }

        internal static bool SetIO(IntPtr cutHandler, UInt16 IOOutPut)
        {
            if ((cutHandler.ToInt64() != 0) && (eCutError.eCut_True == eCutDevice.eCutSetIOOutput(cutHandler, IOOutPut)))
            {
                return true;
            }
            return false;
        }
    }
}
