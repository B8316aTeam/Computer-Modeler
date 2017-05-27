#pragma once
extern "C" __declspec(dllexport) int GetData(void * memory, unsigned id_cell);
extern "C" __declspec(dllexport) bool SetData(void * memory, unsigned id_cell, int data);
extern "C" __declspec(dllexport) bool InitProgram(void * memory ,void * mach_source);
extern "C" __declspec(dllexport) void * Init(unsigned size);
extern "C" __declspec(dllexport) void DeleteMemory(void * memory);
extern "C" __declspec(dllexport) unsigned GetLastChange(void * memory);