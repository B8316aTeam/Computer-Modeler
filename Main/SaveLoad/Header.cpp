#include "Header.h"
#include <fstream>
using namespace std;

bool Save(char * path, SAVE_DATA data)
{
	ofstream fout(path, ios::out | ios::binary);

	fout.write((char*)&data.sorce_code, sizeof data.sorce_code);
	for (int i = 0; i < 8; i++)
	{
		fout.write((char*)&data.mach_state.registers[i], sizeof data.mach_state.registers[i]);
	}
	fout.write((char*)&data.mach_state.accum, sizeof data.mach_state.accum);
	fout.write((char*)&data.mach_state.com_counter, sizeof data.mach_state.com_counter);
	fout.write((char*)&data.mach_state.input_reg, sizeof data.mach_state.input_reg);
	fout.write((char*)&data.mach_state.is_end_work, sizeof data.mach_state.is_end_work);
	fout.write((char*)&*data.memory_state, sizeof *data.memory_state);
	fout.write((char*)&data.memory_size, sizeof data.memory_size);
		
	fout.close();
	system("pause");
	return 0;
}

SAVE_DATA Load(char * path)
{
	ifstream fin(path, ios::in | ios::binary);

	fin.read((char*)&data.sorce_code, sizeof data.sorce_code);
	for (int i = 0; i < 8; i++)
	{
		fin.read((char*)&data.mach_state.registers[i], sizeof data.mach_state.registers[i]);
	}
	fin.read((char*)&data.mach_state.accum, sizeof data.mach_state.accum);
	fin.read((char*)&data.mach_state.com_counter, sizeof data.mach_state.com_counter);
	fin.read((char*)&data.mach_state.input_reg, sizeof data.mach_state.input_reg);
	fin.read((char*)&data.mach_state.is_end_work, sizeof data.mach_state.is_end_work);
	fin.read((char*)&*data.memory_state, sizeof *data.memory_state);
	fin.read((char*)&data.memory_size, sizeof data.memory_size);

	fin.close();
	system("pause");
	return 0;
}