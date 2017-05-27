#pragma once
#include<Windows.h>

struct MACH_STATE 
{
	int registers[8];
	int accum;
	unsigned int com_counter;
	int input_reg;
	bool full_reg;
};

struct SAVE_DATA
{
	LPWSTR sorce_code;
	void * mach_state;
	void * memory;
};
struct MEMORY
{
	int * mem_;
	unsigned size;
	unsigned last_change_ = 0;
};
extern "C" __declspec(dllexport) bool Save(wchar_t * path, SAVE_DATA data);
extern "C" __declspec(dllexport) SAVE_DATA Load(wchar_t * path);