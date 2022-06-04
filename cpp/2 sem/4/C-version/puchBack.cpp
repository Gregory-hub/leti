#include"AllList.h"
StrL* puchBack(FormS p, StrL a) {
	a.next = NULL;
	p.cur = p.head;
	while (p.cur->next != NULL) {
		p.cur = p.cur->next;
	}
	p.cur->next = new StrL;
	*(p.cur->next) = a;
	return p.head;
}
BL1* puchBack(FormB p, BL1 a) {
	a.next = NULL;
	p.cur = p.head;
	while (p.cur->next != NULL) {
		p.cur = p.cur->next;
	}
	p.cur->next = new BL1;
	*(p.cur->next) = a;
	return p.head;
}
int puchBack(FormB& T, FormB c) {
	if (lengthText(c) > lengthText(T))
		return -1;
	T.cur = T.head;
	c.cur = c.head;
	int j = 0, q = 0;
	for (int i = 0; i < lengthText(c); i++) {
		T.cur->S.cur = T.cur->S.head;
		T.cur->S.cur = T.cur->S.last;
		c.cur->S.cur = c.cur->S.head;
		j = lengthStr(T.cur->S) - colBloks(T.cur->S) * N;
		if (j == N) {
			T.cur->S.cur->next = new StrL;
			T.cur->S.cur = T.cur->S.cur->next;
			T.cur->S.last = T.cur->S.cur;
			j = 0;
			T.cur->S.head->length = 0;
		}
		q = 0;
		for (int l = 0; l < lengthStr(c.cur->S); l++) {
			T.cur->S.cur->Info.stroka[j] = c.cur->S.cur->Info.stroka[q];
			j++;
			if (j == N && l != lengthStr(c.cur->S) - 1) {
				T.cur->S.cur->next = new StrL;
				T.cur->S.cur = T.cur->S.cur->next;
				j = 0;
				T.cur->S.head->length = 0;
			}
			q++;
			if (q == N) {
				c.cur->S.cur = c.cur->S.cur->next;
				q = 0;
			}
			if (j != N) {
				T.cur->S.head->length++;
			}
		}
		T.cur->S.cur->next = NULL;
		T.cur->S.setLast();
		T.cur = T.cur->next;
		c.cur = c.cur->next;
	}
}