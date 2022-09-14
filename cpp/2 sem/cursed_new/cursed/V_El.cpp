#include "V_El.h"


FormG* V_El::getForm() {
	return form;
};


void V_El::setForm(FormG* new_form) {
	form = new_form;
};


V_El* V_El::getNext() {
	return next;
};


void V_El::setNext(V_El* new_next) {
	next = new_next;
};

