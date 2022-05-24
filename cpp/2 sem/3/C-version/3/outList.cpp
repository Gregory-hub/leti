#include"Elem.h"
void out(Elem* head, fstream& f) {
	if (!is_Empty(head)) {
		head->s.outStr(f);
		Elem* cur = head->next;
		cur->s.outStr(f);
		cur = cur->next;
		while (cur != NULL) {
			cur->s.outStr(f);
			cur = cur->next;
		}
		f << "NULL" << '\n';
	}
	else
		f << "пустой список" << '\n';
}