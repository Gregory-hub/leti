#include "functions.h"

void out(fstream& file, Elem* first) {
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    if (first == NULL) {
        file << "The list is empty" << endl;
        cout << "The list is empty" << endl;
    }
    else {
        int num = 0;
        printel(file, first, num);
        num++;
        Elem* curr = first->next;
        while (curr != NULL) {
            printel(file, curr, num);
            curr = curr->next;
            num++;
        }
        file << "-> NULL" << endl;
        cout << "-> NULL" << endl;
    }
}


void printel(fstream& file, Elem* el, int num) {
    if (num != 0) {
        file << "-> ";
        cout << "-> ";
    }
    file << num << ". ";
    cout << num << ". ";
    for (int i = 0; i < el->str.len; i++) {
        file << el->str.letters[i];
        cout << el->str.letters[i];
    }
    file << endl;
    cout << endl;
}

