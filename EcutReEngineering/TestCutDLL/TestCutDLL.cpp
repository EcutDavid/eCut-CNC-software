#include "stdafx.h"
using namespace std;
#include <iostream> 
#include <windows.h>
using namespace System;
using namespace System::IO;
#include "ecutdevice.h"


eCutDevice eCut;
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


int _tmain(int argc, _TCHAR* argv[])
{
	wstring  dllName = L"eCutDevice.dll";
	HMODULE hDLL = LoadLibrary(dllName.c_str());
	GetDeviceNumFunc GetDeviceNumfp = GetDeviceNumFunc(GetProcAddress(hDLL, "GetDeviceNum"));
	GetDeviceInfoFunc GetDeviceInfofp = GetDeviceInfoFunc(GetProcAddress(hDLL, "GetDeviceInfo"));
	eCutOpenFunc eCutOpenfp = eCutOpenFunc(GetProcAddress(hDLL, "eCutOpen"));
	eCutCloseFunc eCutClosefp = eCutCloseFunc(GetProcAddress(hDLL, "eCutClose"));
	eCutConfigDeviceDefaultFunc eCutConfigDeviceDefaultfp = eCutConfigDeviceDefaultFunc(GetProcAddress(hDLL, "eCutConfigDeviceDefault"));
	eCutStopFunc eCutStopfp = eCutStopFunc(GetProcAddress(hDLL, "eCutStop"));
	eCutStopAllFunc eCutStopAllfp = eCutStopAllFunc(GetProcAddress(hDLL, "eCutStopAll"));
	eCutSetCoordinateFunc eCutSetCoordinatefp = eCutSetCoordinateFunc(GetProcAddress(hDLL, "eCutSetCoordinate"));
	eCutGetMaxSpeedFunc eCutGetMaxSpeedfp = eCutGetMaxSpeedFunc(GetProcAddress(hDLL, "eCutGetMaxSpeed"));
	eCutSetSoftLimitFunc eCutSetSoftLimitfp = eCutSetSoftLimitFunc(GetProcAddress(hDLL, "eCutSetSoftLimit"));
	eCutSetAccelarationMaxSpeedFunc eCutSetAccelarationMaxSpeedfp = eCutSetAccelarationMaxSpeedFunc(GetProcAddress(hDLL, "eCutSetAccelarationMaxSpeed"));
	eCutMoveAtSpeedFunc eCutMoveAtSpeedfp = eCutMoveAtSpeedFunc(GetProcAddress(hDLL, "eCutMoveAtSpeed"));
	eCutJogOnFunc eCutJogOnfp = eCutJogOnFunc(GetProcAddress(hDLL, "eCutJogOn"));
	eCutMoveAbsoluteFunc eCutMoveAbsolutefp = eCutMoveAbsoluteFunc(GetProcAddress(hDLL, "eCutMoveAbsolute"));
	eCutSetCurrentPostionFunc eCutSetCurrentPostionfp = eCutSetCurrentPostionFunc(GetProcAddress(hDLL, "eCutSetCurrentPostion"));
	eCutPauseFunc eCutPausefp = eCutPauseFunc(GetProcAddress(hDLL, "eCutPause"));
	eCutResumeFunc eCutResumefp = eCutResumeFunc(GetProcAddress(hDLL, "eCutResume"));
	eCutAbortFunc eCutAbortfp = eCutAbortFunc(GetProcAddress(hDLL, "eCutAbort"));
	eCutActiveDepthFunc eCutActiveDepthfp = eCutActiveDepthFunc(GetProcAddress(hDLL, "eCutActiveDepth"));
	eCutQueueDepthFunc eCutQueueDepthfp = eCutQueueDepthFunc(GetProcAddress(hDLL, "eCutQueueDepth"));
	eCutSetStopTypeFunc eCutSetStopTypefp = eCutSetStopTypeFunc(GetProcAddress(hDLL, "eCutSetStopType"));
	eCutGcodeLineInterpretFunc eCutGcodeLineInterpretfp = eCutGcodeLineInterpretFunc(GetProcAddress(hDLL, "eCutGcodeLineInterpret"));
	eCutAddLineFunc eCutAddLinefp = eCutAddLineFunc(GetProcAddress(hDLL, "eCutAddLine"));
	eCutAddCircleFunc eCutAddCirclefp = eCutAddCircleFunc(GetProcAddress(hDLL, "eCutAddCircle"));
	eCutIsDoneFunc eCutIsDonefp = eCutIsDoneFunc(GetProcAddress(hDLL, "eCutIsDone"));
	eCutEStopFunc eCutEStopfp = eCutEStopFunc(GetProcAddress(hDLL, "eCutEStop"));
	eCutGetInputIOFunc eCutGetInputIOfp = eCutGetInputIOFunc(GetProcAddress(hDLL, "eCutGetInputIO"));
	eCutGetStepsFunc eCutGetStepsfp = eCutGetStepsFunc(GetProcAddress(hDLL, "eCutGetSteps"));
	eCutSetSpindleFunc eCutSetSpindlefp = eCutSetSpindleFunc(GetProcAddress(hDLL, "eCutSetSpindle"));
	eCutSetStepNegAndDirNegFunc eCutSetStepNegAndDirNegfp = eCutSetStepNegAndDirNegFunc(GetProcAddress(hDLL, "eCutSetStepNegAndDirNeg"));
	eCutSetStepsPerUnitSmoothCoffFunc eCutSetStepsPerUnitSmoothCofffp = eCutSetStepsPerUnitSmoothCoffFunc(GetProcAddress(hDLL, "eCutSetStepsPerUnitSmoothCoff"));
	eCutGetSmoothCoffFunc eCutGetSmoothCofffp = eCutGetSmoothCoffFunc(GetProcAddress(hDLL, "eCutGetSmoothCoff"));
	eCutGetStepsPerUnitFunc eCutGetStepsPerUnitfp = eCutGetStepsPerUnitFunc(GetProcAddress(hDLL, "eCutGetStepsPerUnit"));
	eCutGetSpindlePostionFunc eCutGetSpindlePostionfp = eCutGetSpindlePostionFunc(GetProcAddress(hDLL, "eCutGetSpindlePostion"));
	eCutGetEncoderPostionFunc eCutGetEncoderPostionfp = eCutGetEncoderPostionFunc(GetProcAddress(hDLL, "eCutGetEncoderPostion"));
	eCutSetAxisOutputConfigFunc eCutSetAxisOutputConfigfp = eCutSetAxisOutputConfigFunc(GetProcAddress(hDLL, "eCutSetAxisOutputConfig"));
	eCutSetInputIOEngineDirFunc eCutSetInputIOEngineDirfp = eCutSetInputIOEngineDirFunc(GetProcAddress(hDLL, "eCutSetInputIOEngineDir"));
	eCutSetG92StepDirEncPinFunc eCutSetG92StepDirEncPinfp = eCutSetG92StepDirEncPinFunc(GetProcAddress(hDLL, "eCutSetG92StepDirEncPin"));
	eCutSetOutputFunc eCutSetOutputfp = eCutSetOutputFunc(GetProcAddress(hDLL, "eCutSetOutput"));
	char buf[1000];
	int cutNumber = GetDeviceNum();
	String^ path = Directory::GetCurrentDirectory();
	int result = GetDeviceNumfp();
	cout << result << endl;
	eCut = eCutOpenfp(0);
	cout << eCut << endl;
	eCutConfigDeviceDefaultfp(eCut);
	/*cout << error << endl;
	eCutPosition Position;
	Position.a = 0;
	Position.x = 0;
	Position.y = 0;
	Position.z = 0;
	error = eCutSetCurrentPostionfp(eCut, &Position);*/
	/*eCutSetAxisOutputConfig(eCut, new INT8U[16]{0, 1, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
		new INT8U[16]{8, 9, 10, 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		new boolean[4]{true, true, true, true}, 0, 0);*/
	//eCutSetStepsPerUnitSmoothCoff(eCut, 50, new int[4]{2000, 2000, 2000, 2000}, new int[4], 16);
	eCutSetStepsPerUnitSmoothCoff(eCut, 50, new int[1]{2000}, new int[4], 16);
	//eCutSetAccelarationMaxSpeed(eCut, new double[4]{50}, new double[4]{50});
	eCutSetAccelarationMaxSpeed(eCut, new double[1]{50}, new double[1]{50});
	//eCutMoveAbsolute(eCut, 1, new double[4]{20000});
	//eCutMoveAbsolute(eCut, 1, new double[4]{-20000});
	eCutMoveAbsolute(eCut, 1, new double[1]{20000});
	eCutMoveAbsolute(eCut, 1, new double[1]{-20000});
	/*cout << "Result of set currentPostion" << error << endl;
	Position.a = 0;
	Position.x = 0;
	Position.y = -500;
	Position.z = 0;
	eCutAddLinefp(eCut, &Position, 50.0, 50.0, 50.0);

	for (size_t i = 0; i < 15; i++)
	{
	int pulseArr[9];
	Sleep(200);
	eCutGetStepsfp(eCut, pulseArr);
	for (size_t j = 0; j < 4; j++)
	{
	cout << "+" << pulseArr[j];
	}
	cout << endl;
	}*/
}


LPCWSTR stringToLPCWSTR(std::string orig)
{
	size_t origsize = orig.length() + 1;
	const size_t newsize = 100;
	size_t convertedChars = 0;
	wchar_t *wcstring = (wchar_t *)malloc(sizeof(wchar_t)*(orig.length() - 1));
	mbstowcs_s(&convertedChars, wcstring, origsize, orig.c_str(), _TRUNCATE);
	return wcstring;
}