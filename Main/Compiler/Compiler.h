#pragma once
#include <Windows.h>
struct MACH_SOURCE
{
	int * commands;
	unsigned int size;
};
extern "C" __declspec(dllexport) int Compile(LPWSTR source, MACH_SOURCE * mach_source);
