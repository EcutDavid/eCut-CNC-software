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

void GerserialNumber()
{
	char serialNumber[12];
	eCutError result = GetDeviceInfo(0, serialNumber);//调用完毕后即可在serialNumber拿到板卡序列号
}

void SetStepsPerUnitSmoothCoff()
{
	//设置脉冲方向延时50,脉冲细分数32, X,Y,Z,A轴脉冲每毫米皆为400脉冲/毫米
	eCutError result = eCutSetStepsPerUnitSmoothCoff(eCut, 50, new int[9]{400, 400, 400, 400}, new int[9], 32);
}

void SmoothCoffStepsPerUnit()
{
	unsigned int smoothCoff;
	//eCutError result = eCutGetSmoothCoff(eCut, &smoothCoff);

	int stepsPerUnitArray[9];
	eCutError result = eCutGetStepsPerUnit(eCut, stepsPerUnitArray);
}

void SetCoordinate()
{
	double posArr[9] = { 30, 40, 50 };
	//配置 X轴机械坐标30mm Y轴机械坐标40mm Z轴机械坐标50mm
	eCutError result = eCutSetCoordinate(eCut, posArr);
}

void MaxSpeedAndSoftLimit()
{
	//double maxSpeedArr[9];
	//eCutError result = eCutGetMaxSpeed(eCut, maxSpeedArr);

	double maxSpeedArr[9] = {100, 100, 100, 100};
	double minSoftLimit[9] = { -100, -100, -100, -100 };
	eCutError result = eCutSetSoftLimit(eCut, maxSpeedArr, minSoftLimit);

	double acceleration[9] = {5, 5, 5, 5};
	double maxSpeed[9] = {10, 10, 10, 10};
	result = eCutSetAccelarationMaxSpeed(eCut, acceleration, maxSpeed);
}

void Move()
{
	char charArr[200];
	//X,Y,Z,A轴无限运动
	eCutMoveAtSpeed(eCut, 15, new double[9]{5, 5, 5, 5, 5}, new double[9]{5, 5, 5, 5, 5});
	//Y轴无限运动
	eCutMoveAtSpeed(eCut, 2, new double[9]{5, 5, 5, 5, 5}, new double[9]{5, 5, 5, 5, 5});

	eCutJogOn(eCut, 1,new double[9]{0, 50});
	eCutMoveAbsolute(eCut, 15, new double[9]{0, 50, 50, 50});
	while (true)
	{
		int steps[4];
		eCutGetSteps(eCut, steps);
		sprintf(charArr, "\n steps of x: %d y: %d b: %d", steps[0], steps[1], steps[4]);
		OutputDebugString(stringToLPCWSTR(charArr));
		Sleep(500);
	}
}

void CutPCMoveDone()
{
	eCutError result = eCutIsDone(eCut);
	eCutPosition Position;
	Position.x = 0; Position.y = 0; Position.z = 0;
	Position.a = 0; Position.b = 0; Position.c = 0;
	Position.u = 0; Position.v = 0; Position.w = 0;
	eCutSetCurrentPostion(eCut, &Position);
	Position.x = -5000; Position.y = 5; Position.z = 0;
	Position.a = 0; Position.b = 0; Position.c = 0;
	Position.u = 0; Position.v = 0; Position.w = 0;
	result = eCutAddLine(eCut, &Position, 5, 5, 5);
    result = eCutIsDone(eCut);
}

void GetInput()
{
	//在I5有输入，其它I无输入的情况下inputArr[0]得到32
	unsigned short inputArr[1];
	eCutGetInputIO(eCut, inputArr);
}

int _tmain(int argc, _TCHAR* argv[])
{
	char charArr[200];
	//TestDepthFunctionWithLine();
	eCutDevice cutHandler;
	eCut = eCutOpen(0);
	eCutConfigDeviceDefault(eCut);
	GetInput();
	eCutPosition Position;
	Position.x = 0; Position.y = 0; Position.z = 0;
	Position.a = 0; Position.b = 0; Position.c = 0;
	Position.u = 0; Position.v = 0; Position.w = 0;
	eCutSetCurrentPostion(eCut, new eCutPosition{-5000});
	Position.x = -5000; Position.y = 5; Position.z = 0;
	Position.a = 0 ; Position.b = 0; Position.c = 0;
	Position.u = 0 ; Position.v = 0; Position.w = 0;
	eCutAddLine(eCut, &Position, 5.0, 5.0, 5.0);
	while (true)
	{
		int steps[4];
		eCutGetSteps(eCut, steps);
		sprintf(charArr, "\n steps of x: %d %d", steps[0], eCutIsDone(eCut));
		OutputDebugString(stringToLPCWSTR(charArr));
		Sleep(500);
	}
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


LPCWSTR stringToLPCWSTR(string orig)
{
	size_t origsize = orig.length() + 1;
	const size_t newsize = 100;
	size_t convertedChars = 0;
	wchar_t *wcstring = (wchar_t *)malloc(sizeof(wchar_t)*(orig.length() - 1));
	mbstowcs_s(&convertedChars, wcstring, origsize, orig.c_str(), _TRUNCATE);
	return wcstring;
}