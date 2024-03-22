#pragma once
#include "ecdh.h"
#include "mac.h"
/*
ecdh   kex exchange
*/
/* pseudo random number generator with 128 bit internal state... probably not suited for cryptographical usage */
typedef struct
{
	uint32_t a;
	uint32_t b;
	uint32_t c;
	uint32_t d;
} prng_t;
static prng_t prng_ctx;
static uint32_t prng_rotate(uint32_t x, uint32_t k);
static uint32_t prng_next(void);
static void prng_init(uint32_t seed);


bool random_bytes(void* addr, size_t len,uint32_t seed);
void ecdh_demo(void);