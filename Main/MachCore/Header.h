#pragma once
struct MACH_STATE
{
	int registers[8];
	int accum;
	unsigned int com_counter;
	int input_reg;
	bool full_reg;
};
extern "C" __declspec(dllexport) bool Tick(void * mach);
extern "C" __declspec(dllexport) void Reset(void * mach);
extern "C" __declspec(dllexport) void * Init(void * memory);
extern "C" __declspec(dllexport) void DeleteMach(void * mach);
extern "C" __declspec(dllexport) bool SetRegData(void * mach, unsigned int reg_id, int data);
extern "C" __declspec(dllexport) void SetCommandNumber(void * mach, unsigned int number);
extern "C" __declspec(dllexport) bool SetInput(void * mach, int data);
extern "C" __declspec(dllexport) bool SetAcum(void * mach, int data);
extern "C" __declspec(dllexport) int GetAcum(void * mach);
extern "C" __declspec(dllexport) unsigned GetCommandNumber(void * mach);
extern "C" __declspec(dllexport) int GetRegData(void * mach, unsigned int reg_id);
extern "C" __declspec(dllexport) int GetInput(void * mach);
extern "C" __declspec(dllexport) bool GetFullReg(void * mach);
extern "C" __declspec(dllexport) void * GetMachSate(void * mach);
extern "C" __declspec(dllexport) void SetMachState(void * mach, void * mach_state);
extern "C" __declspec(dllexport) void DeleteMachState(void * mach_state);
extern "C" __declspec(dllexport) void NewMemory(void * mach, void * memory);

