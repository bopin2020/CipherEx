# ecdh lib使用

> 感谢 https://github.com/kokke/tiny-ECDH-c  注意pr [issues/25](https://github.com/kokke/tiny-ECDH-c/pull/26)



> 新建一个工程项目  配置好include,lib目录

```c
#include <ecdh.h>
#include <ecdh_model.h>
#include <stdio.h>
#include <mac.h>

#define KEYLEN 24

void* generate_pub(char *publickey) {
	char privatekey[KEYLEN] = { 0 };
	if (random_bytes(privatekey, KEYLEN)) {
		hexdump("private key bytes", privatekey, KEYLEN);
	}
	ecdh_generate_keys(publickey, privatekey);
	hexdump("publickey key bytes", publickey, 2 * KEYLEN);

	return privatekey;
}

int main() {
	//ecdh_demo();

	// 1. 生成私钥
	char publickey[2 * KEYLEN] = { 0 };
	char *privatekey = generate_pub(publickey);

	// 2. other publickey
	char publickey_bob[2 * KEYLEN] = { 0 };
	generate_pub(publickey_bob);


	uint8_t share_key[2 * KEYLEN] = { 0 };
	ecdh_shared_secret(privatekey, publickey_bob, share_key);
	hexdump("shared key bytes", share_key, 2 * KEYLEN);
}
```



> 结果如下:



```
private key bytes
0000: 2f 46 53 b8 26 67 5e 49 72 6b 53 e4 99 55 09 99  /FS.&g^IrkS..U..
0016: be 39 47 60 f2 91 87 1f                          .9G`....
publickey key bytes
0000: 55 62 95 c4 e6 ca 69 7a 8d 82 bf cc da d2 22 ca  Ub....iz......".
0016: 2a a2 a3 5d 02 00 00 00 b5 58 d9 07 81 35 b5 19  *..].....X...5..
0032: dd 1a fe 4a 0f a2 83 b6 5d c1 ac 67 03 00 00 00  ...J....]..g....
private key bytes
0000: 8f 69 8f 5a 1d c2 2d 84 3e c0 0c af 89 04 52 39  .i.Z..-.>.....R9
0016: 97 7d 51 4f da 58 ae 16                          .}QO.X..
publickey key bytes
0000: 55 04 b5 37 9a fc 4a 86 e4 f6 dd fa 89 ba 60 49  U..7..J.......`I
0016: f1 e7 e9 0f 03 00 00 00 14 b8 a3 e0 e7 f0 c3 ed  ................
0032: 15 54 68 d9 2d 7c 56 ef 51 7f 44 2f 07 00 00 00  .Th.-|V.Q.D/....
shared key bytes
0000: ef d8 75 19 67 f9 39 1a 3d ce 04 a4 66 21 2f 7a  ..u.g.9.=...f!/z
0016: c7 cb 98 12 00 00 00 00 25 95 6f 98 d7 50 ed d6  ........%.o..P..
0032: 67 b6 7a af 5c 66 08 ad 77 6d 6f e2 03 00 00 00  g.z.\f..wmo.....
```

