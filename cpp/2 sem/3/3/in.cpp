#include "functions.h"


Elem* in(string filename) {
    fstream file;
    file.open(filename, ios::in);
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    //char sep = '0';
    string input;
    //if (getline(file, input)) sep = input[0];

    Elem* prev = new Elem;
	Elem* first = prev;

    if (getline(file, input) && !input.empty()) {
        Str str = create_str((char*)input.c_str());
        prev->str = str;
        //int st = 0;
        //while (st < input.length() && input[st] == sep) st++;
        //int end = st + 1;
        //while (end < input.length() && input[end] != sep) end++;

        //string new_input;
        //for (int i = st; i < end; i++) {
        //    new_input[i - st] = input[i];
        //}
        //Str str = create_str((char*)new_input.c_str());
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

