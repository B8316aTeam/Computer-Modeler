#pragma once
struct MACH_STATE
{
	int registers[8];
	int accum;
	unsigned int com_counter;
	unsigned int input_reg;
	bool is_end_work;
	int full_reg;
	void * memory;
};
extern "C" __declspec(dllexport) MACH_STATE Tick(void * mach);
extern "C" __declspec(dllexport) void Reset(void * mach);
extern "C" __declspec(dllexport) void * Init(void * memory);
extern "C" __declspec(dllexport) void DeleteMach(void * mach);
extern "C" __declspec(dllexport) bool SetRegData(void * mach, unsigned int reg_id, int data);
extern "C" __declspec(dllexport) void SetCommandNumber(void * mach, unsigned int number);
extern "C" __declspec(dllexport) bool SetInput(void * mach, int data);
extern "C" __declspec(dllexport) bool SetAcum(void * mach, int data);
