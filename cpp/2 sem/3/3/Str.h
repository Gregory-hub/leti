#pragma once
#include <iostream>
#include <fstream>
#include <string>

using namespace std;

struct Str {
	char* letters = new char;
	int len = 0;
	void del();
};

