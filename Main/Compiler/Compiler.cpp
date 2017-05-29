#include "Compiler.h"
#define ERROR_CAP L"Ошибка компиляции"
#define ERROR_MESSAGE(text,var) (wstring(text)+ var).data()

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
			{
				*compare_state = COMPARE_STATE::error;
				MessageBox(NULL, ERROR_MESSAGE(L"Неожиданое завершение команды", out[0]), ERROR_CAP, NULL);
			}
			return vector<wstring>();
		}
		case L'\n':
		{
			if (out.size() > 0)
			{
				*compare_state = COMPARE_STATE::error;
				MessageBox(NULL, ERROR_MESSAGE(L"Неожиданое завершение команды", out[0]), ERROR_CAP, NULL);
				return vector<wstring>();
			}
			else
			{
				(*line)++;
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
			if ((out.size() == 1) || (out.size() == 2) && (out.back().empty()))
			{
				if (out.size() == 1)
					out.push_back(L" ");
				else
					out.back() = L" ";
				(*symbol_id)++;
				*compare_state = COMPARE_STATE::not_end;
				return out;
			}
			else
			{
				*compare_state = COMPARE_STATE::error;
				MessageBox(NULL, ERROR_MESSAGE(L"Неожиданое завершение команды", out[0]), ERROR_CAP, NULL);
				return vector<wstring>();
			}
			break;
		}
		case L';':
		{
			if (out.size() == 0)
			{
				*compare_state = COMPARE_STATE::error;
				MessageBox(NULL, L"Отсутсвует команда", ERROR_CAP, NULL);
				return vector<wstring>();
			}
			else
			{
				if (out.back().size() == 0)
					out.pop_back();
				if (out.size() == 0)
				{
					*compare_state = COMPARE_STATE::error;
					MessageBox(NULL, L"Отсутсвует команда", ERROR_CAP, NULL);
					return vector<wstring>();
				}
				else
				{
					(*symbol_id)++;
					*compare_state = COMPARE_STATE::not_end;
					if (out.back().empty())
						out.pop_back();
					return out;
				}
			}
			break;
		}
		default:
		{
			if (out.size() == 0)
				out.push_back(L"");
			if ((source[*symbol_id] >= L'A') && (source[*symbol_id] <= L'z'))
			{
				if ((out.size() == 2) && (out.back().empty()))
				{
					if ((out[0] == L"jmp") || 
						(out[0] == L"jmpz") || 
						(out[0] == L"jmpnz") || 
						(out[0] == L"jmplz") ||  
						(out[0] == L"jmpgz"))
					{
						out.back() = L"cp";
						out.push_back(L"");
					}
					else
					{
						if ((source[*symbol_id] == L'R') || (source[*symbol_id] == L'r'))
						{
							out.back() = L"none";
							out.push_back(L"");
						}
						else
						{
							*compare_state = COMPARE_STATE::error;
							MessageBox(NULL, ERROR_MESSAGE(L"Некорректная команда ", out[0]), ERROR_CAP, NULL);
							return vector<wstring>();
						}
					}
				}
				if ((source[*symbol_id] >= L'a') && (source[*symbol_id] <= L'z'))
					out.back() += source[*symbol_id];
				else
					out.back() += source[*symbol_id]+(L'a' - L'A');
				break;
			}
			if ((source[*symbol_id] >= L'0') && (source[*symbol_id] <= L'9') || (source[*symbol_id] == L'-'))
			{
				if (out.back().empty())
				{
					if (out.size() == 1)
					{
						*compare_state = COMPARE_STATE::error;
						MessageBox(NULL, ERROR_MESSAGE(L"Некорректная команда ", out[0]), ERROR_CAP, NULL);
						return vector<wstring>();
					}
					if (out.size() == 2)
					{
						out.back() = L"none";
						out.push_back(L"");
					}
				}
				else
				{
					if (source[*symbol_id] == L'-')
					{
						*compare_state = COMPARE_STATE::error;
						MessageBox(NULL, ERROR_MESSAGE(L"Некорректная команда ", out[0]), ERROR_CAP, NULL);
						return vector<wstring>();
					}
				}
				out.back() += source[*symbol_id];
			}
			if ((source[*symbol_id] == L'@') || (source[*symbol_id] <= L'#'))
				if ((out.size() == 2) && (out.back().empty()))
				{
					out.back() += source[*symbol_id];
					out.push_back(L"");
				}
				else
				{
					*compare_state = COMPARE_STATE::error;
					MessageBox(NULL, ERROR_MESSAGE(L"Некорректная команда ", out[0]), ERROR_CAP, NULL);
					return vector<wstring>();
				}
		}
		}
	}
	*compare_state = COMPARE_STATE::error;
	MessageBox(NULL, L"Что - то пошло не так....", ERROR_CAP, NULL);
	return vector<wstring>();
}

unsigned CompileMachSource(vector<RAW_COM> commands, void ** mach_source)
{
	unsigned command_id = 0;
	// contr points
	vector <CONTR_POINTS> contr_points;
	for (auto curr_raw_command = commands.begin(); curr_raw_command < commands.end(); ++curr_raw_command)
	{
		if (curr_raw_command->command_.size() > 3)
		{
			MessageBox(NULL, L"Некорректная команда", ERROR_CAP, NULL);
			return curr_raw_command->line_;
		}
		if (curr_raw_command->command_.size() == 0)
			continue;
		if ((curr_raw_command->command_.size() == 1) || (curr_raw_command->command_.size() == 3))
		{
			command_id++;
			continue;
		}
		if (curr_raw_command->command_.back() == L" ")
		{
			CONTR_POINTS tmp;
			tmp.target_line = command_id;
			tmp.name = curr_raw_command->command_[0];
			bool is_search = false;
			for (auto curr_contr_point = contr_points.begin(); curr_contr_point < contr_points.end(); ++curr_contr_point)
			{
				if (curr_contr_point->name == tmp.name)
				{
					is_search = true;
				}
			}
			if (is_search)
			{
				MessageBox(NULL, ERROR_MESSAGE(L"Конфликт контрольных точек. Данная точка уже существует ", curr_raw_command->command_[0]), ERROR_CAP, NULL);
				return curr_raw_command->line_;
			}
			else
				contr_points.push_back(tmp);
		}
		else
		{
			MessageBox(NULL, L"Некорректная команда", ERROR_CAP, NULL);
			return curr_raw_command->line_;
		}
	}
	// set memory for source
	MACH_SOURCE * out  = new MACH_SOURCE(command_id);
	command_id = 0;
	wstring commands_list;
	for (auto curr_raw_command = commands.begin(); curr_raw_command < commands.end(); ++curr_raw_command)
	{
		if ((curr_raw_command->command_.size() == 0) || (curr_raw_command->command_.size() == 2))
			continue;
		command_id++;
		if (curr_raw_command->command_.size() == 1)
		{
			if (curr_raw_command->command_.back() == L"htl")
			{
				out->commands_[command_id-1] = COMMAND::htl;
				commands_list += curr_raw_command->command_.back();
				commands_list += L"\n";
				continue;
			}
			if (curr_raw_command->command_.back() == L"in")
			{
				out->commands_[command_id-1] = COMMAND::in;
				commands_list += curr_raw_command->command_.back();
				commands_list += L"\n";
				continue;
			}
			MessageBox(NULL, ERROR_MESSAGE(L"Неизвестная команда", curr_raw_command->command_.back()), ERROR_CAP, NULL);
			return curr_raw_command->line_;
		}
		bool is_error = false;
		bool is_reg = false;
		commands_list += curr_raw_command->command_[0];
		commands_list += L"\t";

		if (curr_raw_command->command_[1] != L"cp")
		{
			out->commands_[command_id-1] = StringToNumber(curr_raw_command->command_.back(), &is_error, &is_reg);
			commands_list += curr_raw_command->command_[1];
			commands_list += L"\t";
			commands_list += curr_raw_command->command_[2];
			commands_list += L"\n";
			if (is_error)
			{
				MessageBox(NULL, ERROR_MESSAGE(L"Ошибка при определении числа ", curr_raw_command->command_.back()), ERROR_CAP, NULL);
				return curr_raw_command->line_;
			}
			if (curr_raw_command->command_[1] == L"none")
			{
				if (is_reg)
					out->commands_[command_id - 1] |= ADRESS_TYPE::reg;
			}
			else
			{
				if (curr_raw_command->command_[1] == L"#")
					out->commands_[command_id - 1] |= ADRESS_TYPE::hash;
				if (curr_raw_command->command_[1] == L"@")
					out->commands_[command_id - 1] |= ADRESS_TYPE::sob;
			}
			if (curr_raw_command->command_[0] == L"add")
			{
				out->commands_[command_id-1] |= COMMAND::add;
				continue;
			}
			if (curr_raw_command->command_[0] == L"sub")
			{
				out->commands_[command_id-1] |= COMMAND::sub;
				continue;
			}
			if (curr_raw_command->command_[0] == L"div")
			{
				out->commands_[command_id-1] |= COMMAND::сdiv;
				continue;
			}
			if (curr_raw_command->command_[0] == L"mul")
			{
				out->commands_[command_id-1] |= COMMAND::mul;
				continue;
			}
			if (curr_raw_command->command_[0] == L"rd")
			{
				out->commands_[command_id-1] |= COMMAND::rd;
				continue;
			}
			if (curr_raw_command->command_[0] == L"wr")
			{
				out->commands_[command_id-1] |= COMMAND::wr;
				continue;
			}
			if (curr_raw_command->command_[0] == L"jmp")
			{
				out->commands_[command_id-1] |= COMMAND::jmp;
				continue;
			}
			if (curr_raw_command->command_[0] == L"jmpz")
			{
				out->commands_[command_id-1] |= COMMAND::jmpz;
				continue;
			}
			if (curr_raw_command->command_[0] == L"jmpnz")
			{
				out->commands_[command_id-1] |= COMMAND::jmpnz;
				continue;
			}
			if (curr_raw_command->command_[0] == L"jmplz")
			{
				out->commands_[command_id-1] |= COMMAND::jmplz;
				continue;
			}
			if (curr_raw_command->command_[0] == L"jmpgz")
			{
				out->commands_[command_id-1] |= COMMAND::jmpgz;
				continue;
			}
			MessageBox(NULL, ERROR_MESSAGE(L"Неизвестная команда", curr_raw_command->command_[0]), ERROR_CAP, NULL);
			return curr_raw_command->line_;
		}
		else
		{
			out->commands_[command_id-1] = StringToNumber(curr_raw_command->command_.back(), &is_error, &is_reg);
			if (is_error)
			{
				// sreach contr point
				bool is_search = false;
				for (auto curr_contr_point = contr_points.begin(); curr_contr_point < contr_points.end(); ++curr_contr_point)
				{
					if (curr_contr_point->name == curr_raw_command->command_[2])
					{
						out->commands_[command_id-1] = curr_contr_point->target_line;
						commands_list += L"none";
						commands_list += L"\t";
						commands_list += std::to_wstring(curr_contr_point->target_line);
						commands_list += L"\n";
						is_search = true;
					}
				}
				if (!is_search)
				{
					MessageBox(NULL, ERROR_MESSAGE(L"Невозможно найти контрольную точку ", curr_raw_command->command_[2]), ERROR_CAP, NULL);
					return curr_raw_command->line_;
				}
			}
			else
			{
				out->commands_[command_id-1] |= ADRESS_TYPE::reg;
			}
			if (curr_raw_command->command_[0] == L"jmp")
			{
				out->commands_[command_id-1] |= COMMAND::jmp;
				continue;
			}
			if (curr_raw_command->command_[0] == L"jmpz")
			{
				out->commands_[command_id-1] |= COMMAND::jmpz;
				continue;
			}
			if (curr_raw_command->command_[0] == L"jmpnz")
			{
				out->commands_[command_id-1] |= COMMAND::jmpnz;
				continue;
			}
			if (curr_raw_command->command_[0] == L"jmplz")
			{
				out->commands_[command_id-1] |= COMMAND::jmplz;
				continue;
			}
			if (curr_raw_command->command_[0] == L"jmpgz")
			{
				out->commands_[command_id-1] |= COMMAND::jmpgz;
				continue;
			}
			MessageBox(NULL, ERROR_MESSAGE(L"Неизвестная команда", curr_raw_command->command_[0]), ERROR_CAP, NULL);
			return curr_raw_command->line_;
		}
	}
	commands_list += L'\0';
	out->commands_list = new wchar_t[commands_list.size()];
	memcpy_s(out->commands_list, sizeof(wchar_t) * commands_list.size(), commands_list.c_str(), sizeof(wchar_t) * commands_list.size());
	*mach_source = (void *)out;
	return 0;
}

int StringToNumber(wstring data, bool * is_error, bool * is_reg)
{
	int out = 0;
	bool otr = false;
	if (data[0] == L'-')
		otr = true;
	else
	{
		if (data[0] == L'r')
			*is_reg = true;
		else
		{
			if ((data[0] >= L'0') && (data[0] <= L'9'))
				out = data[0] - L'0';
			else
			{
				*is_error = true;
				return 0;
			}
		}
	}
	for (unsigned i = 1; i < data.size(); i++)
	{
		if ((data[i] >= L'0') && (data[i] <= L'9'))
		{
			out *= 10;
			out += data[i] - L'0';
		}
		else
		{
			*is_error = true;
			return 0;
		}
	}
	if (out >= MAX_NUMBER)
	{
		*is_error = true;
		return 0;
	}
	if (otr)
	{
		out *= -1;
		out &= 0x7fffff;
		out |= SIGN_TYPE::minus;
	}
	if (data[0] == L'r')
		out |= SIGN_TYPE::minus;
	return out;
}

wchar_t * TestCompareComWord(LPWSTR source)
{
	vector <wstring> tmp;
	unsigned symbol_id = 0, line = 0;
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
			for (auto iter = tmp.begin(); iter < tmp.end(); ++iter)
			{
				out += (*iter);
				out += L"\t";
			}
		}
		out += L"\n";
	} while (compare_state == COMPARE_STATE::not_end);
	out += L'\0';
	wchar_t * tmp_buf = new wchar_t[out.size()];
	memcpy_s(tmp_buf, sizeof(wchar_t) * out.size(), out.data(), sizeof(wchar_t) * out.size());
	return tmp_buf;
}

void * Compile(LPWSTR source,int * error_line)
{
	// select memory for out data
	// id symbol in string
	unsigned symbol_id = 0, line = 1;
	bool is_error = false;
	vector<RAW_COM> raw_commands;
	COMPARE_STATE compare_state = COMPARE_STATE::not_end;
	do
	{
		raw_commands.emplace_back(CompareComWord(source,&symbol_id,&line,&compare_state),line);
	} while (compare_state == COMPARE_STATE::not_end);
	if (compare_state == COMPARE_STATE::error)
	{
		*error_line = line;
		return NULL;
	}
	void * mach_source;
	*error_line = CompileMachSource(raw_commands, &mach_source);
	return mach_source;
}

LPWSTR GetCommadList(void * mach_source)
{
	MACH_SOURCE * tmp = ((MACH_SOURCE *)mach_source);
	if (tmp != nullptr)
	{
		if (tmp->commands_list == nullptr)
			return L"";
	}
	else
		return L"";
	return tmp->commands_list;
}

void DeleteMachSource(void * mach_source)
{
	delete [] ((MACH_SOURCE *)mach_source)->commands_;
	delete[]((MACH_SOURCE *)mach_source)->commands_list;
	delete ((MACH_SOURCE *)mach_source);
}

RAW_COM::RAW_COM()
{
	line_ = 0;
}

RAW_COM::RAW_COM(vector<wstring> command, unsigned line)
{
	command_.swap(command);
	line_ = line;
}

