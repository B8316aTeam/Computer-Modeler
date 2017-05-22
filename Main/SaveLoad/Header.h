#pragma once
#include<Windows.h>

struct MACH_STATE 
{
	int registers[8];
	int accum;
	unsigned int com_counter;
	int input_reg;
	bool is_end_work;
};

struct SAVE_DATA
{
	LPWSTR sorce_code;
	MACH_STATE mach_state;
	int * memory_state;
	unsigned int memory_size;
};

extern "C" __declspec(dllexport) bool Save(wchar_t * path, SAVE_DATA data);

extern "C" __declspec(dllexport) SAVE_DATA Load(wchar_t * path);