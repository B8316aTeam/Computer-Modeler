#pragma once
enum COMMAND
{
	htl = 0x00000000,
	add = 0x10000000,
	sub = 0x20000000,
	div = 0x30000000,
	mul = 0x40000000,
	jmp = 0x50000000,
	jmpz = 0x60000000,
	jmpnz = 0x70000000,
	jmplz = 0x80000000,
	jmpgz = 0x90000000,
	rd = 0xA0000000,
	wr = 0xB0000000,
	in = 0xC0000000,
	mod = 0xD0000000
};
enum ADRESS_TYPE
{
	none = 0x00000000,
	hash = 0x04000000,
	sob = 0x08000000,
	rez = 0x0C000000
};
enum SIGN_TYPE
{
	pos = 0x00000000,
	minus = 0x02000000
};