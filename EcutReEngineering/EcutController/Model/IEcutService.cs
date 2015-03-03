using System;

namespace EcutController
{
    public interface IEcutService
    {
        /// <summary>
        /// 开启与ECut的通讯
        /// </summary>
        /// <param name="num">第几个Ecut</param>
        /// <returns></returns>
        bool Open(int num);
        void Close();
        bool IsOpen();
        int GetSumNumberOfEcut();
        int[] GetSteps();
        void AddLine(double[] postion, double velocity, double acceleration);
        void AddLineWithCertainPulse(int[] Pulse, double velocity, double acceleration);
        void StopAll();
        void EStop();
        bool StopCertainAxis(int axisNum);
        int? InputIO { get; }
        ushort OutputIO { set; }
        bool SetSoftLimit(double[] maxSoftLimit, double[] minSoftLimit);
        ushort DelayBetweenPulseAndDir { get; set; }
        uint SmoothCoff { get; set; }
        double[] Acceleration { set; }
        double[] MaxSpeed { get; set; }
        int[] StepsPerUnit { get; set; }
        double[] MachinePostion { get; set; }
        byte[] StepPin { get; set; }
        byte[] DirPin { get; set; }
        bool eCutSetInputIOEngineDir(UInt64 InputIOEnable,
                UInt64 InputIONeg, byte[] InputIOPin, SByte[] EngineDirections);
        bool eCutIsDone();
        bool eCutPause();
        bool eCutResume();
        bool eCutAbort();
        int eCutActiveDepth();
        int eCutQueueDepth();
        bool GetDeviceInfo(int taskNumber, byte[] charArray);

        bool eCutMoveAbsolute(ushort AxisMask, double[] doubleArray);

        void setSpindle(ushort taskNumber);

        bool CutStop(ushort taskNumber);

        object MoveAtSpeed(ushort axis, double[] Acceleration, double[] MaxSpeed);

        ushort GetSpindlePostion();

        object SetCurrentPostion(double[] pos);

        object CutAddLine(double[] pos, double vel, double ini_maxvel, double acc);

        object CutAddCircle(double[] pos, double[] center, double[] normal, int turn, double vel, double ini_maxvel, double acc);

        bool SetStopType(eCutStopType type, double tolerance);

        bool eCutSetOutput(UInt16[] DigitalOut);
    }
}
