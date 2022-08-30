#include "FormG.h"


G_El* FormG::getHead() {
	return head;
};


void FormG::setHead(G_El* new_head) {
	head = new_head;
};


G_El* FormG::getCurr() {
	return curr;
};


void FormG::setCurr(G_El* new_curr) {
	curr = new_curr;
};


G_El* FormG::getPrev() {
	return prev;
};


void FormG::setPrev(G_El* new_prev) {
	prev = new_prev;
};


void FormG::reset() {
	curr = head;
	prev = nullptr;
}
