#include"AllList.h"
void initS(FormS* p, StrA a) {
	p->head = new StrL;
	p->head->Info = a;
	p->head->next = NULL;
	p->cur = NULL;
	p->prev;
}
void InitFormB(FormB* p, FormS a) {
	p->head = new BL1;
	p->head->S = a;
	p->head->next = NULL;
	p->cur = NULL;
	p->prev = NULL;
	p->setLast();
}