#include "FormV.h"


V_El* FormV::getHead() {
	return head;
};


void FormV::setHead(V_El* new_head) {
	head = new_head;
};


V_El* FormV::getCurr() {
	return curr;
};


void FormV::setCurr(V_El* new_curr) {
	curr = new_curr;
};


V_El* FormV::getPrev() {
	return prev;
};


void FormV::setPrev(V_El* new_prev) {
	prev = new_prev;
};


void FormV::reset() {
	curr = head;
	while (curr != nullptr) {
		curr->getForm()->setCurr(curr->getForm()->getHead());

		curr = curr->getNext();
	}
	curr = head;
}

