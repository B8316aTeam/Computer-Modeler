#include <iostream> 
#include <conio.h>
#include <windows.h>
#define COUNT_TESTS 5
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
	LPWSTR tests[COUNT_TESTS] = { L"htl;\nrtest :\n add # r1;\n ADd 1; \ndiv -1;\n jmp rtESt;\n jmpz @ 54;\n jmp # R34;\n add r5;\njmplz r54;\n jmp 89;\n",
		L"htl;\nrtest :\n add # rtr;\n ADd 1; \ndiv -1;\n jmp rtESt;\n jmpz @ 54;\n jmp # R34;\n add r5;\njmplz r54;\n jmp 89;\n",
		L"htl;\nrtest :\n add # r1;\n\n\n ADd 1; \ndiv -1;\n jmpf rtESt;\n jmpz @ 54;\n jmp # R34;\n add r5;\njmplz r54;\n jmp 89;\n",
		L"htl;\nrtest :\n add # r1;\n ADd 1; \ndiv -1;\n jmp rtESt;\n jmpz @ 54;\n jmp # R34;\n add r5;\njmplz r54;\n jmp ds 89;\n",
		L"htl;\n\nrtest :\n add # r1;\n\n ADd doc 1; \ndiv -1;\n jmp rtESt;\n jmpz @ 54;\n jmp # R34;\n add r5;\njmplz r54;\n jmp 89;\n" };
	// Определяем соответствующий указатель на ф-ю
	typedef wchar_t *(*Multiplies) (LPWSTR source);
	typedef int (*TestFunc) (LPWSTR source, void ** mach_source);
	Multiplies mm;
	TestFunc compile;
	 /*Средствами WinAPI загружаем библиотеку, извлекаем
	 адрес функции в указатель*/
	HINSTANCE h = 0;
	h = LoadLibrary(L"Compiler.dll");
	mm = reinterpret_cast<wchar_t *(*)(LPWSTR source)>(GetProcAddress(h, "TestCompareComWord"));
	compile = reinterpret_cast<int (*)(LPWSTR source, void ** mach_source)>(GetProcAddress(h, "Compile"));
	for (int test_num = 0; test_num < COUNT_TESTS; test_num++)
	{
		wchar_t * tmp = mm(tests[test_num]);
		int i = 0, line = 0;
		std::wcout << line << L": ";
		while (tests[test_num][i] != L'\0')
		{
			std::wcout << tests[test_num][i];
			if (tests[test_num][i] == L'\n')
			{
				line++;
				std::wcout << line << L": ";
			}
			i++;
		}
		i = 0;
		line = 0;
		std::wcout << L"\n----------------------------------\n";
		std::wcout << line << L": ";
		line++;
		while (tmp[i] != L'\0')
		{
			std::wcout << tmp[i];
			if (tmp[i] == L'\n')
			{
				std::wcout << line << L": ";
				line++;
			}
			i++;
		}
		MACH_SOURCE * mach_sorce;
		int err_line = compile(tests[test_num], (void**)&mach_sorce);
		std::wcout << L"\nError line :" <<std::dec <<err_line;
		std::wcout << L"\n----------------------------------\n";
		for (unsigned i = 0; i < mach_sorce->size_; i++)
		{
			std::wcout <<std::dec<<i << L": ";
			std::wcout <<std::hex << mach_sorce-> commands_[i]<< std::endl;
		}
		delete tmp;
		std::wcout << std::endl;
	}
	_getch();
	return 0;
}