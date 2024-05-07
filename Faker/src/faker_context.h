#pragma once
#include <Windows.h>
#include <stdio.h>
#include <assert.h>

typedef struct _task_state {
	const char* tag;
	enum {t_init,t_running,t_finished,} current_state;
	void* lock;
	// 初始时间
	// 结束时间
	// 初始回调函数
	// 结束回调函数
}task_state, * ptask_state;

typedef char UBYTE;

#define ARGCHK(x) assert(x)

#define init_sbox(num) static UBYTE sbox##num[256] = { 0x00 };
init_sbox(1)
init_sbox(2)
init_sbox(3)
init_sbox(4)
init_sbox(5)
init_sbox(6)
init_sbox(7)
init_sbox(8)

// magic
#define PNGR 0x32efabd8


#define ERROR_INVALID_ROUND			0xe0000001
#define ERROR_INVALID_KEYSIZE		0xe0000002


typedef struct _faker_context {
	unsigned int error_code;
	BYTE cipher_key[32];
	BYTE key_len;								// 32 bytes
	BYTE nonce_ctr[12];
}faker_context, * pfaker_context;

static void _encrypt_inernal(faker_context* ctx,BYTE data, BYTE* one, BYTE* two, int round);
void encrypt(faker_context* ctx,BYTE data, BYTE* one, BYTE* two, int round);

static void setup_faker(faker_context* ctx, BYTE* key);
