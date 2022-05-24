#include"Elem.h"
bool is_Empty(Elem* head) {
	return head == NULL;
}
Elem* DeleteHead(Elem* head) {
	if (!is_Empty(head)) {
		Elem* p = head ->next;
		delete head;
		return(p);
	}
}
Elem* deleteList(Elem* head) {
	if (!is_Empty(head)) {
		while (head != NULL)
			head = DeleteHead(head);
	}
	return head;
}
int minStrLen(Elem* head) {
	int minl = head->s.length;
	Elem* cur = head->next;
	while (cur != NULL) {
		if (cur->s.length < minl)
			minl = cur->s.length;
		cur = cur->next;
	}
	return minl;
}
int maxStrLen(Elem* head) {
	int maxl = head->s.length;
	Elem* cur = head->next;
	while (cur != NULL) {
		if (cur->s.length > maxl)
			maxl = cur->s.length;
		cur = cur->next;
	}
	return maxl;
}