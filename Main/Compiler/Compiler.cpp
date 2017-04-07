#include "Compiler.h"
// 0 command, 1 number, 2 symbol, 3 end

vector<wstring> CompareComWord(LPWSTR source,unsigned * symbol_id, bool * is_error)
{
	vector<wstring> out;
	for (;(source[*symbol_id] != ';') || (source[*symbol_id] != ':'); (*symbol_id)++)
	{
		if (source[*symbol_id] == '\0')
		{
			if (out.size() > 0)
				*is_error = 1;
			else
				*is_error = 0;
			return vector<wstring>();
		}
		if (source[*symbol_id] != '\n')
		{
			if (out.size() > 0)
			{
				*is_error = 1;
				return vector<wstring>();
			}
			else
				continue;
		}

	}
	return out;
}

int Compile(LPWSTR source,void ** mach_source)
{
	// select memory for out data
	MACH_SOURCE * mach_source_out = new MACH_SOURCE;
	// id symbol in string
	unsigned symbol_id = 0, line = 1;
	// work if have source
	while (source != NULL)
	{
		
	}
	return 0;
}
