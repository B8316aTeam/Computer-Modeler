#pragma once
#include <Windows.h>
#include <map>
#include <vector>
#include <string>
#include "ComSpecific.h"
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
struct RAW_COM
{
	vector <wstring> command_;
	unsigned line_;
	RAW_COM();
	RAW_COM(vector <wstring> command, unsigned line);
};
struct CONTR_POINTS
{
	// value
	unsigned target_line;
	// name
	wstring name;
};
enum COMPARE_STATE
{
	error,
	end,
	not_end
};
// result 1 - command error;
//int Command(wstring source);
//int Adress(wstring source);
//int Value(wstring source, bool * error);
vector <wstring> CompareComWord(LPWSTR source,unsigned * symbol_id, unsigned * line, COMPARE_STATE * compare_state);
unsigned CompileMachSource(vector<RAW_COM> commands, void ** mach_source);
int StringToNumber(wstring data, bool * is_error, bool * is_reg);
extern "C" __declspec(dllexport) wchar_t * TestCompareComWord(LPWSTR source);
extern "C" __declspec(dllexport) int Compile(LPWSTR source, void ** mach_source);
extern "C" __declspec(dllexport) void DeleteMachSource(void * mach_source);
