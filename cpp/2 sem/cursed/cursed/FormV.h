#pragma once
#include "V_El.h"


class FormV {
private:
	V_El* head = nullptr;
	V_El* curr = nullptr;
	V_El* prev = nullptr;
public:
	V_El* getHead();
	void setHead(V_El* new_head);
	V_El* getCurr();
	void setCurr(V_El* new_curr);
	V_El* getPrev();
	void setPrev(V_El* new_prev);
};

