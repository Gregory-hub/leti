#include"AllList.h"
StrL* setPrev(FormS p) {
	p.prev = p.head;
	while (p.prev->next != p.cur) {
		p.prev = p.prev->next;
	}
	return p.prev;
}
StrL* setPrev1(FormS p, StrL* a) {
	p.prev = p.head;
	while (p.prev->next != a) {
		p.prev = p.prev->next;
	}
	return p.prev;
}