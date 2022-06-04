#pragma once
#include "FormV.h"

class All {
public:
	FormV* list1 = nullptr;
	FormV* list2 = nullptr;
	FormV* list3 = nullptr;

	void read(string filename1, string filename2, string filename3);
};

FormV* read_one(string filename);
FormG* create_formG(char* line);
G_El* create_G_El(char* line, unsigned int i);
Str* create_str(char* line, unsigned int i);

