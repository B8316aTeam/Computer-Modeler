#include <iostream>
#include <conio.h>
#include <Windows.h>
using namespace std;
struct MACH_SOURCE
{
	int * commands_;
	unsigned size_ = 0;
	MACH_SOURCE(unsigned size)
	{
		commands_ = new int[size];
		size_ = size;
	}
};
int main()
{
	MACH_SOURCE test_data(5);
	test_data.commands_[0] = 1003;
	test_data.commands_[1] = 1003;
	test_data.commands_[2] = 15623;
	test_data.commands_[3] = 1123;
	test_data.commands_[4] = 11232;


	typedef bool(*GetData) (void * memory, unsigned id_cell, int * data);
	typedef bool(*SetData) (void * memory, unsigned id_cell, int data);
	typedef bool(*InitProgram) (void * memory, void * mach_source);
	typedef void *(*Init) (unsigned size);
	GetData get_data;
	SetData set_data;
	InitProgram init_program;
	Init init;
	HINSTANCE h = 0;
	h = LoadLibrary(L"MachMem.dll");
	get_data = reinterpret_cast<bool(*)(void * memory, unsigned id_cell, int * data)>(GetProcAddress(h, "GetData"));
	set_data = reinterpret_cast<bool(*)(void * memory, unsigned id_cell, int data)>(GetProcAddress(h, "SetData"));
	init_program = reinterpret_cast<bool(*)(void * memory, void * mach_source)>(GetProcAddress(h, "InitProgram"));
	init = reinterpret_cast<void *(*)(unsigned size)>(GetProcAddress(h, "Init"));
	void * memory = init(10);
	init_program(memory, (void*)&test_data);
	set_data(memory, 9, 10);
	for (unsigned i = 0; i < 10; i++)
	{
		int data = 0;
		get_data(memory, i, &data);
		cout << data << endl;
	}
	_getch();
	return 0;
}