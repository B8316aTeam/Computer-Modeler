#include "Header.h"
#include <fstream>
using namespace std;

bool Save(char * path, SAVE_DATA data)
{
	ofstream fout(path, ios::out | ios::binary);
	//ïîäñ÷¸ò ðàçìåðà sorse_code è ñ÷èòûâàíèå ðàçìåðà â ôàéë è ñàìî ñëîâî
	int size_sorse_code = 0;
	while (data.sorce_code[size_sorse_code]!='\0')
	{
		size_sorse_code++;
	}
	fout.write((char*)&size_sorse_code, sizeof size_sorse_code);
	fout.write((char*)data.sorce_code, sizeof(wchar_t)*size_sorse_code);
	//111111111111111111111111111111111111
	//ðåãèñòð
	fout.write((char*)data.mach_state.registers, sizeof (int) * 8);
	//2222222222222222222222222222222222222222
	fout.write((char*)&data.mach_state.accum, sizeof data.mach_state.accum);
	fout.write((char*)&data.mach_state.com_counter, sizeof data.mach_state.com_counter);
	fout.write((char*)&data.mach_state.input_reg, sizeof data.mach_state.input_reg);
	fout.write((char*)&data.mach_state.is_end_work, sizeof data.mach_state.is_end_work);
	fout.write((char*)&data.memory_size, sizeof data.memory_size);
	fout.write((char*)data.memory_state, sizeof(int) * data.memory_size);
	fout.close();
	system("pause");
	return 0;
}

SAVE_DATA Load(char * path)
{
	SAVE_DATA data;
	ifstream fin(path, ios::in | ios::binary);
	//ñ÷èòûâàåò èç ôàéëà çíà÷åíèå ðàçìåðà sorce_code
	int size_sorse_code = 0;
	fin.read((char*)&size_sorse_code, sizeof size_sorse_code);
	data.sorce_code = new wchar_t[size_sorse_code];
	fin.read((char*)data.sorce_code, sizeof(wchar_t)*size_sorse_code);
	//1111111111111111111111111
	fin.read((char*)data.mach_state.registers, sizeof (int) * 8);
	//2222222222222222222222222
	fin.read((char*)&data.mach_state.accum, sizeof data.mach_state.accum);
	fin.read((char*)&data.mach_state.com_counter, sizeof data.mach_state.com_counter);
	fin.read((char*)&data.mach_state.input_reg, sizeof data.mach_state.input_reg);
	fin.read((char*)&data.mach_state.is_end_work, sizeof data.mach_state.is_end_work);
	fin.read((char*)&data.memory_size, sizeof data.memory_size);
	data.memory_state = new int[data.memory_size];
	fin.read((char*)data.memory_state, sizeof(int) * data.memory_size);
	
	fin.close();
	system("pause");
	return data;
}
