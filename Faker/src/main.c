#include "faker_context.h"


void encrypt(faker_context* ctx,BYTE data, BYTE* one, BYTE* two, int round) {
	ARGCHK(ctx != NULL);
	if (round < 0 || round > 256) { ctx->error_code = ERROR_INVALID_ROUND; return; }

	_encrypt_inernal(&ctx,data,one,two,round);
}

static void _encrypt_inernal(faker_context* ctx,BYTE data, BYTE* one, BYTE* two,int round) {
	int step = (round % 4);// == 0 ? (round % 4) + 1 : (round % 4);
	*one = data >> step;
	*two = data << step;

	switch (round % 8)
	{
	case 0:
		*one = *one ^ sbox1[*one];
		break;
	case 1:
		*one = *one ^ sbox2[*one];
		break;
	case 2:
		*one = *one ^ sbox3[*one];
		break;
	case 3:
		*one = *one ^ sbox4[*one];
		break;
	case 4:
		*one = *one ^ sbox5[*one];
		break;
	case 5:
		*one = *one ^ sbox6[*one];
		break;
	case 6:
		*one = *one ^ sbox7[*one];
		break;
	case 7:
		*one = *one ^ sbox8[*one];
		break;
	default:
		break;
	}
}

int main() {
	BYTE data[] = { 0x4a,0x4a,0x4a,0x4a,0x4a,0x4a,0x4a,0x4a ,0x4a,0x4a,0x4a,0x4a ,0x4a,0x4a,0x4a,0x4a,0x00 };
	faker_context ctx = { 0 };
	const char* key = "thisisakeybopin6thisisakeybopin6";
	setup_faker(&ctx, (BYTE*)key);
	for (size_t i = 0; i < 16; i++)
	{
		BYTE one = 0x00;
		BYTE two = 0x00;
		encrypt(&ctx,data[i],&one,&two,i);
		BYTE d = (one << i % 4) | (two >> i % 4);
		if (d > 256) {

		}
		printf("%c\n%c", one,two);
		printf("\n=> %c",d);

	}
	printf("\n%s\n", data);
}


static void setup_faker(faker_context* ctx, BYTE* key) {
#pragma region 初始化s-box

	for (size_t i = 0; i < 32; i++)
	{
		if (i > 1)
			sbox1[i] = key[i] ^ 0x5e & sbox1[i - 1];
		else
			sbox1[i] = key[i] ^ 0x49;
	}

	for (size_t i = 32; i < 256; i++)
	{
		// todo  sbox初始化线性变化会呈现一定规律性
		sbox1[i] = sbox1[i - 32] << 2 ^ 0x48 & sbox1[i-5];
	}

	for (size_t i = 0; i < 256; i++)
	{
		sbox2[i] = sbox1[i] ^ key[i % 32];
	}

	for (size_t i = 0; i < 256; i++)
	{
		sbox3[i] = sbox2[i] ^ sbox1[i % 64];
	}

	for (size_t i = 0; i < 256; i++)
	{
		sbox4[i] = sbox3[i] ^ sbox2[i % 64];
	}

	for (size_t i = 0; i < 256; i++)
	{
		sbox5[i] = sbox4[i] ^ sbox3[i % 64];
	}

	for (size_t i = 0; i < 256; i++)
	{
		sbox6[i] = sbox5[i] ^ sbox4[i % 64];
	}

	for (size_t i = 0; i < 256; i++)
	{
		sbox7[i] = sbox6[i] ^ sbox5[i % 64];
	}

	for (size_t i = 0; i < 256; i++)
	{
		sbox8[i] = sbox7[i] ^ sbox6[i % 64];
	}

#pragma endregion

#pragma region ctx设置
	CopyMemory(ctx->cipher_key, key, 32);
	ctx->key_len = 32;
	ctx->nonce_ctr[0] = ctx->cipher_key[2] ^ ctx->cipher_key[1];
	ctx->nonce_ctr[1] = ctx->cipher_key[3] ^ ctx->cipher_key[12];
	ctx->nonce_ctr[2] = ctx->cipher_key[5] ^ ctx->cipher_key[29];
	ctx->nonce_ctr[3] = ctx->cipher_key[1] ^ ctx->cipher_key[13];
	ctx->nonce_ctr[4] = ctx->cipher_key[4] ^ ctx->cipher_key[5];
	ctx->nonce_ctr[5] = ctx->cipher_key[6] ^ ctx->cipher_key[16];
	ctx->nonce_ctr[6] = ctx->cipher_key[7] ^ ctx->cipher_key[8];
	ctx->nonce_ctr[7] = ctx->cipher_key[0] ^ ctx->cipher_key[4];
	ctx->nonce_ctr[8] = (BYTE)(PNGR && 0xff);
	ctx->nonce_ctr[9] = (BYTE)((PNGR && 0xff00) >> 8);
	ctx->nonce_ctr[10] = (BYTE)((PNGR && 0xff0000) >> 16);
	ctx->nonce_ctr[11] = (BYTE)((PNGR && 0xff000000) >> 24);
#pragma endregion

	printf("");
}