#include "All.h"


void All::read(string filename1, string filename2, string filename3) {
    list1 = read_one(filename1);
    list2 = read_one(filename2);
    list3 = read_one(filename3);
}


FormV* read_one(string filename) {
    fstream file;
    file.open(filename, ios::in);
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    FormV* form_v = new FormV;
    V_El* first = new V_El;
    string line;

    getline(file, line);
	FormG* form_g = create_formG((char*)line.c_str());
	first->setForm(form_g);

    V_El* prev;
    V_El* curr = new V_El;
    curr = first;
    while (getline(file, line)) {
		prev = curr;
        curr = new V_El;
		form_g = create_formG((char*)line.c_str());
        prev->setNext(curr);
        curr->setForm(form_g);
    }

    form_v->setHead(first);
    form_v->setCurr(first);

    file.close();
    return form_v;
}


FormG* create_formG(char* line) {
    FormG* form = new FormG;
    G_El* head = create_G_El(line, 0);
    G_El* curr = head;
    G_El* prev = new G_El;

    unsigned int i = 0;
    while (curr->getStr()->getLen() == MAX_SIZE) {
        i += MAX_SIZE;
        prev = curr;
        curr = create_G_El(line, i);
        if (curr->getStr()->getLen() != 0) {
            prev->setNext(curr);
        }
    }

    form->setCurr(head);
    form->setHead(head);

    return form;
}


G_El* create_G_El(char* line, unsigned int i) {
    G_El* el = new G_El;
    Str* str = create_str(line, i);
    el->setStr(str);

    return el;
}


Str* create_str(char* line, unsigned int i) {
    Str* str = new Str;
    unsigned int len = 0;
    char sym = line[i];
    while (sym != '\0' && len < MAX_SIZE) {
        str->setLetter(len, sym);
        len++;
        i++;
        sym = line[i];
    }
    if (len < MAX_SIZE) {
        str->setLen(len);
    }

    return str;
}

