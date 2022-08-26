#pragma once
#include <iostream>
#include <fstream>
#include <string>
#include <sstream>


using namespace std;


const int MAX_STR_LEN = 20;

class Str {
private:
	char letters[MAX_STR_LEN];
	int len = MAX_STR_LEN;
public:
	int getLen();
	void setLen(unsigned int new_len);
	char getLetter(int i);
	void setLetter(int i, char new_letter);
	bool equals(Str* str);
};

