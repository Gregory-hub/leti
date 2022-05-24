#include"Elem.h"
void inp(Elem* head, fstream& f) {
	char ogr = '0';
	f >> ogr;
	strL st;
	st.inpStr(f, ogr);
	initList(head, st);
	Elem* cur = new Elem;
	st.inpStr(f, ogr);
	head->next = cur;
	cur->s = st;
	while (1) {
		st.inpStr(f, ogr);
		if (f.eof())
			break;
		cur->next = new Elem;
		cur = cur->next;
		cur->s = st;
	}
	cur ->next = NULL;
}