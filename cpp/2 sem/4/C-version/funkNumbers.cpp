#include"AllList.h"
int set_numbers(AllList T, numbers* headn) {
	int ch, ch1, fst, lst, ch2 = 0;
	ch = ch1 = fst = lst = 0;
	ch = search(T, ch1, fst, lst);
	if (ch >= 0) {
		headn->number = ch;
		ch2 = ch;
	}
	else {
		headn = NULL;
		return -1;
	}
	numbers* cur = headn;
	headn = cur;
	while (ch >= 0) {
		ch = search(T, ch1, fst, lst);
		if (ch >= 0) {
			cur->next = new numbers;
			cur = cur->next;
			cur->number = ch;
			ch2 = ch;
		}
		else {
			cur = NULL;
			break;
		}
	}
	return ch2;
}
 void sort(numbers* headn) {
	int tmp;
	numbers* cur = headn;
	while (cur->next != NULL) {
		if (cur->number > cur->next->number) {
			tmp = cur->next->number;
			cur->next->number = cur->number;
			cur->number = tmp;
		}
		cur = cur->next;
	}
}