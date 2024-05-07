#pragma once
#ifndef _LZNT1_H
#define _LZNT1_H

/*
 * Copyright (C) 2012 Michael Brown <mbrown@fensystems.co.uk>.
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA
 * 02110-1301, USA.
 */

 /**
  * @file
  *
  * LZNT1 decompression
  *
  */

#include <stdint.h>

typedef size_t ssize_t;
typedef char byte;

  /** Extract LZNT1 block length */
#define LZNT1_BLOCK_LEN( header ) ( ( (header) & 0x0fff ) + 1 )

/** Determine if LZNT1 block is compressed */
#define LZNT1_BLOCK_COMPRESSED( header ) ( (header) & 0x8000 )

/** Extract LZNT1 compressed value length */
#define LZNT1_VALUE_LEN( tuple, split ) \
	( ( (tuple) & ( ( 1 << (split) ) - 1 ) ) + 3 )

/** Extract LZNT1 compressed value offset */
#define LZNT1_VALUE_OFFSET( tuple, split ) ( ( (tuple) >> split ) + 1 )

ssize_t lznt1_block(const byte* data, size_t limit, size_t offset,void* block);
extern ssize_t lznt1_decompress(const void* data, size_t len, void* buf);

#endif /* _LZNT1_H */

/*


O1.团队安全工具开发工作
KR1: 
KR1: 

O2.相关工具免杀支撑工作

O3. 团队技术攻坚能力支撑

O4. 其他功能
*/