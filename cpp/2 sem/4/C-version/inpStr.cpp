#include"StrA.h"
int StrA::inpStr(fstream& f) {
	char c = '0';
	for (int i = 0; i < N; i++) {
		f >> stroka[i];
		f.unsetf(ios::skipws);
		f >> c;
		if (c == '\n' || f.eof() == 1) {
			return N + i + 1;
			break;
		}
		f.seekg(-1, ios::cur);
		f.setf(ios::skipws);
	}
	return N;
}