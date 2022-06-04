#include"AllList.h"
void FormS:: setLast() {
	cur = head;
	while (cur->next != NULL) {
		cur = cur->next;
	}
	last = cur;
}
void FormB::setLast() {
	cur = head;
	while (cur->next != NULL) {
		cur = cur->next;
	}
	last = cur;
}