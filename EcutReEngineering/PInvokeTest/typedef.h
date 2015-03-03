/********************************************************************
	Created:	2007/11/02  14:52
	FileName: 	typedef.h	
	Author:		²ÌÐÂ²¨	
	Purpose:	
*********************************************************************/
#ifndef CAIXINBO_TYPEDEF_H
#define CAIXINBO_TYPEDEF_H
#ifdef __cplusplus
extern "C" {
#endif

#ifndef FALSE
#define FALSE 0
#endif

#ifndef TRUE
#define TRUE !FALSE
#endif

	typedef unsigned char  BOOLEAN;
	typedef unsigned char  INT8U;           /* Unsigned  8 bit quantity        */
	typedef signed   char  INT8S;            /* Signed    8 bit quantity        */
	typedef unsigned short INT16U;          /* Unsigned 16 bit quantity        */
	typedef signed   short INT16S;          /* Signed   16 bit quantity        */
	typedef unsigned int   INT32U;          /* Unsigned 32 bit quantity        */
	typedef signed   int   INT32S;          /* Signed   32 bit quantity        */
	typedef unsigned __int64 INT64U;
	typedef __int64		   INT64S;
	typedef float          FLOAT32;         /* Single precision floating point */
	typedef double         FLOAT64;         /* Double precision floating point  */
	typedef unsigned int size_t;

	typedef union  
	{
		struct
		{
			unsigned char Low;
			unsigned char High;		
		}bit;
		struct
		{
			INT8U byte0;
			INT8U byte1;
		}byte;
		unsigned short all;
		INT16S signedall;
	}uINT16U;

	typedef union  
	{
		struct
		{
			uINT16U Low;
			uINT16U High;		
		}bit;
		struct
		{
			INT8U byte0;
			INT8U byte1;
			INT8U byte2;
			INT8U byte3;
		}byte;
		unsigned int all;
		INT32S signedall;
		float fp;
	}uINT32U;
	typedef union  
	{
		struct
		{
			uINT32U Low;
			uINT32U High;
		}bit;
		struct
		{
			INT8U byte0;
			INT8U byte1;
			INT8U byte2;
			INT8U byte3;
			INT8U byte4;
			INT8U byte5;
			INT8U byte6;
			INT8U byte7;
		}byte;
		unsigned long long all;
		INT64S signedall;
		double db;
	}uINT64U;


#define CAIXINBO_malloc malloc
#define CAIXINBO_free free

#define ROM_VAR 
#define idata

#ifdef __cplusplus
}
#endif
#endif



