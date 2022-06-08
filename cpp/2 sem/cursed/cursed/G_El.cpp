#include "G_El.h"


G_El* G_El::getNext() {
	return next;
};


void G_El::setNext(G_El* new_next) {
	next = new_next;
};


Str* G_El::getStr() {
	return str;
};


void G_El::setStr(Str* new_str) {
	str = new_str;
};


bool G_El::equals(G_El* el) {
	return getStr()->equals(el->getStr());
}

