// This is the main DLL file.

#include "stdafx.h"
#include <iostream> 
#include <windows.h>
#include "PInvokeTest.h"
#include "ecutdevice.h"
using namespace std;

typedef int(*GetDeviceNumFunc)(void);
typedef eCutError(*GetDeviceInfoFunc)(int Num, char SerialString[]);
typedef eCutDevice(*eCutOpenFunc)(int Num);
typedef eCutError(*eCutCloseFunc)(eCutDevice eCut);
typedef eCutError(*eCutConfigDeviceDefaultFunc)(eCutDevice eCut);
typedef eCutError(*eCutStopFunc)(eCutDevice eCut, unsigned short AxisMask);
typedef eCutError(*eCutStopAllFunc)(eCutDevice eCut);
typedef eCutError(*eCutSetCoordinateFunc)(eCutDevice eCut, double Posi[]);
typedef eCutError(*eCutGetMaxSpeedFunc)(eCutDevice eCut, double MaxSpeed[]);
typedef eCutError(*eCutSetSoftLimitFunc)(eCutDevice eCut, double MaxSoftLimit[], double MinSoftLimit[]);
typedef eCutError(*eCutSetAccelarationMaxSpeedFunc)(eCutDevice eCut, double Acceleration[], double MaxSpeed[]);
typedef eCutError(*eCutMoveAtSpeedFunc)(eCutDevice eCut, unsigned short AxisMask, double Acceleration[], double MaxSpeed[]);
typedef eCutError(*eCutJogOnFunc)(eCutDevice eCut, unsigned short Axis, double PositionGiven[]);
typedef eCutError(*eCutMoveAbsoluteFunc)(eCutDevice eCut, unsigned short AxisMask, double PositionGiven[]);
typedef eCutError(*eCutSetCurrentPostionFunc)(eCutDevice eCut, eCutPosition *Pos);
typedef eCutError(*eCutPauseFunc)(eCutDevice eCut);
typedef eCutError(*eCutResumeFunc)(eCutDevice eCut);
typedef eCutError(*eCutAbortFunc)(eCutDevice eCut);
typedef int(*eCutActiveDepthFunc)(eCutDevice eCut);
typedef int(*eCutQueueDepthFunc)(eCutDevice eCut);
typedef eCutError(*eCutSetStopTypeFunc)(eCutDevice eCut, eCutStopType type, double tolerance);
typedef eCutError(*eCutGcodeLineInterpretFunc)(eCutDevice eCut, char *Code);
typedef eCutError(*eCutAddLineFunc)(eCutDevice eCut, eCutPosition *end, double vel, double ini_maxvel, double acc);
typedef eCutError(*eCutAddCircleFunc)(eCutDevice eCut,
	eCutPosition *end,
	eCutCartesian *center,
	eCutCartesian *normal,
	int turn,
	double vel,
	double ini_maxvel,
	double acc
	);
typedef eCutError(*eCutIsDoneFunc)(eCutDevice eCut);
typedef eCutError(*eCutEStopFunc)(eCutDevice eCut);
typedef eCutError(*eCutGetInputIOFunc)(eCutDevice eCut, unsigned short pInput[]);
typedef eCutError(*eCutGetStepsFunc)(eCutDevice eCut, int Steps[]);
typedef eCutError(*eCutSetSpindleFunc)(eCutDevice eCut, unsigned short Out);
typedef eCutError(*eCutSetStepNegAndDirNegFunc)(eCutDevice eCut, unsigned char StepNeg, unsigned char DirNeg);
typedef eCutError(*eCutSetStepsPerUnitSmoothCoffFunc)(eCutDevice eCut,
	unsigned short DelayBetweenPulseAndDir,
	int StepsPerAxis[],
	int WorkOffset[],
	int SmoothCoff);
typedef eCutError(*eCutGetSmoothCoffFunc)(eCutDevice eCut, unsigned int *pSmoothCoff);
typedef eCutError(*eCutGetStepsPerUnitFunc)(eCutDevice eCut, int StepsPerUnit[]);
typedef eCutError(*eCutGetSpindlePostionFunc)(eCutDevice eCut, unsigned short *SpindlePostion);
typedef eCutError(*eCutGetEncoderPostionFunc)(eCutDevice eCut, unsigned short EncoderPostion[]);
typedef  eCutError(*eCutSetAxisOutputConfigFunc)(
	eCutDevice eCut,
	INT8U StepSel[],
	INT8U DirSel[],
	BOOLEAN Enable[],
	unsigned short StepNeg,
	unsigned short DirNeg
	);
typedef eCutError(*eCutSetInputIOEngineDirFunc)(eCutDevice eCut,
	uINT64U InputIOEnable,
	uINT64U InputIONeg,
	INT8U InputIOPin[],
	INT8S EngineDirections[]
	);
typedef eCutError(*eCutSetG92StepDirEncPinFunc)(eCutDevice eCut,
	INT32U G92Offset[],/*In Pulse Number*/
	INT16U StepNegAndDirNeg,
	INT8U EncoderAPin[],
	INT8U EncoderBPin[],
	INT8U MPGIndex
	);
typedef eCutError(*eCutSetOutputFunc)(
	eCutDevice eCut,
	INT16U Sync,
	INT16S AnalogOut[],
	INT16U DigitalOut[]
	);

struct position
{
public:
	double x, y, z;
};




GetDeviceNumFunc GetDeviceNumfp;
GetDeviceInfoFunc GetDeviceInfofp;
eCutOpenFunc eCutOpenfp;
eCutCloseFunc eCutClosefp;
eCutConfigDeviceDefaultFunc eCutConfigDeviceDefaultfp;
eCutStopFunc eCutStopfp;
eCutStopAllFunc eCutStopAllfp;
eCutSetCoordinateFunc eCutSetCoordinatefp;
eCutGetMaxSpeedFunc eCutGetMaxSpeedfp;
eCutSetSoftLimitFunc eCutSetSoftLimitfp;
eCutSetAccelarationMaxSpeedFunc eCutSetAccelarationMaxSpeedfp;
eCutMoveAtSpeedFunc eCutMoveAtSpeedfp;
eCutJogOnFunc eCutJogOnfp;
eCutMoveAbsoluteFunc eCutMoveAbsolutefp;
eCutSetCurrentPostionFunc eCutSetCurrentPostionfp;
eCutPauseFunc eCutPausefp;
eCutResumeFunc eCutResumefp;
eCutAbortFunc eCutAbortfp;
eCutActiveDepthFunc eCutActiveDepthfp;
eCutQueueDepthFunc eCutQueueDepthfp;
eCutSetStopTypeFunc eCutSetStopTypefp;
eCutGcodeLineInterpretFunc eCutGcodeLineInterpretfp;
eCutAddLineFunc eCutAddLinefp;
eCutAddCircleFunc eCutAddCirclefp;
eCutIsDoneFunc eCutIsDonefp;
eCutEStopFunc eCutEStopfp;
eCutGetInputIOFunc eCutGetInputIOfp;
eCutGetStepsFunc eCutGetStepsfp;
eCutSetSpindleFunc eCutSetSpindlefp;
eCutSetStepNegAndDirNegFunc eCutSetStepNegAndDirNegfp;
eCutSetStepsPerUnitSmoothCoffFunc eCutSetStepsPerUnitSmoothCofffp;
eCutGetSmoothCoffFunc eCutGetSmoothCofffp;
eCutGetStepsPerUnitFunc eCutGetStepsPerUnitfp;
eCutGetSpindlePostionFunc eCutGetSpindlePostionfp;
eCutGetEncoderPostionFunc eCutGetEncoderPostionfp;
eCutSetAxisOutputConfigFunc eCutSetAxisOutputConfigfp;
eCutSetInputIOEngineDirFunc eCutSetInputIOEngineDirfp;
eCutSetG92StepDirEncPinFunc eCutSetG92StepDirEncPinfp;
eCutSetOutputFunc eCutSetOutputfp;


LPCWSTR stringToLPCWSTR(string orig)
{
	size_t origsize = orig.length() + 1;
	const size_t newsize = 100;
	size_t convertedChars = 0;
	wchar_t *wcstring = (wchar_t *)malloc(sizeof(wchar_t)*(orig.length() - 1));
	mbstowcs_s(&convertedChars, wcstring, origsize, orig.c_str(), _TRUNCATE);
	return wcstring;
}

extern "C" PInvokeTestAPI int Add(int a, int b)
{
	char charArr[64];
	sprintf(charArr, "\nVar I get:%d %d", a, b);
	OutputDebugString(stringToLPCWSTR(charArr));
	return a + b;
}

extern "C" PInvokeTestAPI void AddWithArray(int a[], int b[])
{
	char charArr[200];
	sprintf(charArr, "\nVar I get:a[0]%d a[1]%d a[2]%d a[3]%d b[0]%d b[1]%d b[2]%d b[3]%d ", a[0], a[1], a[2], a[3], b[0], b[1], b[2], b[3]);
	OutputDebugString(stringToLPCWSTR(charArr));
}

extern "C" PInvokeTestAPI void AddWithStruct(position pos)
{
	char charArr[200];
	sprintf(charArr, "\nVar I get:X%f Y%f Z%f", pos.x, pos.y, pos.z);
	OutputDebugString(stringToLPCWSTR(charArr));
}

extern "C" PInvokeTestAPI void ImoprterInit()
{
	char charArr[200];
	sprintf(charArr, "This is Lib Importer,Init...");
	OutputDebugString(stringToLPCWSTR(charArr));
	wstring  dllName = L"eCutDevice.dll";
	HMODULE hDLL = LoadLibrary(dllName.c_str());
	GetDeviceNumfp = GetDeviceNumFunc(GetProcAddress(hDLL, "GetDeviceNum"));
	GetDeviceInfofp = GetDeviceInfoFunc(GetProcAddress(hDLL, "GetDeviceInfo"));
	eCutOpenfp = eCutOpenFunc(GetProcAddress(hDLL, "eCutOpen"));
	eCutClosefp = eCutCloseFunc(GetProcAddress(hDLL, "eCutClose"));
	eCutConfigDeviceDefaultfp = eCutConfigDeviceDefaultFunc(GetProcAddress(hDLL, "eCutConfigDeviceDefault"));
	eCutStopfp = eCutStopFunc(GetProcAddress(hDLL, "eCutStop"));
	eCutStopAllfp = eCutStopAllFunc(GetProcAddress(hDLL, "eCutStopAll"));
	eCutSetCoordinatefp = eCutSetCoordinateFunc(GetProcAddress(hDLL, "eCutSetCoordinate"));
	eCutGetMaxSpeedfp = eCutGetMaxSpeedFunc(GetProcAddress(hDLL, "eCutGetMaxSpeed"));
	eCutSetSoftLimitfp = eCutSetSoftLimitFunc(GetProcAddress(hDLL, "eCutSetSoftLimit"));
	eCutSetAccelarationMaxSpeedfp = eCutSetAccelarationMaxSpeedFunc(GetProcAddress(hDLL, "eCutSetAccelarationMaxSpeed"));
	eCutMoveAtSpeedfp = eCutMoveAtSpeedFunc(GetProcAddress(hDLL, "eCutMoveAtSpeed"));
	eCutJogOnfp = eCutJogOnFunc(GetProcAddress(hDLL, "eCutJogOn"));
	eCutMoveAbsolutefp = eCutMoveAbsoluteFunc(GetProcAddress(hDLL, "eCutMoveAbsolute"));
	eCutSetCurrentPostionfp = eCutSetCurrentPostionFunc(GetProcAddress(hDLL, "eCutSetCurrentPostion"));
	eCutPausefp = eCutPauseFunc(GetProcAddress(hDLL, "eCutPause"));
	eCutResumefp = eCutResumeFunc(GetProcAddress(hDLL, "eCutResume"));
	eCutAbortfp = eCutAbortFunc(GetProcAddress(hDLL, "eCutAbort"));
	eCutActiveDepthfp = eCutActiveDepthFunc(GetProcAddress(hDLL, "eCutActiveDepth"));
	eCutQueueDepthfp = eCutQueueDepthFunc(GetProcAddress(hDLL, "eCutQueueDepth"));
	eCutSetStopTypefp = eCutSetStopTypeFunc(GetProcAddress(hDLL, "eCutSetStopType"));
	eCutGcodeLineInterpretfp = eCutGcodeLineInterpretFunc(GetProcAddress(hDLL, "eCutGcodeLineInterpret"));
	eCutAddLinefp = eCutAddLineFunc(GetProcAddress(hDLL, "eCutAddLine"));
	eCutAddCirclefp = eCutAddCircleFunc(GetProcAddress(hDLL, "eCutAddCircle"));
	eCutIsDonefp = eCutIsDoneFunc(GetProcAddress(hDLL, "eCutIsDone"));
	eCutEStopfp = eCutEStopFunc(GetProcAddress(hDLL, "eCutEStop"));
	eCutGetInputIOfp = eCutGetInputIOFunc(GetProcAddress(hDLL, "eCutGetInputIO"));
	eCutGetStepsfp = eCutGetStepsFunc(GetProcAddress(hDLL, "eCutGetSteps"));
	eCutSetSpindlefp = eCutSetSpindleFunc(GetProcAddress(hDLL, "eCutSetSpindle"));
	eCutSetStepNegAndDirNegfp = eCutSetStepNegAndDirNegFunc(GetProcAddress(hDLL, "eCutSetStepNegAndDirNeg"));
	eCutSetStepsPerUnitSmoothCofffp = eCutSetStepsPerUnitSmoothCoffFunc(GetProcAddress(hDLL, "eCutSetStepsPerUnitSmoothCoff"));
	eCutGetSmoothCofffp = eCutGetSmoothCoffFunc(GetProcAddress(hDLL, "eCutGetSmoothCoff"));
	eCutGetStepsPerUnitfp = eCutGetStepsPerUnitFunc(GetProcAddress(hDLL, "eCutGetStepsPerUnit"));
	eCutGetSpindlePostionfp = eCutGetSpindlePostionFunc(GetProcAddress(hDLL, "eCutGetSpindlePostion"));
	eCutGetEncoderPostionfp = eCutGetEncoderPostionFunc(GetProcAddress(hDLL, "eCutGetEncoderPostion"));
	eCutSetAxisOutputConfigfp = eCutSetAxisOutputConfigFunc(GetProcAddress(hDLL, "eCutSetAxisOutputConfig"));
	eCutSetInputIOEngineDirfp = eCutSetInputIOEngineDirFunc(GetProcAddress(hDLL, "eCutSetInputIOEngineDir"));
	eCutSetG92StepDirEncPinfp = eCutSetG92StepDirEncPinFunc(GetProcAddress(hDLL, "eCutSetG92StepDirEncPin"));
	eCutSetOutputfp = eCutSetOutputFunc(GetProcAddress(hDLL, "eCutSetOutput"));
}

extern "C" PInvokeTestAPI int GetDeviceNum(void)
{
	return GetDeviceNumfp();
}
extern "C" PInvokeTestAPI eCutError GetDeviceInfo(int Num, char SerialString[])
{
	return GetDeviceInfofp(Num, SerialString);
}
extern "C" PInvokeTestAPI eCutDevice eCutOpen(int Num)
{
	return eCutOpenfp(Num);
}
extern "C" PInvokeTestAPI eCutError eCutClose(eCutDevice eCut)
{
	return eCutClosefp(eCut);
}
extern "C" PInvokeTestAPI eCutError eCutConfigDeviceDefault(eCutDevice eCut)
{
	return eCutConfigDeviceDefaultfp(eCut);
}
extern "C" PInvokeTestAPI eCutError eCutStop(eCutDevice eCut, unsigned short AxisMask)
{
	return eCutStopfp(eCut, AxisMask);
}
extern "C" PInvokeTestAPI eCutError eCutStopAll(eCutDevice eCut)
{
	return eCutStopAllfp(eCut);
}
extern "C" PInvokeTestAPI eCutError eCutSetCoordinate(eCutDevice eCut, double Posi[])
{
	return eCutSetCoordinatefp(eCut, Posi);
}
extern "C" PInvokeTestAPI eCutError eCutGetMaxSpeed(eCutDevice eCut, double MaxSpeed[])
{
	return eCutGetMaxSpeedfp(eCut, MaxSpeed);
}
extern "C" PInvokeTestAPI eCutError eCutSetSoftLimit(eCutDevice eCut, double MaxSoftLimit[], double MinSoftLimit[])
{
	return eCutSetSoftLimitfp(eCut, MaxSoftLimit, MinSoftLimit);
}
extern "C" PInvokeTestAPI eCutError eCutSetAccelarationMaxSpeed(eCutDevice eCut, double Acceleration[], double MaxSpeed[])
{
	char charArr[200];
	sprintf(charArr, "\nTHIS IS SetAccelarationMaxSpeed X A%f  Y A%f  Z A%f  A A%f X S%f  Y S%f  Z S%f  A S%f",
		Acceleration[0], Acceleration[1], Acceleration[2], Acceleration[3], MaxSpeed[0], MaxSpeed[1], MaxSpeed[2], MaxSpeed[3]);
	OutputDebugString(stringToLPCWSTR(charArr));
	return eCutSetAccelarationMaxSpeedfp(eCut, Acceleration, MaxSpeed);
}
extern "C" PInvokeTestAPI eCutError eCutMoveAtSpeed(eCutDevice eCut, unsigned short AxisMask, double Acceleration[], double MaxSpeed[])
{
	return eCutMoveAtSpeedfp(eCut, AxisMask, Acceleration, MaxSpeed);
}
extern "C" PInvokeTestAPI eCutError eCutJogOn(eCutDevice eCut, unsigned short Axis, double PositionGiven[])
{
	return eCutJogOnfp(eCut, Axis, PositionGiven);
}
extern "C" PInvokeTestAPI eCutError eCutMoveAbsolute(eCutDevice eCut, unsigned short AxisMask, double PositionGiven[])
{
	return eCutMoveAbsolutefp(eCut, AxisMask, PositionGiven);
}
extern "C" PInvokeTestAPI eCutError eCutSetCurrentPostion(eCutDevice eCut, eCutPosition *Pos)
{
	return eCutSetCurrentPostionfp(eCut, Pos);
}
extern "C" PInvokeTestAPI eCutError eCutPause(eCutDevice eCut)
{
	return eCutPausefp(eCut);
}
extern "C" PInvokeTestAPI eCutError eCutResume(eCutDevice eCut)
{
	return eCutResumefp(eCut);
}
extern "C" PInvokeTestAPI eCutError eCutAbort(eCutDevice eCut)
{
	return eCutAbortfp(eCut);
}
extern "C" PInvokeTestAPI int eCutActiveDepth(eCutDevice eCut)
{
	return eCutActiveDepthfp(eCut);
}
extern "C" PInvokeTestAPI int eCutQueueDepth(eCutDevice eCut)
{
	return eCutQueueDepthfp(eCut);
}
extern "C" PInvokeTestAPI eCutError eCutSetStopType(eCutDevice eCut, eCutStopType type, double tolerance)
{
	return eCutSetStopTypefp(eCut, type, tolerance);
}
extern "C" PInvokeTestAPI eCutError eCutGcodeLineInterpret(eCutDevice eCut, char *Code)
{
	return eCutGcodeLineInterpretfp(eCut, Code);
}
extern "C" PInvokeTestAPI eCutError eCutAddLine(eCutDevice eCut, eCutPosition *end, double vel, double ini_maxvel, double acc)
{
	return eCutAddLinefp(eCut, end, vel, ini_maxvel, acc);
}
extern "C" PInvokeTestAPI eCutError eCutAddCircle(eCutDevice eCut, eCutPosition *end, eCutCartesian *center,
	eCutCartesian *normal,
	int turn,
	double vel,
	double ini_maxvel,
	double acc
	)
{
	return eCutAddCirclefp(eCut, end, center,
		normal,
		turn,
		vel,
		ini_maxvel,
		acc);
}
extern "C" PInvokeTestAPI eCutError eCutIsDone(eCutDevice eCut)
{
	return eCutIsDonefp(eCut);
}
extern "C" PInvokeTestAPI eCutError eCutEStop(eCutDevice eCut)
{
	return eCutEStopfp(eCut);
}
extern "C" PInvokeTestAPI eCutError eCutGetInputIO(eCutDevice eCut, unsigned short pInput[])
{
	return eCutGetInputIOfp(eCut, pInput);
}
extern "C" PInvokeTestAPI eCutError eCutGetSteps(eCutDevice eCut, int Steps[])
{
	return eCutGetStepsfp(eCut, Steps);
}
extern "C" PInvokeTestAPI eCutError eCutSetSpindle(eCutDevice eCut, unsigned short Out)
{
	return eCutSetSpindlefp(eCut, Out);
}
extern "C" PInvokeTestAPI eCutError eCutSetStepNegAndDirNeg(eCutDevice eCut, unsigned char StepNeg, unsigned char DirNeg)
{
	return eCutSetStepNegAndDirNegfp(eCut, StepNeg, DirNeg);
}
extern "C" PInvokeTestAPI eCutError eCutSetStepsPerUnitSmoothCoff(eCutDevice eCut,
	unsigned short DelayBetweenPulseAndDir,
	int StepsPerAxis[],
	int WorkOffset[],
	int SmoothCoff)
{
	char charArr[200];
	sprintf(charArr, "\nTHIS IS SetStepsPerUnit X %d  Y %d  Z %d  A %d SMO%d",
		StepsPerAxis[0], StepsPerAxis[1], StepsPerAxis[2], StepsPerAxis[3], SmoothCoff);
	OutputDebugString(stringToLPCWSTR(charArr));
	return eCutSetStepsPerUnitSmoothCofffp(eCut,
		DelayBetweenPulseAndDir,
		StepsPerAxis,
		WorkOffset,
		SmoothCoff);
}
extern "C" PInvokeTestAPI eCutError eCutGetSmoothCoff(eCutDevice eCut, unsigned int *pSmoothCoff)
{
	return eCutGetSmoothCofffp(eCut, pSmoothCoff);
}
extern "C" PInvokeTestAPI eCutError eCutGetStepsPerUnit(eCutDevice eCut, int StepsPerUnit[])
{
	return eCutGetStepsPerUnitfp(eCut, StepsPerUnit);
}
extern "C" PInvokeTestAPI eCutError eCutGetSpindlePostion(eCutDevice eCut, unsigned short *SpindlePostion)
{
	return eCutGetSpindlePostionfp(eCut, SpindlePostion);
}
extern "C" PInvokeTestAPI eCutError eCutGetEncoderPostion(eCutDevice eCut, unsigned short EncoderPostion[])
{
	return eCutGetEncoderPostionfp(eCut, EncoderPostion);
}
extern "C" PInvokeTestAPI  eCutError eCutSetAxisOutputConfig(
	eCutDevice eCut,
	INT8U StepSel[],
	INT8U DirSel[],
	BOOLEAN Enable[],
	unsigned short StepNeg,
	unsigned short DirNeg
	)
{
	return eCutSetAxisOutputConfigfp(eCut,
		StepSel,
		DirSel,
		Enable,
		StepNeg,
		DirNeg);
}
extern "C" PInvokeTestAPI eCutError eCutSetInputIOEngineDir(eCutDevice eCut,
	uINT64U InputIOEnable,
	uINT64U InputIONeg,
	INT8U InputIOPin[],
	INT8S EngineDirections[]
	)
{
	return eCutSetInputIOEngineDirfp(eCut,
		InputIOEnable,
		InputIONeg,
		InputIOPin,
		EngineDirections);
}
extern "C" PInvokeTestAPI eCutError eCutSetG92StepDirEncPin(eCutDevice eCut,
	INT32U G92Offset[],/*In Pulse Number*/
	INT16U StepNegAndDirNeg,
	INT8U EncoderAPin[],
	INT8U EncoderBPin[],
	INT8U MPGIndex
	)
{
	return eCutSetG92StepDirEncPinfp(eCut,
		G92Offset,/*In Pulse Number*/
		StepNegAndDirNeg,
		EncoderAPin,
		EncoderBPin,
		MPGIndex);
}
extern "C" PInvokeTestAPI eCutError CutSetOutput(eCutDevice eCut,
	INT16U Sync,
	INT16S AnalogOut[],
	INT16U DigitalOut[]
	)
{
	return eCutSetOutputfp(eCut,
		Sync,
		AnalogOut,
		DigitalOut);
}