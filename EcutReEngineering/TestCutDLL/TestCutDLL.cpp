#include "stdafx.h"
using namespace std;
#include <iostream> 
#include <windows.h>
using namespace System;
using namespace System::IO;
#include "ecutdevice.h"

eCutDevice eCut;

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
		for (size_t i = 0; i < 200; i++)
		{
			result = eCutAddLine(cutHandler, &GeneratePositon(20, 0, 0), 5, 5, 5);
			eCutAddCircle(cutHandler, &GeneratePositon(20, 0, 0), &GenerateCartesian(15, 0, 0), &GenerateCartesian(0, 0, 1),0,5,5,5);
			eCutAddLine(cutHandler, &GeneratePositon(20, -20, 0), 5, 5, 5);
			eCutAddLine(cutHandler, &GeneratePositon(0, -20, 0), 5, 5, 5);
			eCutAddLine(cutHandler, &GeneratePositon(0, -0, 0), 5, 5, 5);
		}
		
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