#pragma once
#include"StrL.h"
struct FormS {
	StrL* head, *cur, *last, *prev;
	void setLast();
};
void initS(FormS* p, StrA a);
void inputS(fstream &f, FormS* p);
void outputS(fstream& f, FormS p);
void deleteS(FormS* p);
StrL* setPrev(FormS p);
StrL* setPrev1(FormS p, StrL* a);
StrL* puchBack(FormS p, StrL a);
int lengthStr(FormS s);
int colBloks(FormS s);