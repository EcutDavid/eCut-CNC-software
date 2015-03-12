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

LPCWSTR stringToLPCWSTR(std::string orig);
eCutPosition GeneratePositon(double x, double y, double z);
eCutCartesian GenerateCartesian(double x, double y, double z);

void CheckResult(int flag)
{
	char charArr[200];
	if (flag == 0 || flag == 3)
	{
		sprintf(charArr, "\ninvoke success");
	}
	else
	{
		sprintf(charArr, "\ninvoke failed");
	}
	OutputDebugString(stringToLPCWSTR(charArr));

}

//0 & 3 Means Success
void TestDepthFunctionWithLine()
{
	char charArr[200];
	int DeviceNums = GetDeviceNum();
	if (DeviceNums)
	{
		
		//Open ASU
		sprintf(charArr, "Cut exist addcircle test begin");
		OutputDebugString(stringToLPCWSTR(charArr));
		eCutDevice cutHandler = eCutOpen(0);
		eCutError result = eCutConfigDeviceDefault(cutHandler);
		result = eCutSetStepsPerUnitSmoothCoff(cutHandler, 50,
			new int[9]{1600, 1600, 1600},new int[9],16);
		int stepArray[9];
		result = eCutGetSteps(cutHandler, stepArray);
		eCutSetCurrentPostion(cutHandler, &GeneratePositon(0, 0, 0));
		eCutSetCoordinate(cutHandler, new double[9]{0,0,0,0,0,0,0,0});
		//AddLineTest
		result = eCutAddLine(cutHandler, &GeneratePositon(20, 0, 0), 5, 5, 5);
		eCutAddCircle(cutHandler, &GeneratePositon(20, 0, 0), &GenerateCartesian(15, 0, 0), &GenerateCartesian(0, 0, 1),0,5,5,5);
		eCutAddLine(cutHandler, &GeneratePositon(20, -20, 0), 5, 5, 5);
		eCutAddLine(cutHandler, &GeneratePositon(0, -20, 0), 5, 5, 5);
		eCutAddLine(cutHandler, &GeneratePositon(0, -0, 0), 5, 5, 5);
		while (true)
		{
			int activeDepth = eCutActiveDepth(cutHandler);
			int queueDepth  = eCutQueueDepth(cutHandler);
			eCutGetSteps(cutHandler, stepArray);
			sprintf(charArr, "\nactiveDepth: %d", activeDepth);
			OutputDebugString(stringToLPCWSTR(charArr));
			sprintf(charArr, "\queueDepth: %d",   queueDepth);
			OutputDebugString(stringToLPCWSTR(charArr));
			sprintf(charArr, "\nPosition X:%fX:%fY", stepArray[0] / (1600.0 * 16), stepArray[1] / (1600.0 * 16));
			OutputDebugString(stringToLPCWSTR(charArr));
			Sleep(1500);
			if (queueDepth == 0)
			{
				sprintf(charArr, "\nTestEnd");
				OutputDebugString(stringToLPCWSTR(charArr));
				exit(0);
			}
		}
	}
}

eCutCartesian GenerateCartesian(double x, double y, double z)
{
	eCutCartesian pos;
	pos.x = x;
	pos.y = y;
	pos.z = z;
	return pos;
}

eCutPosition GeneratePositon(double x, double y, double z)
{
	eCutPosition pos;
	pos.a = pos.b = pos.c = pos.u = pos.v = pos.w = 0;
	pos.x = x;
	pos.y = y;
	pos.z = z;
	return pos;
}

int _tmain(int argc, _TCHAR* argv[])
{
	TestDepthFunctionWithLine();
	
	wstring  dllName = L"eCutDevice.dll";
	HMODULE hDLL = LoadLibrary(dllName.c_str());
	/*GetDeviceNumFunc GetDeviceNumfp = GetDeviceNumFunc(GetProcAddress(hDLL, "GetDeviceNum"));
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
	eCutSetOutputFunc eCutSetOutputfp = eCutSetOutputFunc(GetProcAddress(hDLL, "eCutSetOutput"));*/
	eCut = eCutOpen(0);
	eCutConfigDeviceDefault(eCut);
	eCutPosition Position;
	Position.x = 0;
	Position.y = 0;
	Position.z = 0;
	Position.a = 0;
	Position.b = 0;
	Position.c = 0;
	Position.u = 0;
	Position.v = 0;
	Position.w = 0;
	eCutSetCurrentPostion(eCut, &Position);
	eCutSetStepsPerUnitSmoothCoff(eCut, 50, new int[4]{1600, 1600, 1600, 1600}, new int[4], 16);
	Position.x = -5;
	Position.y = 5;
	Position.z = 0;
	Position.a = 0;
	Position.b = 0;
	Position.c = 0;
	Position.u = 0;
	Position.v = 0;
	Position.w = 0;
	eCutAddLine(eCut, &Position, 5.0, 5.0, 5.0);
	for (size_t i = 0; i < 15; i++)
	{
		int pulseArr[9];
		Sleep(2000);
		//eCutGetSteps(eCut, pulseArr);
		/*for (size_t j = 0; j < 4; j++)
		{
			cout << "+" << pulseArr[j];
		}
		cout << endl;*/
	}
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