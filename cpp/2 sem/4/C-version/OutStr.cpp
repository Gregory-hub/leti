#include"strA.h"
void StrA::outStr(fstream& f) {
	for (int i = 0; i < N; i++) {
		f << stroka[i];
	}
	f << " ->";
}
void StrA:: outStrLast(fstream& f, int length) {
	for (int i = 0; i < length; i++) {
		f << stroka[i];
	}
	f << " ->";
}