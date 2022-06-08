#pragma once
#include "FormG.h"


class V_El{
private:
	FormG* form = nullptr;
	V_El* next = nullptr;
public:
	FormG* getForm();
	void setForm(FormG* new_form);
	V_El* getNext();
	void setNext(V_El* new_next);
};

