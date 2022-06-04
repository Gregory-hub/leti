#pragma once
#include <iostream>
#include <fstream>
#include <string>


using namespace std;


const int MAX_SIZE = 20;

class Str {
private:
	char letters[MAX_SIZE];
	int len = MAX_SIZE;
public:
	int getLen();
	void setLen(unsigned int new_len);
	char getLetter(int i);
	void setLetter(int i, char new_letter);
	bool equals(Str* str);
};

