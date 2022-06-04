#pragma once
#include<fstream>
#include<iostream>
const unsigned N = 5;
using namespace std;
struct StrA {
	char stroka[N] = {'0'};
	int inpStr(fstream& f);
	void outStr(fstream& f);
	void outStrLast(fstream& f, int length);
};
