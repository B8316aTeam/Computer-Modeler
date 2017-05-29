#pragma once
#include <Windows.h>
struct MEMORY
{
	int * mem_ = 0;
	unsigned size = 0;
	unsigned last_change_ = 0;
};
struct MACH_SOURCE
{
	int * commands_;
	unsigned size_ = 0;
	LPWSTR commands_list;
	MACH_SOURCE(unsigned size)
	{
		commands_ = new int[size];
		size_ = size;
	}
};