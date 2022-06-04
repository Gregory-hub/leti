#include"AllList.h"
int obrabotka(AllList& T) {
	int ch1, lst, fst, ch, ch2;
	ch1 = fst = ch = lst = 0;
	numbers* headn = new numbers;
	ch = set_numbers(T, headn);
	sort(headn);
	FormB chTc, chT;
	chTc = cut(T.T1,headn->number, headn->next->number);
	AllList chTs;
	chTs.T1 = chTc;
	chTs.T2 = T.T2;
	chTs.T3 = T.T3;
	change(chTs, ch1, lst, fst);
	chTc = chTs.T1;
	putProbel(chTc);
	chT = chTc;
	headn = headn->next;
	while (headn->next != NULL) {
		ch1 = lst = fst = 0;
		chTc = cut(T.T1, headn->number, headn->next->number);
		chTs.T1 = chTc;
		chTs.T2 = T.T2;
		chTs.T3 = T.T3;
		change(chTs, ch1, fst, lst);
		chTc = chTs.T1;
		putProbel(chTc);
		ch2 = puchBack(chT, chTc);
		headn = headn->next;
	}
	ch1 = fst = lst = 0;
	chTc = cutLast(T.T1, headn->number);
	chTs.T1 = chTc;
	chTs.T2 = T.T2;
	chTs.T3 = T.T3;
	change(chTs, ch1, fst, lst);
	chTc = chTs.T1;
	ch2 = puchBack(chT, chTc);
	deleteProbelk(chT);
	T.T1 = chT;
	return ch;
}