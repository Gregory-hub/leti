#include"strL.h"
void strL::outStr(fstream& f) {
	if (length == 0)
		f << "������ ������";
	else {
		for (int i = 0; i < length; i++) {
			f << stroka[i];
		}
		f << " ->";
	}
	f << '\n';
}