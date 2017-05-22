#pragma once
struct MEMORY
{
	int * mem_;
	unsigned size;
};
struct MACH_SOURCE
{
	int * commands_;
	unsigned size_ = 0;
	MACH_SOURCE(unsigned size)
	{
		commands_ = new int[size];
		size_ = size;
	}
};