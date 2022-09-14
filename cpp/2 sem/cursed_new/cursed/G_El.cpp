#include "G_El.h"


G_El* G_El::getNext() {
	return next;
};


void G_El::setNext(G_El* new_next) {
	next = new_next;
};


G_El* G_El::getPrev() {
	return prev;
};


void G_El::setPrev(G_El* new_prev) {
	prev = new_prev;
};


Str* G_El::getStr() {
	return str;
};


void G_El::setStr(Str* new_str) {
	str = new_str;
};


bool G_El::equals(G_El* el) {
	if (this == nullptr && el == nullptr) return true;
	if (this == nullptr ^ el == nullptr) return false;
	return getStr()->equals(el->getStr());
}

