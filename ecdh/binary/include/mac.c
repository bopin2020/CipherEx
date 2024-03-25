#include "mac.h"

void
dump_data(const char* head, const void* s, size_t len, FILE* f)
{
	EnterCriticalSection(&g_cst);
	if (head != null)
		fprintf(f, "%s\n", head);
	size_t i, j;
	const u_char* p = (const u_char*)s;

	for (i = 0; i < len; i += 16) {
		fprintf(f, "%.4zu: ", i);
		for (j = i; j < i + 16; j++) {
			if (j < len)
				fprintf(f, "%02x ", p[j]);
			else
				fprintf(f, "   ");
		}
		fprintf(f, " ");
		for (j = i; j < i + 16; j++) {
			if (j < len) {
				if (isascii(p[j]) && isprint(p[j]))
					fprintf(f, "%c", p[j]);
				else
					fprintf(f, ".");
			}
		}
		fprintf(f, "\n");
	}
	LeaveCriticalSection(&g_cst);
}

void
dump_data2(const void* s, size_t len, FILE* f) {
	dump_data(null, s, len, f);
}

static const char* get_version() {
	return LibVersion;
}

ECDH_STATUS
ECDH_INIT() {
	InitializeCriticalSection(&g_cst);
	return ECDH_SUCCESS;
}

ECDH_STATUS
ECDH_INIT2(void* param) {
	InitializeCriticalSection(&g_cst);
	return ECDH_SUCCESS;
}

ECDH_STATUS
ECDH_UNINIT() {
	DeleteCriticalSection(&g_cst);
	return ECDH_SUCCESS;
}

ECDH_STATUS
ECDH_UNINIT2(void* param) {
	DeleteCriticalSection(&g_cst);
	return ECDH_SUCCESS;
}