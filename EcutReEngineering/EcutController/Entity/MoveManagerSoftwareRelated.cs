using System;

namespace EcutController
{
    internal class MoveManagerSoftwareRelated
    {
        internal static bool SetSoftLimit(IntPtr cutHandler, double[] MaxSoftLimit, double[] MinSoftLimit)
        {
            if (eCutError.eCut_True == eCutDevice.eCutSetSoftLimit(cutHandler, MaxSoftLimit, MinSoftLimit))
                return true;
            else
                return false;
        }
    }
}
