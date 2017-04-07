#include <fstream>
#include<iostream>
using namespace std;

int main()
{
	ofstream fout; 
	fout.open("infa.txt", ios_base::trunc);
	fout << "Examlesebgdfgh";
	fout.close();
	system("pause");
	return 0;
}
