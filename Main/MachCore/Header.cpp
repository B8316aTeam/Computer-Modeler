#include "Header.h"
#include "MachCoreClass.h"

MACH_STATE Tick(void * mach)
{
	MACH_CORE * core = (MACH_CORE*)mach;
	MACH_STATE out;
	out.is_end_work = core->Tick();
	out.accum = core->GetAccum();
	out.com_counter = core->GetComCount();
	out.full_reg = core->GetInputReg();
	out.memory = core->GetMemory();
	for (short unsigned i = 0; i < 8; i++)
		out.registers[i] = core->GetRegData(i);
	return out;
}

void Reset(void * mach)
{
	MACH_CORE * core = (MACH_CORE*)mach;
	core->SetAccum(0);
	for (short unsigned i = 0; i < 8; i++)
		core->SetRegData(i, 0);
	core->SetInputReg(0);
	core->SetFullState(0);
	core->SetComcount(0);
}

void * Init(void * memory)
{
	MACH_CORE * core = new MACH_CORE(memory);
	Reset((void*)core);
	return core;
}

void DeleteMach(void * mach)
{
	delete (MACH_CORE*)mach;
}

bool SetRegData(void * mach, unsigned int reg_id, int data)
{
	return ((MACH_CORE*)mach)->SetRegData(reg_id,data);
}

void SetCommandNumber(void * mach, unsigned int number)
{
	((MACH_CORE *)mach)->SetComcount(number);
}

bool SetInput(void * mach, int data)
{
	if ((data <= MAX_NUMBER) && (data > (MAX_NUMBER *-1)))
	{
		((MACH_CORE *)mach)->SetInputReg(data);
		return false;
	}
	return true;
}

bool SetAcum(void * mach, int data)
{
	if ((data <= MAX_NUMBER) && (data > (MAX_NUMBER *-1)))
	{
		((MACH_CORE *)mach)->SetAccum(data);
		return false;
	}
	
	return true;
}
