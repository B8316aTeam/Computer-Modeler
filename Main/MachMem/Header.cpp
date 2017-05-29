#include "Specific.h"
#include "Header.h"

int GetData(void * memory, unsigned id_cell)
{
	MEMORY * memory_ = (MEMORY *)memory;
	if (memory_->size > id_cell)
		return memory_->mem_[id_cell];
	else
		throw memory_->size;
}

bool SetData(void * memory, unsigned id_cell, int data)
{
	MEMORY * memory_ = (MEMORY *)memory;
	if (memory_->size > id_cell)
	{
		memory_->mem_[id_cell] = data;
		memory_->last_change_ = id_cell;
		return 0;
	}
	throw id_cell;
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
	for (unsigned i = 0; i < size; i++)
		out->mem_[i] = 0;
	return (void *) out;
}

void DeleteMemory(void * memory)
{
	delete [] ((MEMORY *)memory)->mem_;
	delete (MEMORY *)memory;
}

unsigned GetLastChange(void * memory)
{
	return ((MEMORY *)memory)->last_change_;
}
