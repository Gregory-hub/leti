#pragma once
#include "Str.h"

struct Elem {
	Str str;
	Elem* next;
	void del();
};

