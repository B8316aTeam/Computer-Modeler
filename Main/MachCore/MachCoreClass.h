#pragma once
#include <Windows.h>
#include "ComSpecific.h"

typedef int(*_GetDataMemory) (void * memory, unsigned id_cell);
typedef bool(*_SetDataMemory) (void * memory, unsigned id_cell, int data);
class MACH_CORE
{
public:
	MACH_CORE(void * memory);
	int GetAccum();
	int GetRegData(unsigned id_reg);
	bool GetFullState();
	int GetInputReg();
	void SetNewMemory(void * memory);
	void * GetMemory();
	unsigned GetComCount();
	void SetAccum(int data);
	bool SetRegData(unsigned id_reg, int data);
	void SetInputReg(int data);
	void SetFullState(bool data);
	void SetComcount(unsigned data);
	// if end - true
	bool Tick();
private:
	HINSTANCE h = 0;
	_GetDataMemory GetDataMemory;
	_SetDataMemory SetDataMemory;
	int registers_[8];
	int accum_;
	int input_reg;
	unsigned com_counter_;
	void * memory_;
	bool full_reg_;
	bool is_error = false;
	int GetData(ADRESS_TYPE adress_type, SIGN_TYPE sign, int data);
	void SetData(ADRESS_TYPE adress_type, SIGN_TYPE sign, int data);
	int OutView(int number);
	int InView(int number);
};