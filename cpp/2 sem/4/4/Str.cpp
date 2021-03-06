#include "Str.h"


int Str::getLen() {
	return len;
};


void Str::setLen(unsigned int new_len) {
	if (new_len > MAX_SIZE || new_len < 0) {
		throw "Invalid len";
	}
	len = new_len;
};


char Str::getLetter(int i) {
	return letters[i];
};


void Str::setLetter(int i, char new_letter) {
	if (i >= MAX_SIZE || i < 0) {
		throw "Invalid index";
	}
	letters[i] = new_letter;
};


bool Str::equals(Str* str) {
	if (getLen() != str->getLen()) return false;
	for (int i = 0; i < getLen(); i++) {
		if (getLetter(i) != str->getLetter(i)) return false;
	}
	return true;
}

