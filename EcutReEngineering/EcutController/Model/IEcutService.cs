using System;

namespace EcutController
{
    public interface IEcutService
    {
        #region Common
        /// <summary>
        /// 获取与PC连接的板卡数量
        /// </summary>
        /// <returns>与PC连接的板卡数量</returns>
        Int32 GetConnectedCutNum();

        /// <summary>
        /// 开启与特定板卡的通讯，获取板卡资源句柄
        /// </summary>
        /// <param name="cutIndex">0:PC连接的第一个eCut，为1:PC连接的第二个eCut，在PC只与一个eCut相连时，此项配为0即可</param>
        /// <returns></returns>
        Boolean OpenCut(Int32 cutIndex);

        /// <summary>
        /// 关闭与板卡的通讯
        /// </summary>
        void CloseCut();

        /// <summary>
        /// 获取板卡序列号
        /// </summary>
        /// <param name="cutIndex"></param>
        /// <returns>序列号</returns>
        String GetCutInfo(Int32 cutIndex);

        /// <summary>
        /// 判断当前板卡是否已经开启通讯
        /// </summary>
        /// <returns>true : 已开启</returns>
        Boolean CutIsOpen();

        /// <summary>
        /// 获取各轴相对于机械原点行程的脉冲总量
        /// </summary>
        /// <returns>各轴相对于机械原点行程的脉冲总量</returns>
        Int32[] CutGetSteps();

        /// <summary>
        /// 急停
        /// </summary>
        void CutEStop();

        /// <summary>
        /// 获取IO输入值，掩码形式
        /// </summary>
        UInt64 CutGeneralInputMask { get; }

        /// <summary>
        /// 掩码形式配置输出IO
        /// </summary>
        UInt64 CutGeneralOutputIOMask { set; }

        /// <summary>
        /// 脉冲方向延时，单位us
        /// </summary>
        UInt16 CutDelayBetweenPulseAndDir { get; set; }

        /// <summary>
        /// 脉冲细分数
        /// </summary>
        UInt32 CutSmoothCoff { get; set; }

        /// <summary>
        /// 脉冲反相掩码，0 --> 任何轴脉冲输出不反相， 1 --> X轴脉冲输出反相， 5 --> X与Z轴脉冲输出反相
        /// </summary>
        UInt16 CutStepNegMask { get; set; }

        /// <summary>
        /// 方向反相掩码
        /// </summary>
        UInt16 CutDirNegMask { get; set; }


        Byte[] CutStepPin { get; set; }

        Byte[] CutDirPin { get; set; }

        /// <summary>
        /// 配置主轴输出
        /// </summary>
        /// <param name="outputVal">主轴输出配置值，最大为65535</param>
        void CutSetSpindle(UInt16 outputVal);

        /// <summary>
        /// 各个轴脉冲每毫米
        /// </summary>
        Int32[] CutStepsPerUnit { get; set; }

        /// <summary>
        /// 机械坐标
        /// </summary>
        Double[] CutMachinePostion { get; set; }

        /// <summary>
        /// TODO 配置特殊功能
        /// </summary>
        /// <param name="InputIOEnable"></param>
        /// <param name="InputIONeg"></param>
        /// <param name="InputIOPin"></param>
        /// <returns></returns>
        Boolean CutSetInputIOEngineDir(UInt64 InputIOEnable,UInt64 InputIONeg, Byte[] InputIOPin);
        #endregion

        #region HWPlanMovement
        /// <summary>
        /// 配置软限位
        /// </summary>
        /// <param name="maxSoftLimit"></param>
        /// <param name="minSoftLimit"></param>
        /// <returns></returns>
        Boolean HWPlanMovementSetSoftLimit(Double[] maxSoftLimit, Double[] minSoftLimit);
        
        /// <summary>
        /// 配置运动加速度
        /// </summary>
        Double[] HWPlanMovementAcceleration { set; }
        
        /// <summary>
        /// 配置运动最大速度
        /// </summary>
        Double[] HWPlanMovementMaxSpeed { get; set; }
        
        /// <summary>
        /// 多轴点动运动 参考坐标系为机械坐标系
        /// </summary>
        /// <param name="AxisMask"></param>
        /// <param name="doubleArray"></param>
        /// <returns></returns>
        Boolean HWPlanMovementMove(UInt16 AxisMask, Double[] doubleArray);
        
        /// <summary>
        /// 单轴点动运动 参考坐标系为机械坐标系
        /// </summary>
        /// <param name="AxisIndex"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        Boolean HWPlanMovementMove(UInt16 AxisIndex, Double pos);

        /// <summary>
        /// 停止特定轴的运动
        /// </summary>
        /// <param name="AxisIndex">轴索引</param>
        /// <returns></returns>
        Boolean HWPlanMovementStop(UInt16 AxisIndex);
        
        /// <summary>
        /// 停止所有轴的运动
        /// </summary>
        void HWPlanMovementStop();
        #endregion

        #region PCPlanMovement
        /// <summary>
        /// 判断当前的PC规划运动是否结束
        /// </summary>
        /// <returns>true:已结束</returns>
        Boolean PCPlanMovementIsDone();
        
        /// <summary>
        /// 停止PC规划运动，可由eCutResume恢复运动状态
        /// </summary>
        /// <returns></returns>
        Boolean PCPlanMovementPause();

        /// <summary>
        /// 恢复经eCutPause停止的运动PC规划运动
        /// </summary>
        /// <returns></returns>
        Boolean PCPlanMovementResume();

        /// <summary>
        /// 终止PC规划运动，不可由eCutResume恢复运动状态
        /// </summary>
        /// <returns></returns>
        Boolean PCPlanMovementAbort();

        /// <summary>
        /// 添加直线插补运动轨迹 参考坐标系为PC规划工作坐标系
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="vel"></param>
        /// <param name="ini_maxvel"></param>
        /// <param name="acc"></param>
        void PCPlanMovementAddLine(Double[] pos, Double vel, Double ini_maxvel, Double acc);

        /// <summary>
        /// 添加圆弧插补运动轨迹 参考坐标系为PC规划工作坐标系
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="center"></param>
        /// <param name="normal"></param>
        /// <param name="turn"></param>
        /// <param name="vel"></param>
        /// <param name="ini_maxvel"></param>
        /// <param name="acc"></param>
        void PCPlanMovementAddCircle(Double[] pos, Double[] center, Double[] normal, Int32 turn, Double vel, Double ini_maxvel, Double acc);

        /// <summary>
        /// 前瞻配置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        Boolean PCPlanMovementSetStopType(eCutStopType type, Double tolerance);

        /// <summary>
        /// 获取正在执行的PC规划指令所占的缓冲区长度
        /// </summary>
        /// <returns>正在执行的PC规划指令所占的缓冲区长度</returns>
        Int32 PCPlanMovementActiveDepth();

        /// <summary>
        /// 获取当前eCut指令缓存区长度
        /// </summary>
        /// <returns>当前eCut指令缓存区长度</returns>
        Int32 PCPlanMovementQueueDepth();

        /// <summary>
        /// 配置当前eCut PC规划运动的工作坐标
        /// </summary>
        /// <param name="pos"></param>
        void PCPlanMovementSetCurrentPostion(Double[] pos);
        #endregion
    }
}
