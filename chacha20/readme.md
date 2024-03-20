## chacha20

> chacha20  流密码模式

```c
struct chacha20_context ctx2 = { 0 };

uint8_t key[32] = { 142, 26, 14, 68, 43, 188, 234, 12, 73, 246, 252, 111, 8, 227, 57, 22, 168, 140, 41, 18, 91, 76, 181, 239, 95, 182, 248, 44, 165, 98, 34, 12 };
uint8_t nonce[12] = { 139, 164, 65, 213, 125, 108, 159, 118, 252, 180, 33, 88 };
uint64_t counter = 1;

chacha20_init_context(&ctx2, key, nonce, counter);
chacha20_xor(&ctx2, (uint8_t*)lpBuffer, lpdwNumberOfBytesRead);
```

