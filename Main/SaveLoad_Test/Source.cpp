#include <fstream>
#include<iostream>
#include<Windows.h>
#include<SaveLoad\Header.h>
using namespace std;

int main()
{
	typedef bool (*Save) (char * path, SAVE_DATA data);
	typedef SAVE_DATA (*Load) (char * path);
	Save save;
	Load load;
	HINSTANCE h = 0;
	h = LoadLibrary(L"SaveLoad.dll");
	save = reinterpret_cast<bool (*)(char * path, SAVE_DATA data)>(GetProcAddress(h, "Save"));
	load = reinterpret_cast<SAVE_DATA(*)(char * path)>(GetProcAddress(h, "Load"));
	SAVE_DATA * data2 = new SAVE_DATA;
	data2->sorce_code = L"hello";
	data2->memory_size = 2;
	data2->memory_state = new int[2];
	data2->memory_state[0] = 10;
	data2->memory_state[1] = 6;
	data2->mach_state.accum = 6732;
	data2->mach_state.com_counter = 10;
	data2->mach_state.input_reg = 67;
	data2->mach_state.is_end_work = true;
	data2->mach_state.registers[0] = 5678;
	data2->mach_state.registers[1] = 5678;
	data2->mach_state.registers[2] = 5678;
	data2->mach_state.registers[3] = 5678;
	data2->mach_state.registers[4] = 5678;
	data2->mach_state.registers[5] = 5678;
	data2->mach_state.registers[6] = 5678;
	data2->mach_state.registers[7] = 5678;
	save("registr.txt", *data2);
	delete data2;
	data2 = new SAVE_DATA;
	*data2 = load("registr.txt");
	return 0;
}
