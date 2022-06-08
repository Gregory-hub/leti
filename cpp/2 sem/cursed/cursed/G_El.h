#pragma once
#include "Str.h"


class G_El {
private:
	Str* str;
	G_El* next = nullptr;
public:
	G_El* getNext();
	void setNext(G_El* new_next);
	Str* getStr();
	void setStr(Str* new_str);
	bool equals(G_El* el);
};

