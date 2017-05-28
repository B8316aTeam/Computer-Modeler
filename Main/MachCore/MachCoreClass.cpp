#include "MachCoreClass.h"
#define CORE_ERROR L"Core_error"
int MACH_CORE::GetData(ADRESS_TYPE adress_type,SIGN_TYPE sign, int data)
{
	switch (adress_type)
	{
	case reg:
	{
		if (data > 8)
		{
			MessageBox(NULL, L"Out of register", CORE_ERROR, NULL);
			is_error = true;
			return 0;
		}
		else
			return OutView(registers_[data]);
	}
	case none:
	{
		try
		{
			return OutView(data|sign);
		}
		catch (unsigned size)
		{
			MessageBox(NULL, L"Out of memory", CORE_ERROR, NULL);
			is_error = true;
		}
		return 0;
	}
	case sob:
	{
		int out = 0;
		if (sign == pos)
		{
			try
			{
				out = GetDataMemory(memory_, data);
			}
			catch (unsigned size)
			{
				MessageBox(NULL, L"Out of memory", CORE_ERROR, NULL);
				is_error = true;
				return 0;
			}
			return GetData(none, pos, out);
		}
		else
		{
			if (data >= 8)
			{
				MessageBox(NULL, L"Out of registers", CORE_ERROR, NULL);
				is_error = true;
			}
			else
				out = registers_[data];
			return GetData(sob, pos, out);
		}

	}
	case hash:
	{
		int out = 0;
		if (sign == pos)
		{

			try
			{
				out = GetDataMemory(memory_, data);
			}
			catch (unsigned size)
			{
				MessageBox(NULL, L"Out of memory", CORE_ERROR, NULL);
				is_error = true;
			}
			return GetData(sob, pos, out);
		}
		else
		{
			if (data >= 8)
			{
				MessageBox(NULL, L"Out of registers", CORE_ERROR, NULL);
				is_error = true;
			}
			else
			{
				out = registers_[data];
				return GetData(hash, pos, out);
			}
		}
		break;
	}
	default:
		break;
	}
	return 0;
}

void MACH_CORE::SetData(ADRESS_TYPE adress_type, SIGN_TYPE sign, int data)
{
	switch (adress_type)
	{
	case reg:
	{
		if (data > 8)
		{
			MessageBox(NULL, L"Out of register", CORE_ERROR, NULL);
			is_error = true;
			break;
		}
		registers_[data] = accum_;
		break;
	}
	case none:
	{
		try
		{
			if (SetDataMemory(memory_, data, OutView(accum_)))
			{
				MessageBox(NULL, L"Out of memory", CORE_ERROR, NULL);
				is_error = true;
			}
		}
		catch (int i)
		{

		}
		break;
	}
	case sob:
	{
		int out = 0;
		if (sign == pos)
		{
			try
			{
				out = GetDataMemory(memory_, data);
			}
			catch (int id)
			{
				MessageBox(NULL, L"Out of memory", CORE_ERROR, NULL);
				is_error = true;
				return;
			}
		}
		else
		{
			if (data > 7)
			{
				MessageBox(NULL, L"Out of memory", CORE_ERROR, NULL);
				is_error = true;
				return;
			}
			out = registers_[data];
		}
		SetData(none, pos, out);
		break;
	}
	case hash:
	{
		int out = 0;
		if (sign == pos)
		{
			out = GetData(none, pos, data);
		}
		else
		{
			out = GetData(reg, pos, data);
		}
		SetData(sob, pos, out);
		break;
	}
	default:
		break;
	}
}

int MACH_CORE::OutView(int number)
{
	int out = number;
	if ((out >> 24) == 0)
		return out;
	else
	{
		if ((out & SIGN_TYPE::minus) == SIGN_TYPE::minus)
			out |= (-1 ^ MAX_NUMBER);
		return out;
	}
}

int MACH_CORE::InView(int number)
{
	int out = number & MAX_NUMBER;
	if (number < 0)
		out |= SIGN_TYPE::minus;
	return out;
}

MACH_CORE::MACH_CORE(void * memory)
{
	h = LoadLibrary(L"MachMem.dll");
	GetDataMemory = reinterpret_cast<int(*)(void * memory, unsigned id_cell)>(GetProcAddress(h, "GetData"));
	SetDataMemory = reinterpret_cast<bool(*)(void * memory, unsigned id_cell, int data)>(GetProcAddress(h, "SetData"));
	memory_ = memory;
	accum_ = 0;
	input_reg = 0;
	com_counter_ = 0;
	full_reg_ = false;
	for (short unsigned i = 0; i < 8; i++)
	{
		registers_[i] = 0;
	}
}

int MACH_CORE::GetAccum()
{
	return OutView(accum_);
}

int MACH_CORE::GetRegData(unsigned id_reg)
{
	return OutView(registers_[id_reg]);
}

int MACH_CORE::GetFullState()
{
	return full_reg_;
}

int MACH_CORE::GetInputReg()
{
	return OutView(input_reg);
}

void MACH_CORE::SetNewMemory(void * memory)
{
	memory_ = memory;
}

void * MACH_CORE::GetMemory()
{
	return memory_;
}

unsigned MACH_CORE::GetComCount()
{
	return com_counter_;
}

void MACH_CORE::SetAccum(int data)
{
	accum_ = InView(data);
}

bool MACH_CORE::SetRegData(unsigned id_reg, int data)
{
	if (id_reg < 8)
	{
		registers_[id_reg] = InView(data);
		return false;
	}
	else
		return true;
}

void MACH_CORE::SetInputReg(int data)
{
	input_reg = InView(data);
}

void MACH_CORE::SetFullState(bool data)
{
	is_error = data;
}

void MACH_CORE::SetComcount(unsigned data)
{
	com_counter_ = data;
}

bool MACH_CORE::Tick()
{
	int command = 0;
	try
	{
		command = GetDataMemory(memory_,com_counter_);
	}
	catch (unsigned size)
	{
		MessageBox(NULL, L"Out of memory", CORE_ERROR, NULL);
		is_error = true;
	}
	if (!is_error)
	{
		switch (command & 0xF0000000)
		{
		case htl:
		{
			throw 1;
		}
		case add:
		{
			int x1 = OutView(accum_),
				x2 = GetData(ADRESS_TYPE(command & reg), SIGN_TYPE(command & minus), command & MAX_NUMBER);
			x1 += x2;
			if ((x1 < (MAX_NUMBER *-1)) || (x1 > MAX_NUMBER))
				full_reg_ = 1;
			else
				full_reg_ = 0;
			accum_ = InView(x1);
			com_counter_++;
			break;
		}
		case sub:
		{
			int x1 = OutView(accum_),
				x2 = GetData(ADRESS_TYPE(command & reg), SIGN_TYPE(command & minus), command & MAX_NUMBER);
			x1 -= x2;
			if ((x1 < (MAX_NUMBER *-1)) || (x1 > MAX_NUMBER))
				full_reg_ = 1;
			else
				full_reg_ = 0;
			accum_ = InView(x1);
			com_counter_++;
			break;
		}
		case ndiv:
		{
			int x1 = OutView(accum_),
				x2 = GetData(ADRESS_TYPE(command & reg), SIGN_TYPE(command & minus), command & MAX_NUMBER);
			accum_ = InView(x1 / x2);
			com_counter_++;
			break;
		}
		case mul:
		{
			int x1 = OutView(accum_),
				x2 = GetData(ADRESS_TYPE(command & reg), SIGN_TYPE(command & minus), command & MAX_NUMBER);
			x1 -= x2;
			if ((x1 < (MAX_NUMBER *-1)) || (x1 > MAX_NUMBER))
				full_reg_ = 1;
			else
				full_reg_ = 0;
			accum_ = InView(x1);
			com_counter_++;
			break;
		}
		case mod:
		{
			int x1 = OutView(accum_),
				x2 = GetData(ADRESS_TYPE(command & reg), SIGN_TYPE(command & minus), command & MAX_NUMBER);
			accum_ = InView(x1 % x2);
			com_counter_++;
			break;
		}
		case jmp:
		{
			int tmp = GetData(ADRESS_TYPE(command & reg), SIGN_TYPE(command & minus), command & MAX_NUMBER);
			if (tmp < 0)
			{
				MessageBox(NULL, L"Error command number", CORE_ERROR, NULL);
				is_error = true;
				throw 1;
			}
			else
				com_counter_ = tmp;
			break;
		}
		case jmpz:
		{
			if (OutView(accum_) == 0)
			{
				int tmp = GetData(ADRESS_TYPE(command & reg), SIGN_TYPE(command & minus), command & MAX_NUMBER);
				if (tmp < 0)
				{
					MessageBox(NULL, L"Error command number", CORE_ERROR, NULL);
					is_error = true;
					throw 1;
				}
				else
					com_counter_ = tmp;
			}
			else
				com_counter_++;
			break;
		}
		case jmpnz:
		{
			if (OutView(accum_) != 0)
			{
				int tmp = GetData(ADRESS_TYPE(command & reg), SIGN_TYPE(command & minus), command & MAX_NUMBER);
				if (tmp < 0)
				{
					MessageBox(NULL, L"Error command number", CORE_ERROR, NULL);
					is_error = true;
					throw 1;
				}
				else
					com_counter_ = tmp;
			}
			else
				com_counter_++;
			break;
		}
		case jmplz:
		{
			if (OutView(accum_) < 0)
			{
				int tmp = GetData(ADRESS_TYPE(command & reg), SIGN_TYPE(command & minus), command & MAX_NUMBER);
				if (tmp < 0)
				{
					MessageBox(NULL, L"Error command number", CORE_ERROR, NULL);
					is_error = true;
					throw 1;
				}
				else
					com_counter_ = tmp;
			}
			else
				com_counter_++;
			break;
		}
		case jmpgz:
		{
			if (OutView(accum_) > 0)
			{
				int tmp = GetData(ADRESS_TYPE(command & reg), SIGN_TYPE(command & minus), command & MAX_NUMBER);
				if (tmp < 0)
				{
					MessageBox(NULL, L"Error command number", CORE_ERROR, NULL);
					is_error = true;
					throw 1;
				}
				else
					com_counter_ = tmp;
			}
			else
				com_counter_++;
			break;
		}
		case rd:
		{
			accum_ = GetData(ADRESS_TYPE(command & reg), SIGN_TYPE(command & minus), command & MAX_NUMBER);
			com_counter_++;
			break;
		}
		case wr:
		{
			SetData(ADRESS_TYPE(command & reg), SIGN_TYPE(command & minus), command & MAX_NUMBER);
			com_counter_++;
			break;
		}
		case in:
		{
			accum_ = input_reg;
			com_counter_++;
			break;
		}
		default:
			MessageBox(NULL, L"Unknow command", CORE_ERROR, NULL);
			is_error = true;
			throw 1;
		}
	}
	return false;
}
