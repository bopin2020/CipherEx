#pragma once
#include <windows.h>
#include <stdio.h>
#include <assert.h>

#define LibVersion "version 1.0"
#define VER 3
#define null NULL
#define bool BOOL
#define true TRUE
#define false FALSE

#define print(x)\
{\
	if(VER){\
		printf(x);\
	}\
}
#define printl(x) print(x)

#define DbgPrint(...) {char cad[512]; sprintf(cad, __VA_ARGS__);  OutputDebugStringA(cad);}
#define DbgPrintW(...) {WCHAR cad[512]; wsprintf(cad, __VA_ARGS__);  OutputDebugString(cad);}

#define si static byte
#define si2 static short
#define si4 static int
#define si8 static long
#define sui2 static unsigned short
#define sui4 static unsigned int
#define sui8 static unsigned long
#define ssize static size_t
#define sv static void
#define svp static void*
#define sb static bool

#define sp sftp_state* state

#define NSEC_IN_SEC 1000000000ULL // 10**9

#define	TIMEVAL_TO_TIMESPEC(tv, ts) {					\
	(ts)->tv_sec = (tv)->tv_sec;					\
	(ts)->tv_nsec = (tv)->tv_usec * 1000;				\
}
void
dump_data(const char* head, const void* s, size_t len, FILE* f);
void
dump_data2(const void* s, size_t len, FILE* f);
#define hexdump(h,s,len)\
{\
	if (VER) {\
		dump_data(h, s, len, stdout);\
	}\
}
#define hexdump2(s,len) dump_data2(s,len,stdout)

static const char* get_version();

static CRITICAL_SECTION g_cst;