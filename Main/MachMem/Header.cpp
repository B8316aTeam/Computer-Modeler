#include "Specific.h"
#include "Header.h"

bool GetData(void * memory, unsigned id_cell, int * data)
{
	MEMORY * memory_ = (MEMORY *)memory;
	if (memory_->size > id_cell)
	{
		*data = memory_->mem_[id_cell];
		return false;
	}
	return true;
}

bool SetData(void * memory, unsigned id_cell, int data)
{
	MEMORY * memory_ = (MEMORY *)memory;
	if (memory_->size > id_cell)
	{
		memory_->mem_[id_cell] = data;
		return false;
	}
	return true;
}

bool InitProgram(void * memory, void * mach_source)
{
	MEMORY * memory_ = (MEMORY *)memory;
	MACH_SOURCE * mach_source_ = (MACH_SOURCE *)mach_source;
	if (memory_->size < mach_source_->size_)
		return true;
	for (unsigned i = 0; i < mach_source_->size_; i++)
		memory_->mem_[i] = mach_source_->commands_[i];
	return false;
}

void * Init(unsigned size)
{
	MEMORY * out = new MEMORY;
	out->mem_ = new int[size];
	out->size = size;
	return (void *) out;
}

void DeleteMemory(void * memory)
{
	delete [] ((MEMORY *)memory)->mem_;
	delete (MEMORY *)memory;
}
