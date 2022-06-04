#pragma once
#include "G_El.h"


class FormG {
private:
	G_El* head = nullptr;
	G_El* curr = nullptr;
	G_El* prev = nullptr;
public:
	G_El* getHead();
	void setHead(G_El* new_head);
	G_El* getCurr();
	void setCurr(G_El* new_curr);
	G_El* getPrev();
	void setPrev(G_El* new_prev);
};

