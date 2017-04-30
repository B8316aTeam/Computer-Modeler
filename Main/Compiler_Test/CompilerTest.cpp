#include <iostream> 
#include <conio.h>
#include <windows.h>
int main() 
{
	LPWSTR test1 = L"htl; add 1;\n\n\n ADd @ 54; test: \n\t\t div 1; jmp tESt;\0",
		test2 = L"htl;\n add 1;\n ad\nd @ 54; test: \n div 1; jmp test;\0";
	// Определяем соответствующий указатель на ф-ю
	typedef wchar_t *(*Multiplies) (LPWSTR source);
	Multiplies mm;
	// Средствами WinAPI загружаем библиотеку, извлекаем
	// адрес функции в указатель
	HINSTANCE h = 0;
	h = LoadLibrary(L"Compiler.dll");
	mm = reinterpret_cast<wchar_t *(*)(LPWSTR source)>(GetProcAddress(h, "TestCompareComWord"));
	wchar_t * tmp = mm(test1);
	int i = 0;
	while (tmp[i] != L'\0')
	{
		std::wcout << tmp[i];
		i++;
	}
	delete tmp;
	_getch();
	return 0;
}