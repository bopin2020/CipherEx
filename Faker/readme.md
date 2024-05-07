# Faker

> faker 加密算法是流密码方式
>
> 参考serpent加密算法，serpent本身是AES海选最终入围AES的五大算法之一，并且仅次于Rijdanel 排名第二。
>
> 安全性方面甚至是最强的，因为它的加密运算级别在bit上，其他加密算法在byte字节上，但是也导致了serpent运算速度太慢，从而落选了AES，与冠军失之交臂。
>
> 我的目的是基于serpent加密粒度，设计一个新的加密算法 - 流密码模式。这里不同于serpent块加密模式，不支持128，192，256三种。
>
> 基于流密码，同时将加密粒度控制在bit层，并且保证同一套数据密文输出不同，兼容速度与安全性考虑。

## S-box

> 初始化   8个 S-box    [256]

```
 S-box   key[8] = {
	s1[256]{},
	s2[256]{},
	s3[256]{},
	s4[256]{},
	s5[256]{},
	s6[256]{},
	s7[256]{},
	s8[256]{},
}
```

> https://github.com/kberger/serpent/blob/master/src/Serpent.java#L254

```plain
0 0 0 1 0 0 0 1
0 0 0 0 0 0 0 1     1bytes 		 高四位	低四位
0 0 0 1 0 0 0 0
0 0 1 0 0 0 1 0
0 1 0 0 0 1 0 0
1 0 0 0 1 0 0 0

0 0 0 1 0 0 0 0		左移
```



> 初始化32key   密钥
>
> 8bit     

```
0x90,0x80,0xde,0x34			32bits    拆分成8组参与运算  输出也是32bits
```



> https://github.com/libtom/libtomcrypt/blob/develop/src/ciphers/serpent.c#L495



```flow
s=>start: 开始
e=>end: end

op1=>operation: 设置sbox
op2=>operation: linear transformation


s->op1->op2
```







