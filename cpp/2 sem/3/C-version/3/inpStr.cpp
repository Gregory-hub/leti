#include"strL.h"
void strL::inpStr(fstream& f, char ogr) {
	char c;
	unsigned i = 0;
	while (f.eof() == 0) {
		f >> c;
		if (c == ogr) {
			f >> c;
			while (c != ogr && c != '\n' && f.eof() == 0) {
				stroka[i] = c;
				f.unsetf(ios::skipws);
				f >> c;
				i++;
				if (i > N)
					break;
				f.setf(ios::skipws);
			}
			length = i;
			break;
		}
		else
			continue;
	}
}