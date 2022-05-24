#pragma once
#include<fstream>
const unsigned N = 10;
using namespace std;
struct strL {
	char stroka[N];
	int length;
	void inpStr(fstream& f, char ogr);
	void outStr(fstream& f);
};
