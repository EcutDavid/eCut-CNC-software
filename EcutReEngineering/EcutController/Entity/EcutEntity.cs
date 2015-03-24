using System;
using System.Text;

namespace EcutController
{
    public class EcutEntity : IEcutService
    {
        private IntPtr eCutHandler;
        private const int NumOfAxis = 4;
        private UInt16 _delayBetweenPulseAndDir;
        private int[] _stepsPerUnit = new int[9];
        private UInt32 _smoothCoff;
        private double[] _acceleration = new double[9];
        private double[] _maxSpeed = new double[9];
        private byte[] _stepPin = new byte[8];
        private byte[] _dirPin = new byte[8];

        private void CheckCutHasOpen()
        {
            //THROW EXCEPTIUON HERE
        }

        private void CheckCutExist()
        { 
        
        }

        public static IEcutService GetCutServiceInstance()
        { 
            return new EcutEntity();
        }

        private EcutEntity(){}

        public bool OpenCut(int num)
        {
            if (eCutHandler.ToInt64() == 0)
            {
                eCutHandler = eCutDevice.eCutOpen(num);
                if (eCutHandler == null)
                    return false;
                var res = eCutDevice.eCutConfigDeviceDefault(eCutHandler);
            }
            return true;
        }

        public void CloseCut()
        {
            if (eCutHandler.ToInt64() != 0)
            {
                eCutDevice.eCutEStop(eCutHandler);
                eCutDevice.eCutClose(eCutHandler);
                eCutHandler = (IntPtr)0;
            }
        }

        public bool CutIsOpen()
        {
            return (eCutHandler.ToInt64() != 0);
        }

        public int GetConnectedCutNum()
        {
            return eCutDevice.GetDeviceNum();
        }

        public int[] CutGetSteps()
        {
            int[] steps = new int[6];
            if ((eCutHandler.ToInt64()) != 0 && (eCutError.eCut_True == eCutDevice.eCutGetSteps(eCutHandler, steps)))
                return steps;
            return null;
        }
        

        public void HWPlanMovementStop()
        {
            var result = eCutDevice.eCutStopAll(eCutHandler);
        }

        public void CutEStop()
        {
            if(eCutHandler.ToInt64() != 0)
                eCutDevice.eCutEStop(eCutHandler);
        }


        public ushort CutDelayBetweenPulseAndDir
        {
            get
            {
                return _delayBetweenPulseAndDir;
            }
            set
            {
                _delayBetweenPulseAndDir = value;
                MoveManagerHardwareRelated.SetStepsPerUnit(eCutHandler, _stepsPerUnit, (int)_smoothCoff, _delayBetweenPulseAndDir);
            }
        }

        public uint CutSmoothCoff
        {
            get
            {
                return (eCutHandler.ToInt64() != 0) ? CutConfiguration.GetSmoothCoff(eCutHandler) : 0;
            }
            set
            {
                if (eCutHandler.ToInt64() != 0)
                {
                    _smoothCoff = value;
                    CutConfiguration.SetStepsPerUnitWithSmoothCoffAndPulseDelay(eCutHandler, _stepsPerUnit, (int)_smoothCoff, _delayBetweenPulseAndDir);
                }
            }
        }

        public double[] HWPlanMovementAcceleration
        {
            set
            {
                _acceleration = value;
                CutConfiguration.EcutSetAccelerationMaxSpeed(eCutHandler, _acceleration, _maxSpeed);
            }
        }

        public double[] HWPlanMovementMaxSpeed
        {
            get
            {
                return CutConfiguration.EcutGetMaxSpeed(eCutHandler);
            }
            set
            {
                _maxSpeed = value;
                CutConfiguration.EcutSetAccelerationMaxSpeed(eCutHandler, _acceleration, _maxSpeed);
            }
        }

        public int[] CutStepsPerUnit
        {
            get
            {
                return (eCutHandler.ToInt64() != 0) ? MoveManagerHardwareRelated.GetStepsPerUnit(eCutHandler) : null;
            }
            set
            {
                _stepsPerUnit = value;
                MoveManagerHardwareRelated.SetStepsPerUnit(eCutHandler, _stepsPerUnit, (int)_smoothCoff, _delayBetweenPulseAndDir);
            }
        }

        public double[] CutMachinePostion
        {
            get
            {
                var doubleArray = new double[4];
                if ((eCutHandler.ToInt64()) == 0)
                    return doubleArray;
                var stepNumer = CutGetSteps();
                var stepsPerUnit = CutStepsPerUnit;
                var smooth = (double)CutSmoothCoff;

                for (int i = 0; i < 4; i++)
                    doubleArray[i] = stepNumer[i] / smooth / stepsPerUnit[i];

                return doubleArray;
            }
            set
            {
                if (value == null)
                    value = new double[9];
                eCutDevice.eCutSetCoordinate(eCutHandler, value);
            }
        }


        public UInt64 CutGeneralInputMask
        {
            get
            {
                return IOManager.GetIO(eCutHandler);
            }
        }

        public UInt64 CutGeneralOutputIOMask
        {
            set
            {
                IOManager.SetIO(eCutHandler, value);
            }
        }


        public byte[] CutStepPin
        {
            get
            {
                return _stepPin;
            }
            set
            {
                _stepPin = value;
                eCutDevice.eCutSetAxisOutputConfig(eCutHandler, _stepPin, _dirPin, new bool[9] { true, true, true, true, true, true, true, true, true }, stepNeg, dirNeg);
             }
        }

        private ushort stepNeg;
        public ushort CutStepNegMask
        {
            get 
            {
                return stepNeg; 
            }
            set 
            {
                stepNeg = value;
                eCutDevice.eCutSetAxisOutputConfig(eCutHandler, _stepPin, _dirPin, new bool[9] { true, true, true, true, true, true, true, true, true }, stepNeg, dirNeg);
            }
        }

        private ushort dirNeg;
        public ushort CutDirNegMask
        {
            get
            {
                return dirNeg;
            }
            set
            {
                dirNeg = value;
                eCutDevice.eCutSetAxisOutputConfig(eCutHandler, _stepPin, _dirPin, 
                    new bool[9] { true, true, true, true, true, true, true, true, true }, stepNeg, dirNeg);
            }
        }
        

        public byte[] CutDirPin
        {
            get
            {
                return _dirPin;
            }
            set 
            {
                _dirPin = value;
                eCutDevice.eCutSetAxisOutputConfig(eCutHandler, _stepPin, _dirPin, new bool[9] { true, true, true, true, true, true, true, true, true }, stepNeg, dirNeg);
            }
        }


        public bool HWPlanMovementSetSoftLimit(double[] maxSoftLimit, double[] minSoftLimit)
        {
            var maxSoftLimitArray = new double[9];
            var minSoftLimitArray = new double[9];
            if (maxSoftLimit != null)
            {
                for (int i = 0; i < maxSoftLimit.Length; i++)
                {
                    maxSoftLimitArray[i] = maxSoftLimit[i];
                }
            }

            if (minSoftLimit != null)
            {
                for (int i = 0; i < minSoftLimit.Length; i++)
                {
                    minSoftLimitArray[i] = minSoftLimit[i];
                }
            }

            return MoveManagerSoftwareRelated.SetSoftLimit(eCutHandler, maxSoftLimitArray, minSoftLimitArray);
        }

        public bool PCPlanMovementIsDone()
        {
            var result = eCutDevice.eCutIsDone(eCutHandler);
            return eCutError.eCut_True == eCutDevice.eCutIsDone(eCutHandler);
        }

        public bool PCPlanMovementPause()
        {
            var result = eCutDevice.eCutPause(eCutHandler);
            return eCutError.eCut_Error_Ok == eCutDevice.eCutPause(eCutHandler);
        }

        public bool PCPlanMovementResume()
        {
            return eCutError.eCut_Error_Ok == eCutDevice.eCutResume(eCutHandler);
        }

        public bool PCPlanMovementAbort()
        {
            return eCutError.eCut_Error_Ok == eCutDevice.eCutAbort(eCutHandler);
        }

        public int PCPlanMovementActiveDepth()
        {
            return eCutDevice.eCutActiveDepth(eCutHandler);
        }

        public int PCPlanMovementQueueDepth()
        {
            return eCutDevice.eCutQueueDepth(eCutHandler);
        }

        public bool CutSetInputIOEngineDir(ulong InputIOEnable, ulong InputIONeg, byte[] InputIOPin)
        {
            return eCutError.eCut_True == eCutDevice.eCutSetInputIOEngineDir(eCutHandler, InputIOEnable, InputIONeg, InputIOPin, new sbyte[9]);
        }

        public String GetCutInfo(int taskNumber)
        {
            var charArray = new byte[12];
            eCutDevice.GetDeviceInfo(taskNumber, charArray);
            return Encoding.GetEncoding("GB2312").GetString(charArray, 0, charArray.Length).ToString(); 
        }

        public bool HWPlanMovementMove(ushort AxisMask, double[] doubleArray)
        {
            return eCutError.eCut_True == eCutDevice.eCutMoveAbsolute(eCutHandler, AxisMask, doubleArray);
        }

        //TODO:测试该函数
        public void CutSetSpindle(ushort taskNumber)
        {
            var result = eCutDevice.eCutSetSpindle(eCutHandler, taskNumber);
        }

        public bool HWPlanMovementStop(ushort taskNumber)
        {
            return eCutError.eCut_True == eCutDevice.eCutStop(eCutHandler, taskNumber);
        }


        public void PCPlanMovementSetCurrentPostion(double[] pos)
        {
            var ecutPos = new eCutPosition();
            ecutPos.x = pos[0];
            ecutPos.y = pos[1];
            ecutPos.z = pos[2];
            eCutDevice.eCutSetCurrentPostion(eCutHandler, ref ecutPos);
        }

        public void PCPlanMovementAddLine(double[] pos, double vel, double ini_maxvel, double acc)
        {
            var ecutPos = new eCutPosition();
            ecutPos.x = pos[0];
            ecutPos.y = pos[1];
            ecutPos.z = pos[2];
           eCutDevice.eCutAddLine(eCutHandler, ref ecutPos, vel, ini_maxvel, acc);
        }

        public void PCPlanMovementAddCircle(double[] pos, double[] center, double[] normal, int turn, double vel, double ini_maxvel, double acc)
        {
            var ecutPos = new eCutPosition();
            ecutPos.x = pos[0];
            ecutPos.y = pos[1];
            ecutPos.z = pos[2];
            var ecutcenter = new eCutCartesian();
            ecutcenter.x = center[0];
            ecutcenter.y = center[1];
            ecutcenter.z = center[2];
            var ecutnormal = new eCutCartesian();
            ecutnormal.x = normal[0];
            ecutnormal.y = normal[1];
            ecutnormal.z = normal[2];
            var result = eCutDevice.eCutAddCircle(eCutHandler, ref ecutPos, ref ecutcenter, ref ecutnormal, turn, vel, ini_maxvel, acc);
        }

        public bool PCPlanMovementSetStopType(eCutStopType type, double tolerance)
        {
            return eCutError.eCut_Error_Ok == eCutDevice.eCutSetStopType(eCutHandler, type, tolerance);
        }


        public bool HWPlanMovementMove(ushort Axis, double pos)
        {
            var doubleArray = new double[9];
            doubleArray[Axis] = pos;
            if (eCutHandler.ToInt64() == 0)
                return false;
            return eCutError.eCut_True == eCutDevice.eCutJogOn(eCutHandler, Axis, doubleArray);
        }
        
    }
}
