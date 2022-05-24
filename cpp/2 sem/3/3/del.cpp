#include "Elem.h"
#include "Str.h"
#include "functions.h"


void Str::del() {
	delete[] letters;
}


void Elem::del() {
	if (next != NULL) {
		next->del();
	}
	delete next;
	next = NULL;
}

void desintegrate(Elem*& first) {
	if (first != NULL) {
		first->del();
		delete first;
		first = NULL;
	}
}

