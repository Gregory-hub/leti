#include"Elem.h"
Elem* obrabotka(Elem* head) {
	Elem* cur = head;
	cur = head;
	bool h = false;
	int i = maxStrLen(head) - 1;
	while (i > -1) {
		while (cur->next != NULL) {
			if (i >= cur->s.length) {
				cur = cur->next;
				continue;
			}
			if (cur->s.stroka[i] > cur->next->s.stroka[i]) {
				head = change(head, cur, cur->next);
				h = true;
			}
			else
				cur = cur->next;
		}
		if (!h) {
			i--;
		}
		cur = head;
		h = false;
	}
	return head;
}