#pragma once
#include"BL1.h"
struct FormB {
	BL1* head, *cur, *last, *prev;
	void setLast();
};
void InitFormB(FormB* p, FormS a);
void InputB(fstream& f, FormB* p);
void OutputB(fstream& f, FormB p);
void DeleteB(FormB* p);
int lengthText(FormB t);
BL1* puchBack(FormB p, BL1 a);
int maxlengthStr(FormB T);
void putProbel(FormB& T);
void deleteProbelk(FormB& T);
