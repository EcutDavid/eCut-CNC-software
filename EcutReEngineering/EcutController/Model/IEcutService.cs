using System;

namespace EcutController
{
    public interface IEcutService
    {
        #region Common
        /// <summary>
        /// 开启与特定板卡的通讯，获取板卡资源句柄
        /// </summary>
        /// <param name="cutIndex">0:PC连接的第一个eCut，为1:PC连接的第二个eCut，在PC只与一个eCut相连时，此项配为0即可</param>
        /// <returns></returns>
        /// <exception cref="CutNotExistException">该索引值板卡不存在</exception>
        Boolean OpenCut();

        /// <summary>
        /// 关闭与板卡的通讯
        /// </summary>
        void CloseCut();


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

        /// <summary>
        /// 板卡各个轴脉冲信号输出引脚配置
        /// </summary>
        Byte[] CutStepSigPinConfigArr { get; set; }

        /// <summary>
        /// 板卡各个轴方向信号输出引脚配置
        /// </summary>
        Byte[] CutDirSigPinConfigArr { get; set; }

        /// <summary>
        /// 板卡各个轴方向信号输出引脚配置
        /// </summary>
        Boolean[] AxisEnableConfigArr { get; set; }

        /// <summary>
        /// 配置主轴输出
        /// </summary>
        /// <param name="outputVal">主轴输出配置值，最大为65535</param>
        void CutSetSpindle(UInt16 outputVal);

        /// <summary>
        /// 各个轴脉冲每毫米
        /// </summary>
        Int32[] CutStepsPerUnitConfigArr { get; set; }

        /// <summary>
        /// 机械坐标
        /// </summary>
        Double[] CutMachinePostionArr { get; set; }

        /// <summary>
        /// TODO 配置特殊功能
        /// </summary>
        /// <param name="inputIOEnableMask"></param>
        /// <param name="inputIONegMask"></param>
        /// <param name="inputIOPinConfigArr"></param>
        /// <returns></returns>
        void CutSetInputIOEngineDir(UInt64 inputIOEnableMask, UInt64 inputIONegMask, Byte[] inputIOPinConfigArr);
        #endregion

        #region HWPlanMovement
        /// <summary>
        /// 配置软限位
        /// </summary>
        /// <param name="maxPosArr"></param>
        /// <param name="minPosArr"></param>
        /// <returns></returns>
        void HWPlanMovementSetSoftLimit(Double[] maxPosArr, Double[] minPosArr);
        
        /// <summary>
        /// 配置运动加速度
        /// </summary>
        Double[] HWPlanMovementAccConfigArr { set; }
        
        /// <summary>
        /// 配置运动最大速度
        /// </summary>
        Double[] HWPlanMovementMaxSpeedConfigArr { get; set; }
        
        /// <summary>
        /// 多轴点动运动 参考坐标系为机械坐标系
        /// </summary>
        /// <param name="axisMask"></param>
        /// <param name="posArr"></param>
        /// <returns></returns>
        void HWPlanMovementMove(UInt16 axisMask, Double[] posArr);
        
        /// <summary>
        /// 单轴点动运动 参考坐标系为机械坐标系
        /// </summary>
        /// <param name="axisIndex"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        void HWPlanMovementMove(UInt16 axisIndex, Double pos);

        /// <summary>
        /// 停止特定轴的运动
        /// </summary>
        /// <param name="axisIndex">轴索引</param>
        /// <returns></returns>
        void HWPlanMovementStop(UInt16 axisIndex);
        
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
        void PCPlanMovementPause();

        /// <summary>
        /// 恢复经eCutPause停止的运动PC规划运动
        /// </summary>
        void PCPlanMovementResume();

        /// <summary>
        /// 终止PC规划运动，不可由eCutResume恢复运动状态
        /// </summary>
        void PCPlanMovementAbort();

        /// <summary>
        /// 添加直线插补运动轨迹 参考坐标系为PC规划工作坐标系
        /// </summary>
        /// <param name="posArray"></param>
        /// <param name="vel"></param>
        /// <param name="ini_maxvel"></param>
        /// <param name="acc"></param>
        void PCPlanMovementAddLine(Double[] posArray, Double vel, Double ini_maxvel, Double acc);

        /// <summary>
        /// 添加圆弧插补运动轨迹 参考坐标系为PC规划工作坐标系
        /// </summary>
        /// <param name="posArr"></param>
        /// <param name="centerPosArr"></param>
        /// <param name="normalPosArray"></param>
        /// <param name="turn"></param>
        /// <param name="vel"></param>
        /// <param name="ini_maxvel"></param>
        /// <param name="acc"></param>
        void PCPlanMovementAddCircle(Double[] posArr, Double[] centerPosArr, 
            Double[] normalPosArray, Int32 turn, Double vel, Double ini_maxvel, Double acc);

        /// <summary>
        /// 前瞻配置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tolerance"></param>
        void PCPlanMovementSetStopType(eCutStopType type, Double tolerance);

        /// <summary>
        /// 正在执行的PC规划指令所占的缓冲区长度
        /// </summary>
        Int32 PCPlanMovementActiveDepth { get; }

        /// <summary>
        /// 当前eCut指令缓存区长度
        /// </summary>
        Int32 PCPlanMovementQueueDepth{ get; }

        /// <summary>
        /// 配置当前eCut PC规划运动的工作坐标
        /// </summary>
        /// <param name="posArr"></param>
        void PCPlanMovementSetCurrentPostion(Double[] posArr);
        #endregion
    }
}
