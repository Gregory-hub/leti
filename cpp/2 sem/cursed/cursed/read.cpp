#include "read.h"


FormV* read(string filename, unsigned int max_line_len) {
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
	FormG* form_g = create_formG((char*)line.c_str(), max_line_len);
	first->setForm(form_g);

    V_El* prev;
    V_El* curr = new V_El;
    curr = first;
    while (getline(file, line)) {
		prev = curr;
        curr = new V_El;
		form_g = create_formG((char*)line.c_str(), max_line_len);
        prev->setNext(curr);
        curr->setForm(form_g);
    }

    form_v->setHead(first);
    form_v->setCurr(first);

    file.close();
    return form_v;
}


FormG* create_formG(char* line, unsigned int max_line_len) {
    FormG* form = new FormG;
    if (max_line_len < MAX_STR_LEN) {
        G_El* head = create_G_El(line, 0, max_line_len);
        form->setHead(head);
        form->setCurr(head);
        return form;
    }
	G_El* head = create_G_El(line, 0, MAX_STR_LEN);
    G_El* curr = new G_El;
    curr = head;
    G_El* prev = new G_El;

    unsigned int line_len = 0;
    while (curr->getStr()->getLen() == MAX_STR_LEN && line_len < max_line_len) {
        if (line_len + MAX_STR_LEN > max_line_len) {
            line_len = max_line_len;
        }
        else {
            line_len += MAX_STR_LEN;
        }
        prev = curr;
        curr = create_G_El(line, line_len, max_line_len - line_len);
        if (curr->getStr()->getLen() != 0) {
            prev->setNext(curr);
        }
    }

    form->setCurr(head);
    form->setHead(head);

    return form;
}


G_El* create_G_El(char* line, unsigned int i, unsigned int str_len) {
    G_El* el = new G_El;
    Str* str = create_str(line, i, str_len);
    el->setStr(str);

    return el;
}


Str* create_str(char* line, unsigned int i, unsigned int str_len) {
    Str* str = new Str;
    unsigned int len = 0;
    char sym = line[i];
    while (sym != '\0' && len < str_len && len < MAX_STR_LEN) {
        str->setLetter(len, sym);
        len++;
        i++;
        sym = line[i];
    }
    if (len < MAX_STR_LEN) {
        str->setLen(len);
    }

    return str;
}

