#include"AllList.h"
void inputS(fstream& f, FormS* p) {
	StrA a;
	int g;
	g = a.inpStr(f);
	initS(p, a);
	if (g == N) {
		g = a.inpStr(f);
		p->cur = new StrL;
		p->head->next = p->cur;
		p->cur->Info = a;
		if (g == N) {
			while (1) {
				g = a.inpStr(f);
				if (g > N) {
					break;
				}
				p->cur->next = new StrL;
				p->cur = p->cur->next;
				p->cur->Info = a;
			}
			p->cur->next = new StrL;
			p->cur = p->cur->next;
			p->cur->Info = a;
			p->cur->length = g - N;
			p->cur->next = NULL;
			p->head->length = p->cur->length;
		}
		else {
			p->cur->next = NULL;
			p->head->length = g - N;
		}
	}
	else {
		p->head->length = g - N;
	}
	p->setLast();
}
void InputB(fstream& f, FormB* p) {
	FormS a;
	inputS(f, &a);
	InitFormB(p, a);
	if (!f.eof()) {
		inputS(f, &a);
		p->cur = new BL1;
		p->head->next = p->cur;
		p->cur->S = a;
		if (!f.eof()) {
			while (1) {
				inputS(f, &a);
				if (f.eof()) {
					break;
				}
				p->cur->next = new BL1;
				p->cur = p->cur->next;
				p->cur->S = a;
			}
			p->cur->next = new BL1;
			p->cur = p->cur->next;
			p->cur->S = a;
		}
	}
	p->cur->next = NULL;
	p->setLast();
}
void Input(fstream& f, fstream& f1, fstream& f2, AllList* p) {
	InputB(f, &(p->T1));
	InputB(f1, &(p->T2));
	InputB(f2, &(p->T3));
}
		