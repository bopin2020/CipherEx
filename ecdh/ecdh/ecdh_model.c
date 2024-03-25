#include "ecdh_model.h"

/*
https://github.com/kokke/tiny-ECDH-c/blob/master/ecdh_example.c
*/

static uint32_t prng_rotate(uint32_t x, uint32_t k)
{
    return (x << k) | (x >> (32 - k));
}

static uint32_t prng_next(void)
{
    uint32_t e = prng_ctx.a - prng_rotate(prng_ctx.b, 27);
    prng_ctx.a = prng_ctx.b ^ prng_rotate(prng_ctx.c, 17);
    prng_ctx.b = prng_ctx.c + prng_ctx.d;
    prng_ctx.c = prng_ctx.d + e;
    prng_ctx.d = e + prng_ctx.a;
    return prng_ctx.d;
}

static void prng_init(uint32_t seed)
{
    uint32_t i;
    prng_ctx.a = 0xf1ea5eed;
    prng_ctx.b = prng_ctx.c = prng_ctx.d = seed;

    for (i = 0; i < 31; ++i)
    {
        (void)prng_next();
    }
}


bool random_bytes(void* addr, size_t len, uint32_t seed) {
    prng_init(seed);
    for (size_t i = 0; i < len; i++)
    {
        ((byte*)addr)[i] = (byte)prng_next();
    }
    return true;
}

bool random_bytes_ex(void* addr, size_t len) {
    return TryGenerateRandomBuffer(len, addr);
}

BOOL TryGenerateRandomBuffer(DWORD size, PVOID buffer)
{
    pRtlGenRandom rtlGenrandom = (pRtlGenRandom)GetProcAddress(GetModuleHandleA("advapi32.dll"), "SystemFunction036");
    if (rtlGenrandom) {
        return rtlGenrandom(buffer, size);
    }
}

void ecdh_demo(void) {
    // Anymore, we dont need demo code but find here => https://github.com/kokke/tiny-ECDH-c/blob/master/ecdh_example.c#L76
}