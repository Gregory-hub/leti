#include "functions.h"


Elem* in(string filename) {
    fstream file;
    file.open(filename, ios::in);
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    string input;

    Elem* prev = new Elem;
	Elem* first = prev;

    if (getline(file, input) && !input.empty()) {
        Str str = create_str((char*)input.c_str());
        prev->str = str;
    }
    else {
        return NULL;
    }

    while (getline(file, input) && !input.empty()) {
        Str str = create_str((char*)input.c_str());
        Elem* curr = new Elem;
        curr->str = str;
        prev->next = curr;
        prev = curr;
    }

    prev->next = NULL;

    file.close();
    return first;
}


Str create_str(char* word) {
    Str* str = new Str;
    for (char* t = word; *t != '\0'; t++) {
        str->len++;
        str->letters[t - word] = *t;
    }
    return *str;
}

