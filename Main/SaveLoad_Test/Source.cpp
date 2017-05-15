#include <fstream>
#include<iostream>
#include<Windows.h>
#include<SaveLoad\Header.h>
using namespace std;

int main()
{
	typedef bool (*Save) (char * path, SAVE_DATA data);
	Save bar;
	HINSTANCE h = 0;
	h = LoadLibrary(L"SaveLoad.dll");
	bar = reinterpret_cast<bool (*)(char * path, SAVE_DATA data)>(GetProcAddress(h, "Save"));

	SAVE_DATA data2;
	data2.mach_state.accum = 6732;
		bar("registr.txt", data2);
	return 0;
}
