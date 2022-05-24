#include"Elem.h"
Elem* change(Elem* head, Elem* lst1, Elem* lst2) {
	Elem* prev1, * prev2, * next1, * next2;
	prev1 = head;
	prev2 = head;
	if (prev1 == lst1)
		prev1 = NULL;
	else
		while (prev1->next != lst1)
			prev1 = prev1->next;
	if (prev2 == lst2)
		prev2 = NULL;
	else
		while (prev2->next != lst2)
			prev2 = prev2->next;
	next1 = lst1->next;
	next2 = lst2->next;
	if (lst2 == next1) {
		lst2->next = lst1;
		lst1->next = next2;
		if (lst1 != head)
			prev1->next = lst2;
	}
	else 
		if (lst1 == next2) {
			lst1->next = lst2;
			lst2->next = next1;
			if (lst2 != head) {
				prev2->next = lst2;
			}
		}
		else {
			if (lst1 != head)
				prev1->next = lst2;
			lst2->next = next1;
			if (lst2 != head)
				prev2->next = lst1;
			lst1->next = next2;
		}
	if (lst1 == head)
		return lst2;
	if (lst2 == head)
		return lst1;
	return head;
}