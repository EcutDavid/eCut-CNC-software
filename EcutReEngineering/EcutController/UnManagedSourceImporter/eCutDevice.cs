using System;
using System.Runtime.InteropServices;

namespace EcutController
{
    public enum eCutStopType
    {
        eCut_Stop_Type_Stop = 0,
        eCut_Stop_Type_Exact = 1,
        eCut_Stop_Type_Parabolic = 2,
        eCut_Stop_Type_Tangent = 3
    }

    internal enum eCutError
    {
        eCut_Error_Ok = 0,
        eCut_Error_NullPointer = 1,
        eCut_Error = 2,
        eCut_True = 3,
        eCut_False = 4,
    }

    public struct eCutPosition
    {
        public double x, y, z;
        public double a, b, c;
        public double u, v, w;
    }

    public struct eCutCartesian
    {
        public double x, y, z;
    }

 
   internal class eCutDevice
    {
        #region  通用功能
        /// <summary>   
        /// 开启与eCut的通讯，获取板卡资源句柄
        /// </summary>
        /// <param name="Num">为0:PC连接的第一个eCut，为1:PC连接的第二个eCut，在PC只与一个eCut相连时，此项配为0即可</param>
        /// <returns>eCut资源句柄</returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr eCutOpen(int Num);

        /// <summary>
        /// 获取主轴脉冲计数值
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="SpindlePostion">主轴脉冲计数值的存储区域</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutGetSpindlePostion(IntPtr eCut, ref ushort SpindlePostion);

        /// <summary>
        /// 板卡轴输出配置
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="StepSel"></param>
        /// <param name="DirSel">轴输出管脚配置 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴 值与管脚的对应需对应参考表</param>
        /// <param name="Enable">轴输出使能配置 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴 值为true则使能</param>
        /// <param name="StepNeg">脉冲反相掩码，0 --> 任何轴脉冲输出不反相， 1 --> X轴脉冲输出反相， 5 --> X与Z轴脉冲输出反相</param>
        /// <param name="DirNeg">方向反相掩码，0 --> 任何轴方向输出不反相， 1 --> X轴方向输出反相， 5 --> X与Z轴方向输出反相</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutSetAxisOutputConfig(IntPtr eCut, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]byte[] StepSel,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]byte[] DirSel,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]bool[] Enable,
                ushort StepNeg,
                ushort DirNeg);

        /// <summary>
        /// 配置输入引脚的特殊功能
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="InputIOEnable">输入引脚使能 bit0--> Input0 bit2--> Input2 bit7--> Input7</param>
        /// <param name="InputIONeg">输入引脚触发信号反相 bit0--> Input0 bit2--> Input2 bit7--> Input7</param>
        /// <param name="InputIOPin">输入引脚映射 0--> Input0 2--> Input2 7--> Input7</param>
        /// <param name="EngineDirections">改变运动方向</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutSetInputIOEngineDir(IntPtr eCut,
                UInt64 InputIOEnable,
                UInt64 InputIONeg,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = 64)]byte[] InputIOPin,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]sbyte[] EngineDirections
            );

        /// <summary>
        /// 获取当前PC检测到的eCut数量
        /// </summary>
        /// <returns>与PC相连的eCut总数</returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetDeviceNum();

        /// <summary>
        /// 获取eCut板卡序列号
        /// </summary>
        /// <param name="Num">为0:PC连接的第一个eCut，为1:PC连接的第二个eCut，在PC只与一个eCut相连时，此项配为0即可</param>
        /// <param name="SerialString">用于存储序列号的字节数组</param>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError GetDeviceInfo(int Num, byte[] SerialString);

        /// <summary>
        /// 断开与eCut的通讯
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutClose(IntPtr eCut);

        /// <summary>
        /// 初始化eCut参数配置
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutConfigDeviceDefault(IntPtr eCut);

        /// <summary>
        /// 获取输入信号
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="Input">用于存储输入信号值的存储区域</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutGetInputIO(IntPtr eCut, ref int Input);

        /// <summary>
        /// 获取eCut各轴相对于机械原点行程的脉冲总量
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="Steps">各轴相对于机械原点的脉冲总量的存储区域</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutGetSteps(IntPtr eCut, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]Int32[] Steps);

        /// <summary>
        /// 设置输出信号
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="Out">bit 0 --> OutPut0 bit 1 --> OutPut1 bit 2 --> OutPut2</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutSetIOOutput(IntPtr eCut, ushort Out);

        /// <summary>
        /// 配置主轴输出
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="Out">主轴输出配置值，最大为65535</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutSetSpindle(IntPtr eCut, ushort Out);


        /// <summary>
        /// 急停
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutEStop(IntPtr eCut);

        /// <summary>
        /// 获取板卡细分数
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="pSmoothCoff">板卡细分数的存储区域</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutGetSmoothCoff(IntPtr eCut, ref UInt32 pSmoothCoff);

        /// <summary>
        /// 获取板卡脉冲每毫米的参数配置值
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="StepsPerUnit">板卡脉冲每毫米的参数配置值的存储区域</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutGetStepsPerUnit(IntPtr eCut, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] int[] StepsPerUnit);

        /// <summary>
        /// 配置板卡脉冲与方向输出的反相,已经删除
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="StepNeg">脉冲反相掩码，0 --> 任何轴脉冲输出不反相， 1 --> X轴脉冲输出反相， 5 --> X与Z轴脉冲输出反相</param>
        /// <param name="DirNeg">方向反相掩码，0 --> 任何轴方向输出不反相， 1 --> X轴方向输出反相， 5 --> X与Z轴方向输出反相</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutSetStepNegAndDirNeg(IntPtr eCut, Byte StepNeg, Byte DirNeg);

        /// <summary>
        /// TODO:删除掉老的SETIO
        /// </summary>
        /// <param name="eCut"></param>
        /// <param name="Sync"></param>
        /// <param name="AnalogOut"></param>
        /// <param name="DigitalOut"></param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutSetOutput(IntPtr eCut, UInt16 Sync,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 20)]Int16[] AnalogOut,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)]UInt16[] DigitalOut);

        /// <summary>
        /// 配置板卡参数：脉冲每毫米，细分数，脉冲方向延时
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="DelayBetweenPulseAndDir">脉冲方向延时，单位us</param>
        /// <param name="StepsPerAxis">脉冲每毫米 0 --> X轴脉冲每毫米 1 --> Y轴脉冲每毫米</param>
        /// <param name="WorkOffset">保留参数，暂不使用</param>
        /// <param name="SmoothCoff">脉冲细分数</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutSetStepsPerUnitSmoothCoff(IntPtr eCut, ushort DelayBetweenPulseAndDir,
        [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]int[] StepsPerAxis,
        [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]int[] WorkOffset, int SmoothCoff);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutSetG92StepDirEncPin(eCutDevice eCut, UInt32[] G92Offset, 
        UInt16 StepNegAndDirNeg, sbyte[] EncoderAPin, byte[] EncoderBPin, byte MPGIndex);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutGetEncoderPostion(eCutDevice eCut, UInt16[] EncoderPostion);

        #endregion

        #region  板卡规划运动
        /// <summary>
        /// 相当于机械坐标的运动
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="AxisMask">运动轴使能掩码，0 --> 任何轴的运动都被禁止， 1 --> X轴运动使能 3 --> X,Y轴运动使能</param>
        /// <param name="PositionGiven">运动目的位置，单位毫米 0 --> X 1 --> Y 2 --> Z 3 --> A</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutMoveAbsolute(IntPtr eCut, ushort AxisMask, [MarshalAs(UnmanagedType.LPArray, SizeConst = 9)] double[] PositionGiven);

        /// <summary>
        /// 配置板卡规划运动的加速度与最大速度
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="Acceleration">加速度 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <param name="MaxSpeed">最大允许速度 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutSetAccelarationMaxSpeed(IntPtr eCut,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]double[] Acceleration,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]double[] MaxSpeed);

        /// <summary>
        /// 距离无限远的运动
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="AxisMask">运动轴使能掩码，0 --> 任何轴的运动都被禁止， 1 --> X轴运动使能 3 --> X,Y轴运动使能</param>
        /// <param name="Acceleration">加速度 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <param name="MaxSpeed">最大允许速度 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutMoveAtSpeed(IntPtr eCut, ushort AxisMask,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]double[] Acceleration,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]double[] MaxSpeed);

        /// <summary>
        /// 软限位配置
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="MaxSoftLimit">允许运动到的最大机械位置 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <param name="MinSoftLimit">允许运动到的最小机械位置 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutSetSoftLimit(IntPtr eCut,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] double[] MaxSoftLimit,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]double[] MinSoftLimit);

        /// <summary>
        /// 获取板卡各轴最大运动速度
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="MaxSpeed">板卡各轴最大运动速度的存储区域，单位毫米每分钟</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]

        internal static extern eCutError eCutGetMaxSpeed(IntPtr eCut, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]Double[] MaxSpeed);
        /// <summary>
        /// 停止由板卡规划的所有轴运动
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutStopAll(IntPtr eCut);


        /// <summary>
        /// 停止由板卡规划的特定的轴的运动,NEED TEST
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="Axis">轴索引 0 --> X轴停止运动， 1 --> Y轴停止运动 2 --> Z轴停止运动</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutStop(IntPtr eCut, ushort Axis);

        /// <summary>
        /// 配置板卡当前的机械坐标
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="Posi">机械坐标，单位毫米 0 --> X 1 --> Y 2 --> Z 3 --> A</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutSetCoordinate(IntPtr eCut, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]double[] Posi);

        /// <summary>
        /// 点动运行，需要调用Stop停止
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="Axis">运动轴 0 --> X轴， 1 --> Y轴 2 --> Z轴</param>
        /// <param name="PositionGiven">运动目的位置，单位毫米 0 --> X 1 --> Y 2 --> Z 3 --> A</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutJogOn(IntPtr eCut, ushort Axis, double[]PositionGiven);
        #endregion

        #region  PC规划运动
        /// <summary>
        /// 恢复经eCutPause停止的运动PC规划运动
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutResume(IntPtr eCut);

        /// <summary>
        /// 停止PC规划运动，可由eCutResume恢复运动状态
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutPause(IntPtr eCut);

        /// <summary>
        /// 终止PC规划运动，不可由eCutResume恢复运动状态
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutAbort(IntPtr eCut);

        /// <summary>
        /// 判断当前的PC规划运动是否结束
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <returns>为3(eCut_True),当前没有任何PC规划运动正在进行，为4(eCut_False)当前有正在运行的PC规划运动</returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutIsDone(IntPtr eCut);

        /// <summary>
        /// 配置当前eCut PC规划运动的工作坐标
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="Pos">期望配置坐标值的位置结构体</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutSetCurrentPostion(IntPtr eCut, ref eCutPosition Pos);//DONE

        /// <summary>
        /// 添加直线插补运动轨迹
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="end">目的终点</param>
        /// <param name="vel">期望直线运动的速度</param>
        /// <param name="ini_maxvel">最大允许速度</param>
        /// <param name="acc">加速度</param>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutAddLine(IntPtr eCut, ref eCutPosition end, double vel, double ini_maxvel, double acc);


        /// <summary>
        /// eCut PC规划添加圆弧插补  由目的终点，圆心，法线确定圆平面
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="end">目的终点</param>
        /// <param name="center">圆心</param>
        /// <param name="normal">法线终点，起点为原点</param>
        /// <param name="turn"></param>
        /// <param name="vel">期望圆弧运动的速度</param>
        /// <param name="ini_maxvel">最大速度</param>
        /// <param name="acc">加速度</param>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutAddCircle(IntPtr eCut,
                ref eCutPosition end,
                ref eCutCartesian center,
                ref eCutCartesian normal,
                int turn,
                double vel,
                double ini_maxvel,
                double acc
                );

        /// <summary>
        /// 正在执行的PC规划指令所占的缓冲区长度
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int eCutActiveDepth(IntPtr eCut);

        /// <summary>
        /// 当前eCut指令缓存区长度
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int eCutQueueDepth(IntPtr eCut);

        /// <summary>
        /// 设置PC规划运动的停止类型
        /// </summary>
        /// <param name="eCut">eCut资源句柄</param>
        /// <param name="type">停止类型 </param>
        /// <param name="tolerance">精度要求，目标位值和当前位值的绝对值小于此数时规划结束了</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern eCutError eCutSetStopType(IntPtr eCut, eCutStopType type, double tolerance);


        #endregion

        #region TEST P/I INVOKE
        //[DllImport("PI.dll", CallingConvention = CallingConvention.Cdecl)]
        //internal static extern int Add(int a, int b);

        //[DllImport("PI.dll", CallingConvention = CallingConvention.Cdecl)]
        //internal static extern void AddWithArray(int[] a, int[] b);

        //[DllImport("PI.dll", CallingConvention = CallingConvention.Cdecl)]
        //internal static extern void AddWithStruct(position pos);

        //[DllImport("PI.dll", CallingConvention = CallingConvention.Cdecl)]
        //internal static extern void ImoprterInit();
        #endregion
    }
}
