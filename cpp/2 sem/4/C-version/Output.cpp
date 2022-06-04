#include"AllList.h"
void outputS(fstream& f, FormS p) {
	if (!is_Empty(p.head)) {
		if (p.head != p.last) {
			p.head->Info.outStr(f);
			p.cur = p.head->next;
			if (p.cur != p.last) {
				p.cur->Info.outStr(f);
				p.cur = p.cur->next;
				while (p.cur->next != NULL) {
					p.cur->Info.outStr(f);
					p.cur = p.cur->next;
				}
				p.cur->Info.outStrLast(f, p.head->length);
			}
			else
				p.cur->Info.outStrLast(f, p.head->length);
		}
		else
			p.head->Info.outStrLast(f, p.head->length);
		f << "NULL" << '\n';
	}
	else
		f << "пустая строка" << '\n';
}
void OutputB(fstream& f, FormB p) {
	if (!is_Empty(p.head)) {
		outputS(f, p.head->S);
		f << " |" << '\n' << "\\ /" << '\n';
		p.cur = p.head->next;
		outputS(f, p.cur->S);
		f << " |" << '\n' << "\\ /" << '\n';
		p.cur = p.cur->next;
		while (p.cur != NULL) {
			outputS(f, p.cur->S);
			f << " |" << '\n' << "\\ /" << '\n';
			p.cur = p.cur->next;
		}
		f << "NULL" << '\n';
	}
	else
		f << "пустой список" << '\n';
}
void Output(fstream& f, AllList p) {
	f << "список L:" << '\n';
	OutputB(f, p.T1);
	f << "список L1:" << '\n';
	OutputB(f, p.T2);
	f << "список L2:" << '\n';
	OutputB(f, p.T3);
}