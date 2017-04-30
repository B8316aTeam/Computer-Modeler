#pragma once
#include <Windows.h>
#include <map>
#include <vector>
#include <string>
#include "ComSpecific.h"
using namespace std;
struct MACH_SOURCE
{
	int * commands;
	unsigned size = 0;
};
struct CONTR_POINTS
{
	// value
	unsigned target_line;
	// commands to which the value will be substituted
	vector <unsigned> places;
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
extern "C" __declspec(dllexport) wchar_t * TestCompareComWord(LPWSTR source);
extern "C" __declspec(dllexport) int Compile(LPWSTR source, void ** mach_source);
