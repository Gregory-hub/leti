#include"AllList.h"
bool ravno(StrL a, StrL b) {
	bool h = true;
	for (int i = 0; i < N; i++) {
		if (a.Info.stroka[i] != b.Info.stroka[i]) {
			h = false;
			break;
		}
	}
	return h;
}
int searchStr(FormB T, FormS s1, int& ch1, int& st) {
	StrL cur;
	bool h = false;
	int j = 0, l = 0, ch = 0, k, q, w;
	q = st;
	w = ch1;
	s1.cur = s1.head;
	T.cur = T.head;
	for (int i = 0; i < st; i++) {
		T.cur = T.cur->next;
	}
	l = ch1 / N;
	T.cur->S.cur = T.cur->S.head;
	for (int i = 0; i < l; i++) {
		T.cur->S.cur = T.cur->S.cur->next;
	}
	j = ch1 - N * l;
	for (int i = st; i < lengthText(T); i++) {
		if (i == st) {
			cur = *(T.cur->S.cur);
		}
		else {
			cur = *(T.cur->S.head);
		}
		while (cur.next != NULL) {
			if (i != st) {
				j = 0;
				l = 0;
			}
			while(j < N) {
				q = 0;
				if (cur.Info.stroka[j] == s1.cur->Info.stroka[0]) {
					h = true;
					ch = l * N + j;
					ch1 = ch;
					st = i;
					k = 1;
					for (int p = 1; p < lengthStr(s1); p++) {
						j++;
						if (j == N) {
							cur = *(cur.next);
							l++;
							j = 0;
						}
						if (k == N) {
							s1.cur = s1.cur->next;
							k = 0;
						}
						q++;
						if (cur.Info.stroka[j] != s1.cur->Info.stroka[k]) {
							h = false;
							break;
						}
						else {
							if (cur.next == NULL && j == T.cur->S.head->length - 1 && p != lengthStr(s1) - 1) {
								h = false;
								break;
							}
							if (j == N - 1) {
								cur = *(cur.next);
								l++;
								j = -1;
							}
							if (k == N - 1) {
								s1.cur = s1.cur->next;
								k = 0;
							}
						}
						k++;
					}
					if (h) {
						return ch;
					}
				}
				if (!h && q > 0)
					j -= (q - 1);
				else
					j++;
				if (j == T.cur->S.head->length && cur.next == NULL) {
					break;
				}
			}
			l++;
			if (l <= colBloks(T.cur->S)) {
				cur = *(cur.next);
				j = 0;
			}
		}
		if (!h) {
			if (T.cur->S.cur->length >= lengthStr(s1)) {
				j = 0;
				while(j < T.cur->S.head->length) {
					if (cur.Info.stroka[j] == s1.cur->Info.stroka[0]) {
						h = true;
						ch = l * N + j;
						ch1 = ch;
						st = i;
						for (int k = 1; k < lengthStr(s1); k++) {
							j++;
							if (cur.Info.stroka[j] != s1.cur->Info.stroka[k]) {
								h = false;
								break;
							}
						}
						if (h) {
							return ch;
						}
						else {
							j = ch - N * l + 1;
							continue;
						}
					}
					j++;
				}
			}
		}
		T.cur = T.cur->next;
	}
	st = q;
	ch1 = w;
	return -1;
}
int search(AllList T, int& ch1, int& fst, int& lst) {
	int q = fst;
	int w = ch1;
	StrL cur;
	if (lengthText(T.T1) - fst < lengthText(T.T2)) {
		fst = q;
		ch1 = w;
		lst = 0;
		return -1;
	}
	bool g = false;
	T.T1.cur = T.T1.head;
	T.T2.cur = T.T2.head;
	int ch = searchStr(T.T1, T.T2.cur->S,ch1, fst);
	if (ch < 0) {
		fst = q;
		ch1 = w;
		lst = 0;
		return -1;
	}
	lst = fst;
	int l = ch / N;
	int j = ch - l * N, h = 0;
	T.T2.cur = T.T2.cur->next;
	for (int k = 0; k <= fst; k++) {
		T.T1.cur = T.T1.cur->next;
	}
	for (int i = 1; i < lengthText(T.T2); i++) {
		cur = *(T.T1.cur->S.head);
		for (int k = 0; k < l; k++) {
			cur = *(cur.next);
		}
		h = 0;
		g = false;
		while (h < lengthStr(T.T2.cur->S)) {
			if (cur.Info.stroka[j] == T.T2.cur->S.head->Info.stroka[h]) {
				g = true;
			}
			else {
				g = false;
				break;
			}
			h++;
			if (h == N) {
				if (T.T2.cur->S.head->next != NULL) {
					T.T2.cur->S.head = T.T2.cur->S.head->next;
					h = 0;
				}
			}
			j++;
			if (j == N) {
				if (T.T1.cur->S.head->next != NULL) {
					cur = *(cur.next);
					j = 0;
				}
				else {
					j = 0;
					while (h < lengthStr(T.T2.cur->S)) {
						if (cur.Info.stroka[j] == T.T2.cur->S.head->Info.stroka[h]) {
							g = true;
						}
						j++;
					}
				}
			}
		}
		if (!g) {
			lst = 0;
			return -1;
		}
		else {
			T.T2.cur = T.T2.cur->next;
			T.T1.cur = T.T1.cur->next;
			lst++;
			j = ch - l * N;
		}
	}
	ch1 = ch + lengthStr(T.T2.head->S);
	return ch;
}
void cut(FormB Text, FormB& part, int n0, int n) {

}

