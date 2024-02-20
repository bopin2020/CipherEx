# WipeID

> author:  bopin
>
> date: 2024-1
>
> description:   WipeID加密算法设计文档
>
> ps:
>
> 基于流密码设计，密钥流的随机性参考 RC*算法;   
>
> 加密方式基于原先RC4单纯的异或 代数加法运算更改为 乘法运算。
>
> RC4算法  S-box 范围的256，不安全, 使用一个变量和  s-box  来随机化密钥流。

> todo:    想到了更好的tips在修改把

* 引入随机数 nonce
* 使用非线性反馈移位寄存器保证  one-time pad



## How to use

```c#
// unit.test   Example_HowtoUse method
using (WipeID wi = new WipeID())
{
    wi.SetCipherStream("this is key");
    byte[] input = new byte[]
    {
        0x90,0x90,0x90,0x90,
        0x90,0x90,0x90,0x90,
    };
    using (MemoryStream outs = new MemoryStream())
    {
        byte[] output = wi.Encrypt(input,outs);

        using (MemoryStream outs2 = new MemoryStream())
        {
            CollectionAssert.AreEquivalent(input, wi.Decrypt(output, outs2));
        }
    }
}
```



## 优缺点

> 优点
>
> 基于乘法运算可以极大的混乱原始痕迹，比如字符串加密时消除明文字符，这一点是古典密码中无法具备的
>
> 对于异或门加密，因为**密钥的固定或密钥在Galois Field  伽罗瓦域即有限域**内，对于抗穷举效果不太好。
>
> 使用乘法运算，配合加法，并且扩大了密钥流的范围，仅用作学习目的。
>
> 缺点
>
> 作者数学基础薄弱，算法本身可能存在问题，不建议在严格保密场景下使用。但作为chacha20替代在一线和木马流量加密场景下不需要引用第三方库。（只要密钥流随机性不可预测，流密码的优势还是有发挥余地的）
>
> 因为使用乘法对原始数据进行了操作，不能算是标准的流密码模式



## Algorithm

### 密钥流生成器

```c
void rc4_init(unsigned char* s, unsigned char* key, unsigned long Len) //初始化函数
{
	int i = 0, j = 0;
	char k[256] = { 0 };
	unsigned char tmp = 0;
	for (i = 0; i < 256; i++)
	{
		/// S-box 初始化为 0-255的自然数  升序排列
		s[i] = i;

		/// t-box  当key 大于256时, 是无用的。因为key密钥是输入参数，t-box是随机的

		k[i] = key[i % Len]; // t-box使用key来进行填充的
	}
	/// 这里只是控制 S-box 密钥流的随机性
	for (i = 0; i < 256; i++)
	{
		/// 引入一个j变量   和 S-box  t-box  j   相加 mod 256

		j = (j + s[i] + k[i]) % 256; // j = 取一个s[i]+k[i]+j的索引
		//交换s[i]和s[j]
		/// 
		tmp = s[i];
		s[i] = s[j];
		s[j] = tmp;
	}
	printf("");
}

```

### 加解密算法

> 假设待加密的明文是:   WVsJv4b
>
> 共7个字节     偶数
>
> 明文记作:   p

```c
																		 6		mod 255
// 伪代码
p[0] * p[1]
    ushort值	 p[1]
    
p[2] * p[3]
    ushort值	 p[1]  ushort值2	 p[3]
    
p[4] * p[5]
    ushort值	 p[1]  ushort值2	 p[3]   ushort值3   p[5]		  此时密文序列共计9个字节
                                                                             		mod 65535

	
    4
ushort * ushort  ushort * ushort  ushort * ushort  ushort * ushort  1   
    6				6				6			  6
    [int  ushort]   [int  ushort]  [int  ushort]  [int  ushort]		1         25
    
    
                                                                             
    144  21   115
    136  110  121
    136  224  121                                                                         	
    
// 对于0字节数据加密
0x00 0x50
0x65 0x00
0x00 0x00 

	0x40 0x5c 0x00
    
最后一个字节（异或运算存储）
p[6] ^ 
```

> 算法存在漏洞时，程序就可能出现漏洞。即bug既有既无，无法调试。



* 当相乘时 有可能产生，左或右值为0的情况，









































































