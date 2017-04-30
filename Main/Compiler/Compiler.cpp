#include "Compiler.h"

vector<wstring> CompareComWord(LPWSTR source,unsigned * symbol_id , unsigned * line, COMPARE_STATE * compare_state)
{
	vector<wstring> out;
	for (;(source[*symbol_id] != L';') || (source[*symbol_id] != L':'); (*symbol_id)++)
	{
		switch (source[*symbol_id])
		{
		case L'\0':
		{
			if (out.size() == 0)
				*compare_state = COMPARE_STATE::end;
			else
				*compare_state = COMPARE_STATE::error;
			return vector<wstring>();
		}
		case L'\n':
		{
			if (out.size() > 0)
			{
				*compare_state = COMPARE_STATE::error;
				return vector<wstring>();
			}
			else
			{
				(*line)++;
				(*symbol_id)++;
				*compare_state = COMPARE_STATE::not_end;
				return vector<wstring>();
			}
			break;
		}
		case L'\t':
		{
			if (out.size() > 0)
				if (out.back().size() > 0)
				{
					out.push_back(L"");
				}
			break;
		}
		case L' ':
		{
			if (out.size() > 0)
				if (out.back().size() > 0)
				{
					out.push_back(L"");
				}
			break;
		}
		case L':':
		{
			if (out.size() == 1)
			{
				out.push_back(L"contr_point");
				(*symbol_id)++;
				*compare_state = COMPARE_STATE::not_end;
				return out;
			}
			else
			{
				*compare_state = COMPARE_STATE::error;
				return vector<wstring>();
			}
			break;
		}
		case L';':
		{
			if (out.size() == 0)
			{
				*compare_state = COMPARE_STATE::error;
				return vector<wstring>();
			}
			else
			{
				if (out.back().size() == 0)
					out.pop_back();
				if (out.size() == 0)
				{
					*compare_state = COMPARE_STATE::error;
					return vector<wstring>();
				}
				else
				{
					(*symbol_id)++;
					*compare_state = COMPARE_STATE::not_end;
					return out;
				}
			}
			break;
		}
		default:
		{
			if ((source[*symbol_id] >= L'A') && (source[*symbol_id] <= L'z'))
			{
				if (out.size() == 0)
					out.push_back(L"");
				if ((source[*symbol_id] >= L'a') && (source[*symbol_id] <= L'z'))
					out.back() += source[*symbol_id];
				else
					out.back() += source[*symbol_id]+(L'a' - L'A');
				break;
			}
			if ((source[*symbol_id] >= L'0') && (source[*symbol_id] <= L'9'))
			{
				if (out.size() == 0)
				{
					//*compare_state = COMPARE_STATE::error;
					return vector<wstring>();
				}
				if (out.size() == 2)
				{
					out.back() = L"none";
					out.push_back(L"");
				}
				out.back() += source[*symbol_id];
			}
			if ((source[*symbol_id] == L'@') || (source[*symbol_id] <= L'#'))
				out.back() += source[*symbol_id];
		}
		}
	}
}

wchar_t * TestCompareComWord(LPWSTR source)
{
	vector <wstring> tmp;
	unsigned symbol_id = 0, line = 1;
	COMPARE_STATE compare_state = COMPARE_STATE::not_end;
	std::wstring out;
	unsigned last_size = 0;
	do
	{
		tmp = CompareComWord(source, &symbol_id, &line, &compare_state);
		if (compare_state == COMPARE_STATE::error)
		{
			out += L"error!!!\n";
			break;
		}
		else
		{
			for (auto iter = tmp.begin(); iter < tmp.end(); iter++)
			{
				out += (*iter);
				out += L" ";
			}
		}
		out += L'\n';
	} while (compare_state == COMPARE_STATE::not_end);
	out += L'\0';
	wchar_t * tmp_buf = new wchar_t[out.size()];
	memcpy_s(tmp_buf, sizeof(wchar_t) * out.size(), out.data(), sizeof(wchar_t) * out.size());
	return tmp_buf;
}

int Compile(LPWSTR source,void ** mach_source)
{
	// select memory for out data
	MACH_SOURCE * mach_source_out = new MACH_SOURCE;
	// id symbol in string
	unsigned symbol_id = 0, line = 1;
	bool is_error = false;
	// work if have source
	return 110;
}
