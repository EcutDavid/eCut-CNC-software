using System;

namespace EcutController
{
    internal class CutConfiguration
    {
        /// <summary>
        /// 获取脉冲细分数
        /// </summary>
        /// <param name="cutHandler">e-Cut资源句柄</param>
        /// <returns>脉冲细分数</returns>
        internal static UInt32 GetSmoothCoff(IntPtr cutHandler)
        {
            UInt32 coffValue = 0;
            if ((cutHandler.ToInt64() != 0) && (eCutError.eCut_True == eCutDevice.eCutGetSmoothCoff(cutHandler, ref coffValue)))
            {
                return coffValue;
            }
            return 0;
        }

        /// <summary>
        /// 获取当前各轴输出脉冲数
        /// </summary>
        /// <param name="cutHandler">e-Cut资源句柄</param>
        /// <returns>各轴输出脉冲数</returns>
        internal static int[] GetStepsPerUnit(IntPtr cutHandler)
        {
            var stepsPerUnit = new int[9];
            if ((cutHandler.ToInt64() != 0) && (eCutError.eCut_True == eCutDevice.eCutGetStepsPerUnit(cutHandler, stepsPerUnit)))
            {
                return stepsPerUnit;
            }
            return null;
        }

        /// <summary>
        /// 设置每MM输出脉冲数，细分数，脉冲方向延时
        /// </summary>
        /// <param name="cutHandler"></param>
        /// <param name="stepsPerUnit">每MM输出脉冲数</param>
        /// <param name="smoothCoff">细分数</param>
        /// <param name="DelayBetweenPulseAndDir">脉冲方向延时</param>
        /// <returns>配置是否成功</returns>
        internal static bool SetStepsPerUnitWithSmoothCoffAndPulseDelay(IntPtr cutHandler, int[] stepsPerUnit, int smoothCoff, UInt16 delayBetweenPulseAndDir)
        {
            if (cutHandler.ToInt64() != 0)
            {
                if (stepsPerUnit == null)
                {
                    stepsPerUnit = new int[9];
                }
                eCutDevice.eCutSetStepsPerUnitSmoothCoff(cutHandler, delayBetweenPulseAndDir, stepsPerUnit, new int[9], smoothCoff);
                return true;
            }
            return false;
        }

        internal static Double[] EcutGetMaxSpeed(IntPtr cutHandler)
        {
            Double[] maxSpeed = new Double[9];
            if ((cutHandler.ToInt64() != 0) && (eCutError.eCut_True == eCutDevice.eCutGetMaxSpeed(cutHandler, maxSpeed)))
            {
                return maxSpeed;
            }
            else
                return null;
        }

        internal static bool EcutSetAccelerationMaxSpeed(IntPtr cutHandler, double[] acceleration, double[] maxSpeed)
        {
            if ((cutHandler.ToInt64() != 0))
            {
                if (eCutError.eCut_True == eCutDevice.eCutSetAccelarationMaxSpeed(cutHandler, acceleration, maxSpeed))
                    return true;
                return false;
            }
            else
                return false;
        }
    }
}
