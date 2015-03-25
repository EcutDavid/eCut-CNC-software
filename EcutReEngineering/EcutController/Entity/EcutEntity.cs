using System;
using System.Text;

namespace EcutController
{
    internal class EcutEntity : IEcutService
    {
        private int cutIndex;
        private IntPtr eCutHandler;
        private const int NumOfAxis = 4;
        private UInt16 _delayBetweenPulseAndDir;
        private int[] _stepsPerUnit = new int[9];
        private UInt32 _smoothCoff;
        private double[] _acceleration = new double[9];
        private double[] _maxSpeed = new double[9];
        private byte[] _stepPin = new byte[9];
        private byte[] _dirPin = new byte[9];
        private Boolean[] _axisEnable = new Boolean[9] { true, true, true, true, true, true, true, true, true };

        public Boolean[] AxisEnableConfigArr
        {
            get
            {
                return _axisEnable;
            }
            set
            {
                _axisEnable = value;
                ConfigAxis();
            }
        }

        internal EcutEntity(int cutIndex)
        {
            this.cutIndex = cutIndex;
        }

        private void CheckCutHasOpen()
        {
            if (eCutHandler.ToInt64() == 0)
                throw new CutNotOpenException();
        }

        private void CheckCutExist(int num)
        {
            if ((CutUtility.GetConnectedCutNum() - 1) < num)
                throw new CutNotExistException();
        }

        public bool OpenCut()
        {
            CheckCutExist(this.cutIndex);
            if (eCutHandler.ToInt64() == 0)
            {
                eCutHandler = eCutDevice.eCutOpen(this.cutIndex);
                if (eCutHandler == null)
                    return false;
                eCutDevice.eCutConfigDeviceDefault(eCutHandler);
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



        public int[] CutGetSteps()
        {
            CheckCutHasOpen();
            int[] steps = new int[6];
            eCutDevice.eCutGetSteps(eCutHandler, steps);
            return steps;
        }


        public void HWPlanMovementStop()
        {
            CheckCutHasOpen();
            var result = eCutDevice.eCutStopAll(eCutHandler);
        }

        public void CutEStop()
        {
            CheckCutHasOpen();

            var result = eCutDevice.eCutEStop(eCutHandler);
        }


        public ushort CutDelayBetweenPulseAndDir
        {
            get
            {
                return _delayBetweenPulseAndDir;
            }
            set
            {
                CheckCutHasOpen();
                _delayBetweenPulseAndDir = value;
                var result = MoveManagerHardwareRelated.SetStepsPerUnit(eCutHandler, _stepsPerUnit, (int)_smoothCoff, _delayBetweenPulseAndDir);
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
                CheckCutHasOpen();
                _smoothCoff = value;
                var result = CutConfiguration.SetStepsPerUnitWithSmoothCoffAndPulseDelay(eCutHandler, _stepsPerUnit, (int)_smoothCoff, _delayBetweenPulseAndDir);
            }
        }

        public double[] HWPlanMovementAccConfigArr
        {
            set
            {
                CheckCutHasOpen();
                _acceleration = value;
                var result = CutConfiguration.EcutSetAccelerationMaxSpeed(eCutHandler, _acceleration, _maxSpeed);
            }
        }

        public double[] HWPlanMovementMaxSpeedConfigArr
        {
            get
            {
                return CutConfiguration.EcutGetMaxSpeed(eCutHandler);
            }
            set
            {
                CheckCutHasOpen();
                _maxSpeed = value;
                var result = CutConfiguration.EcutSetAccelerationMaxSpeed(eCutHandler, _acceleration, _maxSpeed);
            }
        }

        public int[] CutStepsPerUnitConfigArr
        {
            get
            {
                return (eCutHandler.ToInt64() != 0) ? MoveManagerHardwareRelated.GetStepsPerUnit(eCutHandler) : null;
            }
            set
            {
                CheckCutHasOpen();
                _stepsPerUnit = value;
                var result = MoveManagerHardwareRelated.SetStepsPerUnit(eCutHandler, _stepsPerUnit, (int)_smoothCoff, _delayBetweenPulseAndDir);
            }
        }

        public double[] CutMachinePostionArr
        {
            get
            {
                var doubleArray = new double[4];
                if ((eCutHandler.ToInt64()) == 0)
                    return doubleArray;
                var stepNumer = CutGetSteps();
                var stepsPerUnit = CutStepsPerUnitConfigArr;
                var smooth = (double)CutSmoothCoff;

                for (int i = 0; i < 4; i++)
                    doubleArray[i] = stepNumer[i] / smooth / stepsPerUnit[i];

                return doubleArray;
            }
            set
            {
                CheckCutHasOpen();
                if (value == null)
                    value = new double[9];
                var result = eCutDevice.eCutSetCoordinate(eCutHandler, value);
            }
        }

        public UInt64 CutGeneralInputMask
        {
            get
            {
                CheckCutHasOpen();
                return IOManager.GetIO(eCutHandler);
            }
        }

        public UInt64 CutGeneralOutputIOMask
        {
            set
            {
                CheckCutHasOpen();
                var result = IOManager.SetIO(eCutHandler, value);
            }
        }


        public byte[] CutStepSigPinConfigArr
        {
            get
            {
                return _stepPin;
            }
            set
            {
                CheckCutHasOpen();
                _stepPin = value;
                ConfigAxis();
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
                CheckCutHasOpen();
                stepNeg = value;
                ConfigAxis();
            }
        }

        /// <summary>
        /// TODO：增加默认DIGIO 配IO的配置
        /// </summary>
        private void ConfigAxis()
        {
            var result = eCutDevice.eCutSetAxisOutputConfig(eCutHandler, _stepPin, _dirPin, _axisEnable, stepNeg, dirNeg);
        }

        private ushort dirNeg;
        public ushort CutDirNegMask
        {
            get { return dirNeg; }
            set
            {
                CheckCutHasOpen();
                dirNeg = value;
                ConfigAxis();
            }
        }

        public byte[] CutDirSigPinConfigArr
        {
            get
            {
                return _dirPin;
            }
            set
            {
                CheckCutHasOpen();
                _dirPin = value;
                ConfigAxis();
            }
        }


        public void HWPlanMovementSetSoftLimit(double[] maxSoftLimit, double[] minSoftLimit)
        {
            CheckCutHasOpen();
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

            MoveManagerSoftwareRelated.SetSoftLimit(eCutHandler, maxSoftLimitArray, minSoftLimitArray);
        }

        public bool PCPlanMovementIsDone()
        {
            CheckCutHasOpen();
            var result = eCutDevice.eCutIsDone(eCutHandler);
            return (eCutError.eCut_Error_Ok == result || eCutError.eCut_True == result);
        }

        public void PCPlanMovementPause()
        {
            CheckCutHasOpen();
            var result = eCutDevice.eCutPause(eCutHandler);
            eCutDevice.eCutPause(eCutHandler);
        }

        public void PCPlanMovementResume()
        {
            CheckCutHasOpen();
            eCutDevice.eCutResume(eCutHandler);
        }

        public void PCPlanMovementAbort()
        {
            CheckCutHasOpen();
            eCutDevice.eCutAbort(eCutHandler);
        }

        public int PCPlanMovementActiveDepth
        {
            get
            {
                CheckCutHasOpen();
                return eCutDevice.eCutActiveDepth(eCutHandler);
            }

        }

        public int PCPlanMovementQueueDepth
        {
            get
            {
                CheckCutHasOpen();
                return eCutDevice.eCutQueueDepth(eCutHandler);
            }
        }

        public void CutSetInputIOEngineDir(ulong InputIOEnable, ulong InputIONeg, byte[] InputIOPin)
        {
            CheckCutHasOpen();
            eCutDevice.eCutSetInputIOEngineDir(eCutHandler, InputIOEnable, InputIONeg, InputIOPin, new sbyte[9]);
        }


        public void HWPlanMovementMove(ushort AxisMask, double[] doubleArray)
        {
            CheckCutHasOpen();
            eCutDevice.eCutMoveAbsolute(eCutHandler, AxisMask, doubleArray);
        }

        //TODO:测试该函数
        public void CutSetSpindle(ushort taskNumber)
        {
            CheckCutHasOpen();
            var result = eCutDevice.eCutSetSpindle(eCutHandler, taskNumber);
        }

        public void HWPlanMovementStop(ushort taskNumber)
        {
            CheckCutHasOpen();
            eCutDevice.eCutStop(eCutHandler, taskNumber);
        }


        public void PCPlanMovementSetCurrentPostion(double[] pos)
        {
            CheckCutHasOpen();
            var ecutPos = new eCutPosition();
            ecutPos.x = pos[0];
            ecutPos.y = pos[1];
            ecutPos.z = pos[2];
            eCutDevice.eCutSetCurrentPostion(eCutHandler, ref ecutPos);
        }

        public void PCPlanMovementAddLine(double[] pos, double vel, double ini_maxvel, double acc)
        {
            CheckCutHasOpen();
            var ecutPos = new eCutPosition();
            ecutPos.x = pos[0];
            ecutPos.y = pos[1];
            ecutPos.z = pos[2];
            eCutDevice.eCutAddLine(eCutHandler, ref ecutPos, vel, ini_maxvel, acc);
        }

        public void PCPlanMovementAddCircle(double[] pos, double[] center, double[] normal, int turn, double vel, double ini_maxvel, double acc)
        {
            CheckCutHasOpen();
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

        public void PCPlanMovementSetStopType(eCutStopType type, double tolerance)
        {
            CheckCutHasOpen();
            eCutDevice.eCutSetStopType(eCutHandler, type, tolerance);
        }

        public void HWPlanMovementMove(ushort Axis, double pos)
        {
            CheckCutHasOpen();
            var doubleArray = new double[9];
            doubleArray[Axis] = pos;
            eCutDevice.eCutJogOn(eCutHandler, Axis, doubleArray);
        }
    }
}
