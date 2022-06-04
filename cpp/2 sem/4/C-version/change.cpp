#include"AllList.h"
int change(AllList& T, int& ch1, int& fst, int& lst) {
	int ch, l = 0, j, h, t = 0;
	int r1, r2, r3, q;
	StrL cur, pcur;
	ch = search(T, ch1, fst, lst);
	if (ch >= 0) {
		h = ch / N;
		j = ch - N * h;
		T.T1.cur = T.T1.head;
		for (int i = 0; i < fst; i++) {
			T.T1.cur = T.T1.cur->next;
		}
		if (lengthText(T.T2) == lengthText(T.T3)) {
			l = lengthText(T.T2);
			T.T2.cur = T.T2.head;
			T.T3.cur = T.T3.head;
			for (int i = 0; i < l; i++) {
				T.T1.cur->S.cur = T.T1.cur->S.head;
				int k1 = ch / N;
				for (int l = 0; l < k1; l++) {
					T.T1.cur->S.cur = T.T1.cur->S.cur->next;
				}
				j = ch - N * k1;
				r1 = lengthStr(T.T2.cur->S) - lengthStr(T.T3.cur->S);
				if (r1 >= 0) {
					T.T2.cur->S.cur = T.T2.cur->S.head;
					T.T3.cur->S.cur = T.T3.cur->S.head;
					q = 0;
					for (int k = 0; k < lengthStr(T.T3.cur->S); k++) {
						T.T1.cur->S.cur->Info.stroka[j] = T.T3.cur->S.cur->Info.stroka[q];
						j++;
						if (j == N) {
							T.T1.cur->S.cur = T.T1.cur->S.cur->next;
							j = 0;
						}
						q++;
						if (q == N) {
							T.T3.cur->S.cur = T.T3.cur->S.cur->next;
							q = 0;
						}
					}
					T.T1.cur->S.cur = T.T1.cur->S.head;
					j = ch - N * h + lengthStr(T.T3.cur->S);
					if (j >= N) {
						int k = 1;
						while (1) {
							j = j - N;
							if (j < N) {
								h = k;
								break;
							}
							k++;
						}
						for (int k = 0; k < h; k++) {
							T.T1.cur->S.cur = T.T1.cur->S.cur->next;
						}
					}
					cur = *(T.T1.cur->S.cur);
					cur.length = T.T1.cur->S.head->length;
					q = j + r1;
					if (q >= N) {
						int k = 1;
						while (1) {
							q = q - N;
							if (q < N) {
								h = k;
								break;
							}
							k++;
						}
						for (int k = 0; k < h; k++) {
							cur = *(cur.next);
						}
					}
					while (cur.next != NULL || q < cur.length) {
						if (j < N && q < N) {
							T.T1.cur->S.cur->Info.stroka[j] = cur.Info.stroka[q];
						}
						j++;
						if (j == N) {
							T.T1.cur->S.cur = T.T1.cur->S.cur->next;
							j = 0;
						}
						q++;
						if (q >= N) {
							int k = 1;
							while (1) {
								q = q - N;
								if (q < N) {
									h = k;
									break;
								}
							}
							for (int k = 0; k < h; k++) {
								cur = *(cur.next);
							}
						}
					}
					if (T.T1.cur->S.head->length - r1 > 0)
						T.T1.cur->S.head->length -= r1;
					else if (T.T1.cur->S.head->length - r1 == 0) {
						T.T1.cur->S.cur = T.T1.cur->S.last;
						T.T1.cur->S.prev = setPrev(T.T1.cur->S);
						T.T1.cur->S.last = T.T1.cur->S.prev;
						T.T1.cur->S.last->next = NULL;
						T.T1.cur->S.head->length = 5;
					}
					else {
						int e = T.T1.cur->S.head->length - r1;
						T.T1.cur->S.cur->next = NULL;
						T.T1.cur->S.last = T.T1.cur->S.cur;
						T.T1.cur->S.head->length = 5 + e;
					}
				}
				else {
					T.T2.cur->S.cur = T.T2.cur->S.head;
					T.T3.cur->S.cur = T.T3.cur->S.head;
					j = lengthStr(T.T1.cur->S) - 1;
					int k = 0;
					while (1) {
						j = j - N;
						if (j < N) {
							h = k;
							break;
						}
						k++;
					}
					T.T1.cur->S.cur = T.T1.cur->S.last;
					int e = T.T1.cur->S.head->length - r1;
					if (e >= 5) {
						k = e / 5;
						for (int u = 0; u < k; u++) {
							StrL a;
							T.T1.cur->S.head = puchBack(T.T1.cur->S, a);
							T.T1.cur->S.setLast();
							T.T1.cur->S.head->length = 0;
							T.T1.cur->S.prev = new StrL;
							T.T1.cur->S.prev = setPrev(T.T1.cur->S);
							T.T1.cur->S.cur = setPrev1(T.T1.cur->S, T.T1.cur->S.last);
						}
						t = colBloks(T.T1.cur->S) - 1;
					}
					cur = *(T.T1.cur->S.cur);
					pcur = *(T.T1.cur->S.head);
					while (!ravno(*(pcur.next), cur)) {
						pcur = *(pcur.next);
					}
					cur.length = T.T1.cur->S.head->length;
					q = j - r1;
					if (q >= N) {
						int k = 1;
						while (1) {
							q = q - N;
							if (q < N) {
								h = k;
								break;
							}
						}
						for (int k = 0; k < h; k++) {
							T.T1.cur->S.cur = T.T1.cur->S.cur->next;
						}
					}
					while (j + t * N >= ch + lengthStr(T.T2.cur->S)) {
						T.T1.cur->S.cur->Info.stroka[q] = cur.Info.stroka[j];
						j--;
						if (j < 0) {
							cur = pcur;
							pcur = *(T.T1.cur->S.head);
							while (!ravno(*(pcur.next), cur)) {
								pcur = *(pcur.next);
							}
							j += N;
							t--;
						}
						q--;
						if (q < 0) {
							T.T1.cur->S.cur = T.T1.cur->S.prev;
							T.T1.cur->S.prev = setPrev(T.T1.cur->S);
							q += N;
						}
					}
					h = ch / N;
					j = ch - N * h;
					T.T1.cur->S.cur = T.T1.cur->S.head;
					T.T3.cur->S.cur = T.T3.cur->S.head;
					for (int k = 0; k < h; k++) {
						T.T1.cur->S.cur = T.T1.cur->S.cur->next;
					}
					q = 0;
					for (int k = 0; k < lengthStr(T.T3.cur->S); k++) {
						T.T1.cur->S.cur->Info.stroka[j] = T.T3.cur->S.cur->Info.stroka[q];
						j++;
						if (j == N) {
							T.T1.cur->S.cur = T.T1.cur->S.cur->next;
							j = 0;
						}
						q++;
						if (q == N) {
							T.T3.cur->S.cur = T.T3.cur->S.cur->next;
							q = 0;
						}
					}
					if (e <= 5) {
						T.T1.cur->S.head->length = T.T1.cur->S.head->length - r1;
					}
					else {
						while (e > 5) {
							e -= N;
						}
						T.T1.cur->S.head->length = e;
					}
				}
				T.T1.cur = T.T1.cur->next;
				T.T3.cur = T.T3.cur->next;
				T.T2.cur = T.T2.cur->next;
				h = ch / N;
				j = ch - N * h;
				q = 0;
			}
		}
		else {
			r2 = lengthText(T.T2) - lengthText(T.T3);
			if (r2 > 0) {
				l = lengthText(T.T2) - r2;
				T.T2.cur = T.T2.head;
				T.T3.cur = T.T3.head;
				for (int i = 0; i < l; i++) {
					T.T1.cur->S.cur = T.T1.cur->S.head;
					int k1 = ch / N;
					for (int l = 0; l < k1; l++) {
						T.T1.cur->S.cur = T.T1.cur->S.cur->next;
					}
					j = ch - N * k1;
					r1 = lengthStr(T.T2.cur->S) - lengthStr(T.T3.cur->S);
					if (r1 >= 0) {
						T.T2.cur->S.cur = T.T2.cur->S.head;
						T.T3.cur->S.cur = T.T3.cur->S.head;
						q = 0;
						for (int k = 0; k < lengthStr(T.T3.cur->S); k++) {
							T.T1.cur->S.cur->Info.stroka[j] = T.T3.cur->S.cur->Info.stroka[q];
							j++;
							if (j == N) {
								T.T1.cur->S.cur = T.T1.cur->S.cur->next;
								j = 0;
							}
							q++;
							if (q == N) {
								T.T3.cur->S.cur = T.T3.cur->S.cur->next;
								q = 0;
							}
						}
						T.T1.cur->S.cur = T.T1.cur->S.head;
						j = ch - N * h + lengthStr(T.T3.cur->S);
						if (j >= N) {
							int k = 1;
							while (1) {
								j = j - N;
								if (j < N) {
									h = k;
									break;
								}
								k++;
							}
							for (int k = 0; k < h; k++) {
								T.T1.cur->S.cur = T.T1.cur->S.cur->next;
							}
						}
						cur = *(T.T1.cur->S.cur);
						cur.length = T.T1.cur->S.head->length;
						q = j + r1;
						if (q >= N) {
							int k = 1;
							while (1) {
								q = q - N;
								if (q < N) {
									h = k;
									break;
								}
								k++;
							}
							for (int k = 0; k < h; k++) {
								cur = *(cur.next);
							}
						}
						while (cur.next != NULL || q < cur.length) {
							if (j < N && q < N) {
								T.T1.cur->S.cur->Info.stroka[j] = cur.Info.stroka[q];
							}
							j++;
							if (j == N) {
								T.T1.cur->S.cur = T.T1.cur->S.cur->next;
								j = 0;
							}
							q++;
							if (q >= N) {
								int k = 1;
								while (1) {
									q = q - N;
									if (q < N) {
										h = k;
										break;
									}
								}
								for (int k = 0; k < h; k++) {
									cur = *(cur.next);
								}
							}
						}
						if (T.T1.cur->S.head->length - r1 > 0)
							T.T1.cur->S.head->length -= r1;
						else if (T.T1.cur->S.head->length - r1 == 0) {
							T.T1.cur->S.cur = T.T1.cur->S.last;
							T.T1.cur->S.prev = setPrev(T.T1.cur->S);
							T.T1.cur->S.last = T.T1.cur->S.prev;
							T.T1.cur->S.last->next = NULL;
							T.T1.cur->S.head->length = 5;
						}
						else {
							int e = T.T1.cur->S.head->length - r1;
							T.T1.cur->S.cur->next = NULL;
							T.T1.cur->S.last = T.T1.cur->S.cur;
							T.T1.cur->S.head->length = 5 + e;
						}
					}
					else {
						T.T2.cur->S.cur = T.T2.cur->S.head;
						T.T3.cur->S.cur = T.T3.cur->S.head;
						j = lengthStr(T.T1.cur->S) - 1;
						int k = 0;
						while (1) {
							j = j - N;
							if (j < N) {
								h = k;
								break;
							}
							k++;
						}
						T.T1.cur->S.cur = T.T1.cur->S.last;
						int e = T.T1.cur->S.head->length - r1;
						if (e >= 5) {
							k = e / 5;
							for (int u = 0; u < k; u++) {
								StrL a;
								T.T1.cur->S.head = puchBack(T.T1.cur->S, a);
								T.T1.cur->S.setLast();
								T.T1.cur->S.head->length = 0;
								T.T1.cur->S.prev = new StrL;
								T.T1.cur->S.prev = setPrev(T.T1.cur->S);
								T.T1.cur->S.cur = setPrev1(T.T1.cur->S, T.T1.cur->S.last);
							}
							t = colBloks(T.T1.cur->S) - 1;
						}
						cur = *(T.T1.cur->S.cur);
						pcur = *(T.T1.cur->S.head);
						while (!ravno(*(pcur.next), cur)) {
							pcur = *(pcur.next);
						}
						cur.length = T.T1.cur->S.head->length;
						q = j - r1;
						if (q >= N) {
							int k = 1;
							while (1) {
								q = q - N;
								if (q < N) {
									h = k;
									break;
								}
							}
							for (int k = 0; k < h; k++) {
								T.T1.cur->S.cur = T.T1.cur->S.cur->next;
							}
						}
						while (j + t * N >= ch + lengthStr(T.T2.cur->S)) {
							T.T1.cur->S.cur->Info.stroka[q] = cur.Info.stroka[j];
							j--;
							if (j < 0) {
								cur = pcur;
								pcur = *(T.T1.cur->S.head);
								while (!ravno(*(pcur.next), cur)) {
									pcur = *(pcur.next);
								}
								j += N;
								t--;
							}
							q--;
							if (q < 0) {
								T.T1.cur->S.cur = T.T1.cur->S.prev;
								T.T1.cur->S.prev = setPrev(T.T1.cur->S);
								q += N;
							}
						}
						h = ch / N;
						j = ch - N * h;
						T.T1.cur->S.cur = T.T1.cur->S.head;
						T.T3.cur->S.cur = T.T3.cur->S.head;
						for (int k = 0; k < h; k++) {
							T.T1.cur->S.cur = T.T1.cur->S.cur->next;
						}
						q = 0;
						for (int k = 0; k < lengthStr(T.T3.cur->S); k++) {
							T.T1.cur->S.cur->Info.stroka[j] = T.T3.cur->S.cur->Info.stroka[q];
							j++;
							if (j == N) {
								T.T1.cur->S.cur = T.T1.cur->S.cur->next;
								j = 0;
							}
							q++;
							if (q == N) {
								T.T3.cur->S.cur = T.T3.cur->S.cur->next;
								q = 0;
							}
						}
						if (e <= 5) {
							T.T1.cur->S.head->length = T.T1.cur->S.head->length - r1;
						}
						else {
							while (e > 5) {
								e -= N;
							}
							T.T1.cur->S.head->length = e;
						}
					}
					T.T1.cur = T.T1.cur->next;
					T.T3.cur = T.T3.cur->next;
					T.T2.cur = T.T2.cur->next;
					h = ch / N;
					j = ch - N * h;
					q = 0;
				}
				for (int i = 0; i < r2; i++) {
					T.T1.cur->S.cur = T.T1.cur->S.head;
					T.T2.cur->S.cur = T.T2.cur->S.head;
					r3 = lengthStr(T.T2.cur->S);
					h = ch / N;
					j = ch - N * h;
					q = j + r3;
					int r = lengthStr(T.T1.cur->S) - q;
					cur = *(T.T1.cur->S.head);
					if (q >= N) {
						int k = 1;
						while (1) {
							q = q - N;
							if (q < N) {
								h = k;
								break;
							}
						}
						for (int k = 0; k < h; k++) {
							cur = *(cur.next);
						}
					}
					for (int u = 0; u < r; u++) {
						if (j < N && q < N) {
							T.T1.cur->S.cur->Info.stroka[j] = cur.Info.stroka[q];
						}
						j++;
						if (j == N) {
							T.T1.cur->S.cur = T.T1.cur->S.cur->next;
							j = 0;
						}
						q++;
						if (q == N) {
							cur = *(cur.next);
							q = 0;
						}
					}
					if (T.T1.cur->S.head->length - r3 > 0)
						T.T1.cur->S.head->length -= r3;
					else if (T.T1.cur->S.head->length - r3 == 0) {
						T.T1.cur->S.cur = T.T1.cur->S.last;
						T.T1.cur->S.prev = setPrev(T.T1.cur->S);
						T.T1.cur->S.last = T.T1.cur->S.prev;
						T.T1.cur->S.last->next = NULL;
						T.T1.cur->S.head->length = 5;
					}
					else {
						int e = T.T1.cur->S.head->length - r3;
						T.T1.cur->S.cur->next = NULL;
						T.T1.cur->S.last = T.T1.cur->S.cur;
						T.T1.cur->S.head->length = 5 + e;
					}
					T.T1.cur = T.T1.cur->next;
					T.T2.cur = T.T2.cur->next;
				}
			}
			else {
				if (fst + lengthText(T.T3) > lengthText(T.T1)) {
					if (ch == 0) {
						int x = fst + lengthText(T.T3) - lengthText(T.T1);
						for (int u = 0; u < x; u++) {
							BL1 a;
							a.init();
							T.T1.head = puchBack(T.T1, a);
							T.T1.setLast();
						}
					}
					else
						return -1;
				}
				l = lengthText(T.T2);
				T.T1.cur = T.T1.head;
				for (int i = 0; i < fst; i++) {
					T.T1.cur = T.T1.cur->next;
				}
				T.T2.cur = T.T2.head;
				T.T3.cur = T.T3.head;
				for (int i = 0; i < l; i++) {
					T.T1.cur->S.cur = T.T1.cur->S.head;
					int k1 = ch / N;
					for (int l = 0; l < k1; l++) {
						T.T1.cur->S.cur = T.T1.cur->S.cur->next;
					}
					j = ch - N * k1;
					r1 = lengthStr(T.T2.cur->S) - lengthStr(T.T3.cur->S);
					if (r1 >= 0) {
						T.T2.cur->S.cur = T.T2.cur->S.head;
						T.T3.cur->S.cur = T.T3.cur->S.head;

						q = 0;
						for (int k = 0; k < lengthStr(T.T3.cur->S); k++) {
							T.T1.cur->S.cur->Info.stroka[j] = T.T3.cur->S.cur->Info.stroka[q];
							j++;
							if (j == N) {
								T.T1.cur->S.cur = T.T1.cur->S.cur->next;
								j = 0;
							}
							q++;
							if (q == N) {
								T.T3.cur->S.cur = T.T3.cur->S.cur->next;
								q = 0;
							}
						}
						if (j >= N) {
							int k = 1;
							while (1) {
								j = j - N;
								if (j < N) {
									h = k;
									break;
								}
								k++;
							}
							for (int k = 0; k < h; k++) {
								T.T1.cur->S.cur = T.T1.cur->S.cur->next;
							}
						}
						cur = *(T.T1.cur->S.cur);
						cur.length = T.T1.cur->S.head->length;
						q = j + r1;
						if (q >= N) {
							int k = 1;
							while (1) {
								q = q - N;
								if (q < N) {
									h = k;
									break;
								}
								k++;
							}
							for (int k = 0; k < h; k++) {
								cur = *(cur.next);
							}
						}
						while (cur.next != NULL || q < T.T1.cur->S.head->length) {
							if (j < N && q < N) {
								T.T1.cur->S.cur->Info.stroka[j] = cur.Info.stroka[q];
							}
							j++;
							if (j == N) {
								T.T1.cur->S.cur = T.T1.cur->S.cur->next;
								j = 0;
							}
							q++;
							if (q >= N && cur.next != NULL) {
								int k = 1;
								while (1) {
									q = q - N;
									if (q < N) {
										h = k;
										break;
									}
								}
								for (int k = 0; k < h; k++) {
									if (cur.next != NULL)
										cur = *(cur.next);
								}
							}
						}
						if (T.T1.cur->S.head->length - r1 > 0)
							T.T1.cur->S.head->length -= r1;
						else if (T.T1.cur->S.head->length - r1 == 0) {
							T.T1.cur->S.cur = T.T1.cur->S.last;
							T.T1.cur->S.prev = setPrev(T.T1.cur->S);
							T.T1.cur->S.last = T.T1.cur->S.prev;
							T.T1.cur->S.last->next = NULL;
							T.T1.cur->S.head->length = 5;
						}
						else {
							int e = T.T1.cur->S.head->length - r1;
							T.T1.cur->S.cur->next = NULL;
							T.T1.cur->S.last = T.T1.cur->S.cur;
							T.T1.cur->S.head->length = 5 + e;
						}
					}
					else {
						T.T2.cur->S.cur = T.T2.cur->S.head;
						T.T3.cur->S.cur = T.T3.cur->S.head;
						j = lengthStr(T.T1.cur->S) - 1;
						int k = 0;
						while (1) {
							j = j - N;
							if (j < N) {
								h = k;
								break;
							}
							k++;
						}
						T.T1.cur->S.cur = T.T1.cur->S.last;
						T.T1.cur->S.prev = setPrev(T.T1.cur->S);
						int e = T.T1.cur->S.head->length - r1;
						t = colBloks(T.T1.cur->S);
						if (e > 5) {
							k = e / 5;
							for (int u = 0; u < k; u++) {
								StrL a;
								T.T1.cur->S.head = puchBack(T.T1.cur->S, a);
								T.T1.cur->S.setLast();
								T.T1.cur->S.head->length = 0;
								T.T1.cur->S.prev = new StrL;
								T.T1.cur->S.prev = setPrev(T.T1.cur->S);
								T.T1.cur->S.cur = setPrev1(T.T1.cur->S, T.T1.cur->S.last);
							};
						}
						cur = *(T.T1.cur->S.cur);
						pcur = *(T.T1.cur->S.head);
						if (!ravno(cur, pcur)) {
							while (!ravno(*(pcur.next), cur)) {
								pcur = *(pcur.next);
							}
						}
						cur.length = T.T1.cur->S.head->length;
						q = j - r1;
						if (q >= N) {
							int k = 1;
							while (1) {
								q = q - N;
								if (q < N) {
									h = k;
									break;
								}
							}
							for (int k = 0; k < h; k++) {
								T.T1.cur->S.cur = T.T1.cur->S.cur->next;
							}
						}
						int p = j + t * N;
						while (p >= ch + lengthStr(T.T2.cur->S)) {
							T.T1.cur->S.cur->Info.stroka[q] = cur.Info.stroka[j];
							j--;
							if (j < 0) {
								cur = pcur;
								pcur = *(T.T1.cur->S.head);
								if (!ravno(cur, pcur)) {
									while (!ravno(*(pcur.next), cur)) {
										pcur = *(pcur.next);
									}
								}
								j += N;
								t--;
							}
							q--;
							if (q < 0) {
								T.T1.cur->S.cur = T.T1.cur->S.prev;
								T.T1.cur->S.prev = setPrev(T.T1.cur->S);
								q += N;
							}
							p = j + t * N;
						}

						h = ch / N;
						j = ch - N * h;
						T.T1.cur->S.cur = T.T1.cur->S.head;
						T.T3.cur->S.cur = T.T3.cur->S.head;
						for (int k = 0; k < h; k++) {
							T.T1.cur->S.cur = T.T1.cur->S.cur->next;
						}
						q = 0;
						for (int k = 0; k < lengthStr(T.T3.cur->S); k++) {
							T.T1.cur->S.cur->Info.stroka[j] = T.T3.cur->S.cur->Info.stroka[q];
							j++;
							if (j == N) {
								T.T1.cur->S.cur = T.T1.cur->S.cur->next;
								j = 0;
							}
							q++;
							if (q == N) {
								T.T3.cur->S.cur = T.T3.cur->S.cur->next;
								q = 0;
							}
						}
						if (e <= 5) {
							T.T1.cur->S.head->length = T.T1.cur->S.head->length - r1;
						}
						else {
							while (e > 5) {
								e -= N;
							}
							T.T1.cur->S.head->length = e;
						}
					}
					T.T1.cur = T.T1.cur->next;
					T.T3.cur = T.T3.cur->next;
					T.T2.cur = T.T2.cur->next;
					h = ch / N;
					j = ch - N * h;
					q = 0;
				}
				for (int u = 0; u < lengthText(T.T3) - lengthText(T.T2); u++) {
					if (lengthStr(T.T1.cur->S) != ch) {
						T.T1.cur->S.cur = T.T1.cur->S.head;
						if (T.T1.cur->S.head->length > 0) {
							r1 = -lengthStr(T.T3.cur->S);
							T.T1.cur->S.cur = T.T1.cur->S.head;
							T.T3.cur->S.cur = T.T3.cur->S.head;
							j = lengthStr(T.T1.cur->S) - 1;
							int k = 0;
							while (1) {
								j = j - N;
								if (j < N) {
									h = k;
									break;
								}
								k++;
							}
							T.T1.cur->S.cur = T.T1.cur->S.last;
							int e = T.T1.cur->S.head->length - r1;
							if (e >= 5) {
								k = e / 5;
								for (int u = 0; u < k; u++) {
									StrL a;
									T.T1.cur->S.head = puchBack(T.T1.cur->S, a);
									T.T1.cur->S.setLast();
									T.T1.cur->S.head->length = 0;
									T.T1.cur->S.prev = new StrL;
									T.T1.cur->S.prev = setPrev(T.T1.cur->S);
									T.T1.cur->S.cur = setPrev1(T.T1.cur->S, T.T1.cur->S.last);
								}
								t = colBloks(T.T1.cur->S);
							}
							cur = *(T.T1.cur->S.cur);
							pcur = *(T.T1.cur->S.head);
							if (!ravno(cur, pcur)) {
								while (!ravno(*(pcur.next), cur)) {
									pcur = *(pcur.next);
								}
								t--;
							}
							cur.length = T.T1.cur->S.head->length;
							q = j - r1;
							if (q >= N) {
								int k = 1;
								while (1) {
									q = q - N;
									if (q < N) {
										h = k;
										break;
									}
								}
								for (int k = 0; k < h; k++) {
									T.T1.cur->S.cur = T.T1.cur->S.cur->next;
								}
							}
							int p = j + t * N;
							while (p >= ch - 1) {
								T.T1.cur->S.cur->Info.stroka[q] = cur.Info.stroka[j];
								j--;
								if (j < 0) {
									cur = pcur;
									if (t > 0) {
										pcur = *(T.T1.cur->S.head);
										if (!ravno(pcur, cur)) {
											while (!ravno(*(pcur.next), cur)) {
												pcur = *(pcur.next);
											}
										}
										j += N;
										t--;
									}
								}
								q--;
								if (q < 0) {
									T.T1.cur->S.cur = T.T1.cur->S.prev;
									if (T.T1.cur->S.cur != T.T1.cur->S.head) {
										T.T1.cur->S.prev = setPrev(T.T1.cur->S);
									}
									q += N;
								}
								p = j + t * (N);
							}
							h = ch / N;
							j = ch - N * h;
							T.T1.cur->S.cur = T.T1.cur->S.head;
							T.T3.cur->S.cur = T.T3.cur->S.head;
							for (int k = 0; k < h; k++) {
								T.T1.cur->S.cur = T.T1.cur->S.cur->next;
							}
							q = 0;
							for (int k = 0; k < lengthStr(T.T3.cur->S); k++) {
								T.T1.cur->S.cur->Info.stroka[j] = T.T3.cur->S.cur->Info.stroka[q];
								j++;
								if (j == N) {
									T.T1.cur->S.cur = T.T1.cur->S.cur->next;
									j = 0;
								}
								q++;
								if (q == N) {
									T.T3.cur->S.cur = T.T3.cur->S.cur->next;
									q = 0;
								}
							}
							if (e <= 5) {
								T.T1.cur->S.head->length = T.T1.cur->S.head->length - r1;
							}
							else {
								while (e > 5) {
									e -= N;
								}
								T.T1.cur->S.head->length = e;
							}
						}
						else {
							if (lengthStr(T.T3.cur->S) > N) {
								StrL a;
								T.T1.cur->S.head = puchBack(T.T1.cur->S, a);
							}
							q = 0;
							for (int k = 0; k < lengthStr(T.T3.cur->S); k++) {
								T.T1.cur->S.cur->Info.stroka[j] = T.T3.cur->S.cur->Info.stroka[q];
								j++;
								if (j == N) {
									T.T1.cur->S.cur = T.T1.cur->S.cur->next;
									j = 0;
								}
								q++;
								if (q == N) {
									T.T3.cur->S.cur = T.T3.cur->S.cur->next;
									q = 0;
								}
							}
							T.T1.cur->S.head->length = T.T3.cur->S.head->length;
						}
						T.T1.cur = T.T1.cur->next;
						T.T3.cur = T.T3.cur->next;
						h = ch / N;
						j = ch - N * h;
						q = 0;
					}
					else {
						int k1 = ch / N;
						T.T1.cur->S.cur = T.T1.cur->S.head;
						for (int l = 0; l < k1; l++) {
							T.T1.cur->S.cur = T.T1.cur->S.cur->next;
						}
						j = ch - N * k1;
						t = 0;
						q = 0;
						for (int i = 0; i < lengthStr(T.T3.cur->S); i++) {
							T.T1.cur->S.cur->Info.stroka[j] = T.T3.cur->S.cur->Info.stroka[q];
							j++;
							if (j == N) {
								StrL a;
								T.T1.cur->S.head = puchBack(T.T1.cur->S, a);
								T.T1.cur->S.setLast();
								T.T1.cur->S.head->length = 0;
								T.T1.cur->S.cur = T.T1.cur->S.last;
								j = 0;
								t++;
							}
							q++;
							if (q == N) {
								T.T3.cur->S.cur = T.T3.cur->S.cur->next;
								q = 0;
							}
						}
						T.T1.cur->S.head->length = j;
					}
				}
				
			}
		}
	}
	ch1 = ch + maxlengthStr(T.T2);
	return ch;
}
	