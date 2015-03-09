using System;

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

        public EcutEntity()
        {
            eCutDevice.ImoprterInit();
        }

        public bool Open(int num)
        {
            if (eCutHandler.ToInt64() == 0)
            {
                eCutHandler = eCutDevice.eCutOpen(num - 1);
                if (eCutHandler == null)
                    return false;
                var res = eCutDevice.eCutConfigDeviceDefault(eCutHandler);
            }
            return true;
        }

        public void Close()
        {
            if (eCutHandler.ToInt64() != 0)
            {
                //no movement after close comunication
                eCutDevice.eCutEStop(eCutHandler);
                eCutDevice.eCutClose(eCutHandler);
                eCutHandler = (IntPtr)0;
            }
        }

        public bool IsOpen()
        {
            return (eCutHandler.ToInt64() != 0);
        }

        public int GetSumNumberOfEcut()
        {
            return eCutDevice.GetDeviceNum();
        }

        public int[] GetSteps()
        {
            int[] steps = new int[6];
            if ((eCutHandler.ToInt64()) != 0 && (eCutError.eCut_True == eCutDevice.eCutGetSteps(eCutHandler, steps)))
                return steps;
            return null;
        }

        public void AddLine(double[] postion, double velocity, double acceleration)
        {
            eCutPosition pos = new eCutPosition();
            pos.x = pos.y = pos.z = pos.a = 0;
            //eCutDevice.eCutSetCurrentPostion(eCutHandler, ref pos);

            if (postion != null)
            {
                pos.x = postion[0];
                pos.y = postion[1];
                pos.z = postion[2];
                pos.a = postion[3];
            }

            eCutDevice.eCutAddLine(eCutHandler, ref pos, velocity, velocity, acceleration);
        }

        public void AddLineWithCertainPulse(int[] Pulse, double velocity, double acceleration)
        {
            throw new NotImplementedException();
        }

        public void StopAll()
        {
            var result = eCutDevice.eCutStopAll(eCutHandler);
        }

        public void EStop()
        {
            var result = eCutDevice.eCutEStop(eCutHandler);
        }

        public bool StopCertainAxis(int axisNum)
        {
            throw new NotImplementedException();
        }

        public ushort DelayBetweenPulseAndDir
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

        public uint SmoothCoff
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

        public double[] Acceleration
        {
            set
            {
                _acceleration = value;
                CutConfiguration.EcutSetAccelerationMaxSpeed(eCutHandler, _acceleration, _maxSpeed);
            }
        }

        public double[] MaxSpeed
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

        public int[] StepsPerUnit
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

        public double[] MachinePostion
        {
            get
            {
                var doubleArray = new double[4];
                if ((eCutHandler.ToInt64()) == 0)
                    return doubleArray;
                var stepNumer = GetSteps();
                var stepsPerUnit = StepsPerUnit;
                var smooth = (double)SmoothCoff;

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


        public int? InputIO
        {
            get
            {
                return IOManager.GetIO(eCutHandler);
            }
        }

        public ushort OutputIO
        {
            set
            {
                IOManager.SetIO(eCutHandler, value);
            }
        }


        public byte[] StepPin
        {
            get
            {
                return _stepPin;
            }
            set
            {
                _stepPin = value;
                //eCutDevice.eCutSetAxisOutputConfig(eCutHandler, _stepPin, _dirPin, new bool[9] { true, true, true, true, true, true, true, true, true }, stepNeg, dirNeg);
            }
        }

        private ushort stepNeg;
        public ushort StepNeg
        {
            get 
            {
                return stepNeg; 
            }
            set 
            {
                stepNeg = value;
                //eCutDevice.eCutSetAxisOutputConfig(eCutHandler, _stepPin, _dirPin, new bool[9] { true, true, true, true, true, true, true, true, true },stepNeg, dirNeg);
            }
        }

        private ushort dirNeg;
        public ushort DirNeg
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
        

        public byte[] DirPin
        {
            get
            {
                return _dirPin;
            }
            set 
            {
                _dirPin = value;
                //eCutDevice.eCutSetAxisOutputConfig(eCutHandler, _stepPin, _dirPin, new bool[9] { true, true, true, true, true, true, true, true, true }, stepNeg, dirNeg);
            }
        }


        public bool SetSoftLimit(double[] maxSoftLimit, double[] minSoftLimit)
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

        public bool eCutIsDone()
        {
            var result = eCutDevice.eCutIsDone(eCutHandler);
            return eCutError.eCut_True == eCutDevice.eCutIsDone(eCutHandler);
        }

        public bool eCutPause()
        {
            var result = eCutDevice.eCutPause(eCutHandler);
            return eCutError.eCut_Error_Ok == eCutDevice.eCutPause(eCutHandler);
        }

        public bool eCutResume()
        {
            return eCutError.eCut_Error_Ok == eCutDevice.eCutResume(eCutHandler);
        }

        public bool eCutAbort()
        {
            return eCutError.eCut_Error_Ok == eCutDevice.eCutAbort(eCutHandler);
        }

        public int eCutActiveDepth()
        {
            return eCutDevice.eCutActiveDepth(eCutHandler);
        }

        public int eCutQueueDepth()
        {
            return eCutDevice.eCutQueueDepth(eCutHandler);
        }

        public bool eCutSetInputIOEngineDir(ulong InputIOEnable, ulong InputIONeg, byte[] InputIOPin, sbyte[] EngineDirections)
        {
            return eCutError.eCut_True == eCutDevice.eCutSetInputIOEngineDir(eCutHandler, InputIOEnable, InputIONeg, InputIOPin, EngineDirections);
        }

        public bool GetDeviceInfo(int taskNumber, byte[] charArray)
        {
            return eCutError.eCut_True == eCutDevice.GetDeviceInfo(taskNumber, charArray);
        }

        public bool eCutMoveAbsolute(ushort AxisMask, double[] doubleArray)
        {
            return eCutError.eCut_True == eCutDevice.eCutMoveAbsolute(eCutHandler, AxisMask, doubleArray);
        }

        //TODO:测试该函数
        public void setSpindle(ushort taskNumber)
        {
            var result = eCutDevice.eCutSetSpindle(eCutHandler, taskNumber);
        }

        public bool CutStop(ushort taskNumber)
        {
            return eCutError.eCut_True == eCutDevice.eCutStop(eCutHandler, taskNumber);
        }

        public object MoveAtSpeed(ushort axis, double[] Acceleration, double[] MaxSpeed)
        {
            return eCutDevice.eCutMoveAtSpeed(eCutHandler, axis, Acceleration, MaxSpeed);
        }

        //TODO:测试该函数
        public ushort GetSpindlePostion()
        {
            ushort pos = 0;
            eCutDevice.eCutGetSpindlePostion(eCutHandler, ref pos);
            return pos;
        }

        public object SetCurrentPostion(double[] pos)
        {
            var ecutPos = new eCutPosition();
            ecutPos.x = pos[0];
            ecutPos.y = pos[1];
            ecutPos.z = pos[2];
            return eCutError.eCut_True == eCutDevice.eCutSetCurrentPostion(eCutHandler, ref ecutPos);
        }

        public object CutAddLine(double[] pos, double vel, double ini_maxvel, double acc)
        {
            var ecutPos = new eCutPosition();
            ecutPos.x = pos[0];
            ecutPos.y = pos[1];
            ecutPos.z = pos[2];
            return eCutError.eCut_True == eCutDevice.eCutAddLine(eCutHandler, ref ecutPos, vel, ini_maxvel, acc);
        }


        public object CutAddCircle(double[] pos, double[] center, double[] normal, int turn, double vel, double ini_maxvel, double acc)
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
            return eCutError.eCut_Error_Ok == result;
        }

        public bool SetStopType(eCutStopType type, double tolerance)
        {
            return eCutError.eCut_Error_Ok == eCutDevice.eCutSetStopType(eCutHandler, type, tolerance);
        }

        //TODO:测试该函数
        public bool eCutSetOutput(ushort[] DigitalOut)
        {
            var digtalOutArray = new ushort[20];
            var analogOutArray = new short[20];
            
            if(DigitalOut != null)
            {
                for (int i = 0; i < DigitalOut.Length; i++)
                {
                    digtalOutArray[i] = DigitalOut[i];
                }
            }
            var res = eCutDevice.eCutSetOutput(eCutHandler, 0, analogOutArray, digtalOutArray);
            return true;
        }

        public bool eCutJogOn(ushort Axis, double[] doubleArray)
        {
            return eCutError.eCut_True == eCutDevice.eCutJogOn(eCutHandler, Axis, doubleArray);
        }
    }
}
