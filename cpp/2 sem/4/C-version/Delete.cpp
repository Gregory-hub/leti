#include"AllList.h"
void DeleteHeadStr(StrL** head) {
	if (!is_Empty(*head)) {
		StrL* p = (*head)->next;
		delete (*head);
		(*head) = p;
	}
}
void deleteS(FormS* p) {
	if (!is_Empty(p->head)) {
		while (p->head != NULL)
			DeleteHeadStr(&(p->head));
	}
}
void DeleteHeadBL1(BL1** head) {
	if (!is_Empty(*head)) {
		BL1* p = (*head)->next;
		deleteS(&((*head)->S));
		delete (*head);
		(*head) = p;
	}
}
void DeleteB(FormB* p) {
	if (!is_Empty(p->head)) {
		while (p->head != NULL)
			DeleteHeadBL1(&(p->head));
	}
}
void Delete(AllList *p) {
	DeleteB(&(p->T1));
	DeleteB(&(p->T2));
	DeleteB(&(p->T3));
}