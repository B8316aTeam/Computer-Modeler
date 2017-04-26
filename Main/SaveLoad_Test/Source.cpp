#include <fstream>
#include<iostream>
using namespace std;

int main()
{
	fstream fout; 
	fout.open("infa.txt",ios::out);
	if (fout.is_open())
	{
		cout << "ok\n";
	}
	else
	{
		cout << "no\n";
	}
	fout << "Extteesaaaaaaaaaaaadfe";
	fout.close();
	system("pause");
	return 0;
}
