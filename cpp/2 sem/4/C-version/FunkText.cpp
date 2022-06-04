#include"AllList.h"
int lengthStr(FormS s) {
	s.cur = s.head;
	int i = 0;
	while (s.cur != s.last) {
		i++;
		s.cur = s.cur->next;
	}
	return i * N + s.head->length;
}
int lengthText(FormB t) {
	int i = 0;
	t.cur = t.head;
	while (t.cur != t.last) {
		i++;
		t.cur = t.cur->next;
	}
	return i + 1;
}
int colBloks(FormS s) {
	int l = 0;
	s.cur = s.head;
	while (s.cur->next != NULL) {
		l++;
		s.cur = s.cur->next;
	}
	return l;
}
void BL1:: init() {
	S.head = new StrL;
	S.cur = S.head;
	S.prev = NULL;
	S.last = S.head;
	S.head->length = 0;
	S.head->next = NULL;
}
int maxlengthStr(FormB T) {
	int l;
	l = lengthStr(T.head->S);
	T.cur = T.head->next;
	for (int i = 1; i < lengthText(T); i++) {
		if (lengthStr(T.cur->S) > l)
			l = lengthStr(T.cur->S);
		T.cur = T.cur->next;
	}
	return l;
}
FormB cut(FormB T, int n, int k) {
	FormB c;
	StrA a;
	FormS* b = new FormS;
	initS(b, a);
	InitFormB(&c, *b);
	int j = 0, q = 0;
	int h = n / N;
	T.cur = T.head;
	T.cur->S.cur = T.cur->S.head;
	for (int i = 0; i < h; i++) {
		T.cur->S.cur = T.cur->S.cur->next;
	}
	c.cur = c.head;
	for (int i = 0; i < lengthText(T); i++) {
		c.cur->S.cur = c.cur->S.head;
		if (i != 0) {
		T.cur->S.cur = T.cur->S.head;
			for (int l = 0; l < h; l++) {
				T.cur->S.cur = T.cur->S.cur->next;
			}
		}
		q = 0;
		j = n - h * N;
		for (int l = n; l < k; l++) {
			c.cur->S.cur->Info.stroka[q] = T.cur->S.cur->Info.stroka[j];
			j++;
			if (j == N) {
				T.cur->S.cur = T.cur->S.cur->next;
				j = 0;
			}
			q++;
			if (q == N && l != k - 1) {
				c.cur->S.cur->next = new StrL;
				c.cur->S.cur = c.cur->S.cur->next;
				q = 0;
			}
		}
		c.cur->S.cur->next = NULL;
		c.cur->S.head->length = q;
		T.cur = T.cur->next;
		if (i != lengthText(T) - 1) {
			if (lengthStr(T.cur->S) != n) {
				c.cur->S.setLast();
				c.cur->next = new BL1;
				c.cur = c.cur->next;
				initS(&(c.cur->S), a);
			}
			else {
				c.cur->S.setLast();
				break;
			}
		}
		else {
			c.cur->S.setLast();
			break;
		}
	}
	c.cur->next = NULL;
	c.setLast();
	return c;
}
FormB cutLast(FormB T, int n) {
	FormB c;
	StrA a;
	FormS* b = new FormS;
	initS(b, a);
	InitFormB(&c, *b);
	int j = 0, q = 0;
	int h = n / N;
	T.cur = T.head;
	T.cur->S.cur = T.cur->S.head;
	for (int i = 0; i < h; i++) {
		T.cur->S.cur = T.cur->S.cur->next;
	}
	c.cur = c.head;
	for (int i = 0; i < lengthText(T); i++) {
		c.cur->S.cur = c.cur->S.head;
		if (i != 0) {
			T.cur->S.cur = T.cur->S.head;
			for (int l = 0; l < h; l++) {
				T.cur->S.cur = T.cur->S.cur->next;
			}
		}
		q = 0;
		j = n - h * N;
		for (int l = n; l < lengthStr(T.cur->S); l++) {
			c.cur->S.cur->Info.stroka[q] = T.cur->S.cur->Info.stroka[j];
			j++;
			if (j == N) {
				T.cur->S.cur = T.cur->S.cur->next;
				j = 0;
			}
			q++;
			if (q == N && l != lengthStr(T.cur->S) - 1) {
				c.cur->S.cur->next = new StrL;
				c.cur->S.cur = c.cur->S.cur->next;
				q = 0;
			}
		}
		c.cur->S.cur->next = NULL;
		c.cur->S.head->length = q;
		T.cur = T.cur->next;
		if (i != lengthText(T) - 1) {
			if (lengthStr(T.cur->S) > n) {
				c.cur->S.setLast();
				c.cur->next = new BL1;
				c.cur = c.cur->next;
				initS(&(c.cur->S), a);
			}
			else {
				c.cur->S.setLast();
				break;
			}
		}
		else {
			c.cur->S.setLast();
			break;
		}
	}
	c.cur->next = NULL;
	c.setLast();
	return c;
}
void putProbel(FormB& T) {
	int m = maxlengthStr(T);
	T.cur = T.head;
	int j = 0, h = 0;
	for (int i = 0; i < lengthText(T); i++) {
		T.cur->S.cur = T.cur->S.head;
		T.cur->S.cur = T.cur->S.last;
		j = T.cur->S.head->length;
		if (j == 5) {
			T.cur->S.cur->next = new StrL;
			T.cur->S.cur = T.cur->S.cur->next;
			T.cur->S.setLast();
			j = 0;
			T.cur->S.head->length = 0;
		}
		h = colBloks(T.cur->S);
		while (j + h * N < m) {
			if (j < N) {
				T.cur->S.cur->Info.stroka[j] = ' ';
			}
			j++;
			if (j == N) {
				if (T.cur->S.cur->next == NULL) {
					T.cur->S.cur->next = new StrL;
					T.cur->S.cur = T.cur->S.cur->next;
					T.cur->S.head->length = 0;
					h++;
					j = 0;
					continue;
				}
				else {
					T.cur->S.cur = T.cur->S.cur->next;
					j = 0;
					T.cur->S.head->length = 0;
				}
			}
			T.cur->S.head->length++;
		}
		T.cur->S.setLast();
		T.cur = T.cur->next;
	}
}
void deleteProbelk(FormB& T) {
	T.cur = T.head;
	int j = 0, q = 0, k = 0, h = 0;
	for (int i = 0; i < lengthText(T); i++) {
		j = 0;
		k = 0;
		T.cur->S.cur = T.cur->S.head;
		while (j < lengthStr(T.cur->S)) {
			if (T.cur->S.cur->Info.stroka[k] != ' ') {
				j++;
				k++;
				if (k == N) {
					T.cur->S.cur = T.cur->S.cur->next;
					k = 0;
				}
			}
			else {
				k++;
				if (k == N) {
					T.cur->S.cur = T.cur->S.cur->next;
					k = 0;
				}
				q = 1;
				while (T.cur->S.cur->Info.stroka[k] == ' ') {
					q++;
					k++;
					if (k == N) {
						T.cur->S.cur = T.cur->S.cur->next;
						k = 0;
					}
					if (j + q == lengthStr(T.cur->S))
						break;
				}
				if (j + q < lengthStr(T.cur->S)) {
					j += q;
				}
				else
					break;
			}
		}
		if (j != lengthStr(T.cur->S)) {
			h = j / N;
			T.cur->S.cur = T.cur->S.head;
			for (int l = 0; l < h; l++)
				T.cur->S.cur = T.cur->S.cur->next;
			T.cur->S.cur->next = NULL;
			T.cur->S.setLast();
			T.cur->S.head->length = j - N * h;
		}
		T.cur = T.cur->next;
	}
}