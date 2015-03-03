using System;

namespace EcutController
{
    internal class MoveManagerHardwareRelated
    {
        internal static int[] GetStepsPerUnit(IntPtr cutHandler)
        {
            int[] stepsPerUnit = new int[9];
            if ((cutHandler.ToInt64() != 0) && (eCutError.eCut_True == eCutDevice.eCutGetStepsPerUnit(cutHandler, stepsPerUnit)))
            {
                return stepsPerUnit;
            }
            return null;
        }

        internal static bool SetStepsPerUnit(IntPtr cutHandler, int[] stepsPerUnit, int smoothCoff, UInt16 DelayBetweenPulseAndDir)
        {
            if (eCutError.eCut_True == eCutDevice.eCutSetStepsPerUnitSmoothCoff(cutHandler, DelayBetweenPulseAndDir, stepsPerUnit, new int[9], smoothCoff))
                return true;
            else
                return false;
        }

        internal static bool eCutSetSpindle(IntPtr cutHandler, UInt16 spindleValue) 
        {
            if (eCutError.eCut_True == eCutDevice.eCutSetSpindle(cutHandler, spindleValue))
                return true;
            else
                return false;
        }

        internal static bool MoveAbsolute(IntPtr cutHandler, UInt16 axisMask, Double[] positionGiven)
        {
            if (cutHandler.ToInt64() != 0)
            {
                if (eCutError.eCut_True == eCutDevice.eCutMoveAbsolute(cutHandler, axisMask, positionGiven))
                    return true;
            }
            return false;
        }
    }
}
