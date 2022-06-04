#pragma once
#include"StrA.h"
struct StrL {
	StrA Info;
	StrL* next = NULL;
	int length = 0;
};