ECUTDEVICE_API int GetDeviceNum(void);
ECUTDEVICE_API eCutError GetDeviceInfo(int Num, char SerialString[]);
ECUTDEVICE_API eCutDevice eCutOpen(int Num);
ECUTDEVICE_API eCutError eCutClose(eCutDevice eCut);
ECUTDEVICE_API eCutError eCutConfigDeviceDefault(eCutDevice eCut);


/*==================The Following functions are used for setting the parameters used by Card planning==========================*/

/**
* @brief  Stop current movements of given axis
* @param  eCut: a Handle that point to eCutDevice
* @param  Axis: Given a Axis
*   This parameter can be one of the following value.
*     @arg 0x0001: X Mask.
*     @arg 0x0002: Y Mask
*     @arg 0x0003: Z Mask
*     @arg 0x0004: A Mask
*     @arg 0x0005: B Mask
*     @arg 0x0006: C Mask
* @retval eCut_Error_Ok: if sucessed
* @retval false: if failed
*/
ECUTDEVICE_API eCutError eCutStop(eCutDevice eCut, unsigned short AxisMask);
ECUTDEVICE_API eCutError eCutStopAll(eCutDevice eCut);
ECUTDEVICE_API eCutError eCutSetCoordinate(eCutDevice eCut, double Posi[]);
ECUTDEVICE_API eCutError eCutGetMaxSpeed(eCutDevice eCut, double MaxSpeed[]);
ECUTDEVICE_API eCutError eCutSetSoftLimit(eCutDevice eCut, double MaxSoftLimit[], double MinSoftLimit[]);

/*==================The Following four motion functions are planned by Card=================================================*/

ECUTDEVICE_API eCutError eCutSetAccelarationMaxSpeed(eCutDevice eCut, double Acceleration[], double MaxSpeed[]);
ECUTDEVICE_API eCutError eCutMoveAtSpeed(eCutDevice eCut, unsigned short AxisMask, double Acceleration[], double MaxSpeed[]);
ECUTDEVICE_API eCutError eCutJogOn(eCutDevice eCut, unsigned short Axis, double PositionGiven[]);
/**
* @brief  Move to a certain Absolute Position.
* @param  eCut:               a Handle that point to eCutDevice
* @param  AxisMask:   a Axis Mask .
*   This parameter can be any combination of the following value.
*     @arg 0x0001: X Mask.
*     @arg 0x0002: Y Mask
*     @arg 0x0004: Z Mask
*     @arg 0x0008: A Mask
*     @arg 0x0010: B Mask
*     @arg 0x0020: C Mask
* @retval eCut_Error_Ok: if sucessed
* @retval false: if failed
*/
ECUTDEVICE_API eCutError eCutMoveAbsolute(eCutDevice eCut, unsigned short AxisMask, double PositionGiven[]);


/*=======The Following functions are used for config the motion planning by PC====================================================================*/

ECUTDEVICE_API eCutError eCutSetCurrentPostion(eCutDevice eCut, eCutPosition *Pos);
ECUTDEVICE_API eCutError eCutPause(eCutDevice eCut);
ECUTDEVICE_API eCutError eCutResume(eCutDevice eCut);
ECUTDEVICE_API eCutError eCutAbort(eCutDevice eCut);
ECUTDEVICE_API int eCutActiveDepth(eCutDevice eCut);
ECUTDEVICE_API int eCutQueueDepth(eCutDevice eCut);
ECUTDEVICE_API eCutError eCutSetStopType(eCutDevice eCut, eCutStopType type, double tolerance);
ECUTDEVICE_API eCutError eCutGcodeLineInterpret(eCutDevice eCut, char *Code);

/*=======The Following two motion functions are planned by PC====================================================================*/
ECUTDEVICE_API eCutError eCutAddLine(eCutDevice eCut, eCutPosition *end, double vel, double ini_maxvel, double acc);
ECUTDEVICE_API eCutError eCutAddCircle(eCutDevice eCut,
	eCutPosition *end,
	eCutCartesian *center,
	eCutCartesian *normal,
	int turn,
	double vel,
	double ini_maxvel,
	double acc
	);

/*========================================================Common function======================================================== */


ECUTDEVICE_API eCutError eCutIsDone(eCutDevice eCut);
/**
* @brief  Stop current movements of all the axis without deceleration,Sudden Stop
* @param  eCut: a Handle that point to eCutDevice
* @retval ture: if sucessed
* @retval false: if failed
*/
ECUTDEVICE_API eCutError eCutEStop(eCutDevice eCut);
ECUTDEVICE_API eCutError eCutGetInputIO(eCutDevice eCut, unsigned short pInput[]);
ECUTDEVICE_API eCutError eCutGetSteps(eCutDevice eCut, int Steps[]);
ECUTDEVICE_API eCutError eCutSetSpindle(eCutDevice eCut, unsigned short Out);
/**
* @brief  Sets the StepNeg Or Dir Neg.
* @param  eCut:               a Handle that point to eCutDevice
* @param  StepNeg:   a Neg Mask .
*   This parameter can be any combination of the following value.
*     @arg 0x01: X Step Neg Mask.
*     @arg 0x02: Y Step Neg Mask
*     @arg 0x04: Z Step NegMask
*     @arg 0x08: A Step Neg Mask
*     @arg 0x10: B Step Neg Mask
*     @arg 0x20: C Step Neg Mask
* @param  DirNeg:   a Neg Mask .
*   This parameter can be any combination of the following value.
*     @arg 0x01: X Dir Neg Mask.
*     @arg 0x02: Y Dir Neg Mask
*     @arg 0x04: Z Dir NegMask
*     @arg 0x08: A Dir Neg Mask
*     @arg 0x10: B Dir Neg Mask
*     @arg 0x20: C Dir Neg Mask
* @retval ture: if sucessed
* @retval false: if failed
*/
ECUTDEVICE_API eCutError eCutSetStepNegAndDirNeg(eCutDevice eCut, unsigned char StepNeg, unsigned char DirNeg);
ECUTDEVICE_API eCutError eCutSetStepsPerUnitSmoothCoff(eCutDevice eCut,
	unsigned short DelayBetweenPulseAndDir,
	int StepsPerAxis[],
	int WorkOffset[],
	int SmoothCoff);
ECUTDEVICE_API eCutError eCutGetSmoothCoff(eCutDevice eCut, unsigned int *pSmoothCoff);
ECUTDEVICE_API eCutError eCutGetStepsPerUnit(eCutDevice eCut, int StepsPerUnit[]);
ECUTDEVICE_API eCutError eCutGetSpindlePostion(eCutDevice eCut, unsigned short *SpindlePostion);
ECUTDEVICE_API eCutError eCutGetEncoderPostion(eCutDevice eCut, unsigned short EncoderPostion[]);
ECUTDEVICE_API  eCutError eCutSetAxisOutputConfig(
	eCutDevice eCut,
	INT8U StepSel[],
	INT8U DirSel[],
	BOOLEAN Enable[],
	unsigned short StepNeg,
	unsigned short DirNeg
	);
ECUTDEVICE_API eCutError eCutSetInputIOEngineDir(eCutDevice eCut,
	uINT64U InputIOEnable,
	uINT64U InputIONeg,
	INT8U InputIOPin[],
	INT8S EngineDirections[]
	);
ECUTDEVICE_API eCutError eCutSetG92StepDirEncPin(eCutDevice eCut,
	INT32U G92Offset[],/*In Pulse Number*/
	INT16U StepNegAndDirNeg,
	INT8U EncoderAPin[],
	INT8U EncoderBPin[],
	INT8U MPGIndex
	);
eCutError eCutSetOutput(
	eCutDevice eCut,
	INT16U Sync,
	INT16S AnalogOut[],
	INT16U DigitalOut[]
	);




