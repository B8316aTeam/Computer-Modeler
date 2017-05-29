#include "Header.h"
#include <fstream>
using namespace std;

bool Save(wchar_t * path, SAVE_DATA data)
{
	fstream fout(path, ios::out | ios::binary);
	unsigned size_sourse_code = 0;
	while (data.sorce_code[size_sourse_code] != L'\0')
		size_sourse_code++;
	size_sourse_code++;
	fout.write((char*)&size_sourse_code, sizeof size_sourse_code);
	fout.write((char*)data.sorce_code, sizeof(wchar_t)*size_sourse_code);
	MACH_STATE * mach_state = (MACH_STATE *)data.mach_state;
	fout.write((char*)mach_state->registers, sizeof (int) * 8);
	fout.write((char*)&mach_state->accum, sizeof mach_state->accum);
	fout.write((char*)&mach_state->com_counter, sizeof mach_state->com_counter);
	fout.write((char*)&mach_state->input_reg, sizeof mach_state->input_reg);
	fout.write((char*)&mach_state->full_reg, sizeof mach_state->full_reg);
	MEMORY * memory = (MEMORY *)data.memory;
	fout.write((char*)&memory->size, sizeof(unsigned));
	fout.write((char*)memory->mem_, sizeof(int) *((MEMORY *)data.memory)->size);
	fout.close();
	return 0;
}

SAVE_DATA Load(wchar_t * path)
{
	SAVE_DATA data;
	ifstream fin(path, ios::in | ios::binary);
	int size_sorse_code = 0;
	fin.read((char*)&size_sorse_code, sizeof size_sorse_code);
	data.sorce_code = new wchar_t[size_sorse_code];
	fin.read((char*)data.sorce_code, sizeof(wchar_t)*size_sorse_code);
	//mach state
	MACH_STATE * mach_state = new MACH_STATE;
	fin.read((char*)mach_state->registers, sizeof (int) * 8);
	fin.read((char*)&mach_state->accum, sizeof mach_state->accum);
	fin.read((char*)&mach_state->com_counter, sizeof mach_state->com_counter);
	fin.read((char*)&mach_state->input_reg, sizeof mach_state->input_reg);
	fin.read((char*)&mach_state->full_reg, sizeof mach_state->full_reg);
	data.mach_state = (void *)mach_state;
	//mach memory
	data.memory = (void *) new MEMORY;
	fin.read((char*)&((MEMORY *)data.memory)->size, sizeof(unsigned));
	((MEMORY *)data.memory)->mem_ = new int[((MEMORY *)data.memory)->size];
	fin.read((char*)((MEMORY *)data.memory)->mem_, sizeof(int) *((MEMORY *)data.memory)->size);
	fin.close();
	return data;
}