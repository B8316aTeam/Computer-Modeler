#pragma once
class MACH_CORE
{
public:
	int GetAccum();
	int GetRegData(unsigned id_reg);
	int GetFullState();
	int GetInputReg();
	void * GetMemory();
	unsigned GetComCount();
	// if end - true
	bool Tick();
private:
	int registers_[8];
	int accum_;
	int input_reg;
	unsigned com_counter_;
	void * memory_;
	int full_reg_;

};