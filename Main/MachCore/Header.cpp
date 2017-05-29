#include "Header.h"
#include "MachCoreClass.h"
bool ChecData(int data)
{
	if (data < 0)
	{
		if ((data * -1) > MAX_NUMBER)
			return true;
	}
	else
		if (data > MAX_NUMBER)
			return true;
	return false;
}
bool Tick(void * mach)
{
	MACH_CORE * core = (MACH_CORE*)mach;
	return core->Tick();
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
	if (!ChecData(data))
	{
		return ((MACH_CORE*)mach)->SetRegData(reg_id, data);
	}
	return true;
}

void SetCommandNumber(void * mach, unsigned int number)
{
	((MACH_CORE *)mach)->SetComcount(number);
}

bool SetInput(void * mach, int data)
{
	if (!ChecData(data))
	{
		((MACH_CORE *)mach)->SetInputReg(data);
		return false;
	}
	return true;;
}

bool SetAcum(void * mach, int data)
{
	if (!ChecData(data))
	{
		((MACH_CORE *)mach)->SetAccum(data);
		return false;
	}
	
	return true;
}

int GetAcum(void * mach)
{
	return ((MACH_CORE *)mach)->GetAccum();
}

unsigned GetCommandNumber(void * mach)
{
	return ((MACH_CORE *)mach)->GetComCount();
}

int GetRegData(void * mach, unsigned int reg_id)
{
	return ((MACH_CORE *)mach)->GetRegData(reg_id);
}

int GetInput(void * mach)
{
	return ((MACH_CORE *)mach)->GetInputReg();
}

bool GetFullReg(void * mach)
{
	return ((MACH_CORE *)mach)->GetFullState();
}

void * GetMachSate(void * mach)
{
	MACH_STATE * mach_state_ = new MACH_STATE;
	MACH_CORE * mach_core_ = (MACH_CORE *)mach;
	mach_state_->accum = mach_core_->GetAccum();
	mach_state_->com_counter = mach_core_->GetComCount();
	mach_state_->full_reg = mach_core_->GetFullState();
	mach_state_->input_reg = mach_core_->GetInputReg();
	for (unsigned i = 0; i < 8; i++)
		mach_state_->registers[i] = mach_core_->GetRegData(i);
	return (void*)mach_state_;
}

void DeleteMachState(void * mach_state)
{
	MACH_STATE * mach_state_ = (MACH_STATE *)mach_state;
	delete mach_state_;
}

void NewMemory(void * mach, void * memory)
{
	((MACH_CORE *)mach)->SetNewMemory(memory);
}

void SetMachState(void * mach, void * mach_state)
{
	MACH_STATE * mach_state_ = (MACH_STATE *)mach_state;
	MACH_CORE * mach_core_ = (MACH_CORE *)mach;
	mach_core_->SetAccum(mach_state_->accum);
	mach_core_->SetComcount(mach_state_->com_counter);
	mach_core_->SetFullState(mach_state_->full_reg);
	mach_core_->SetInputReg(mach_state_->input_reg);
	for (unsigned i = 0; i < 8; i++)
	{
		mach_core_->SetRegData(i, mach_state_->registers[i]);
	}
}
