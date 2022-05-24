#include "functions.h"


Elem* insert_before_last(Elem* first, Elem* first_inserted) {
    if (first == NULL) {
        return first;
    }
    if (first_inserted == NULL) {
        return first;
    }

    Elem* prelast = first;
    Elem* last = first->next;

    if (first->next == NULL) {
        last = first;
    }
    else {
        while (last != NULL && last->next != NULL) {
            last = last->next;
            prelast = prelast->next;
        }
    }

    Elem* last_inserted = first_inserted;
    while (last_inserted->next != NULL) {
        last_inserted = last_inserted->next;
    }

    if (first->next != NULL) {
        prelast->next = first_inserted;
    }
    else {
        first = first_inserted;
    }
	last_inserted->next = last;
    return first;
}

