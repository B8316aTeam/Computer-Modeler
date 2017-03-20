#include "Compiler.h"
#include <stack>
#include <string>
#include <vector>
using namespace std;
int Compile(LPWSTR source, MACH_SOURCE * mach_source)
{
	/*
	
	
		stack <string> command;
		string oper;
		
	}*/
	// select memory for out data
	MACH_SOURCE * mach_source_out = new MACH_SOURCE;
	// id symbol in string
	unsigned int symbol_id = 0, line = 0;
	// work if have source
	while (source != NULL)
	{
		// form instruction
		// detect adress type
		// detect sign
		// form value
		// detect end comm
		// detect new line
	}
	return 0;
}
