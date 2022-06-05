#include "functions.h"


void print_g_el(fstream& file, G_El* g_el) {
    cout << '"';
    file << '"';
    for (int i = 0; i < g_el->getStr()->getLen(); i++) {
        file << g_el->getStr()->getLetter(i);
        cout << g_el->getStr()->getLetter(i);
    }
    cout << '"';
    file << '"';
}



void print_v_el(fstream& file, V_El* v_el, int num) {
    if (num != 0) {
        file << "-> ";
        cout << "-> ";
    }
    file << num << ". ";
    cout << num << ". ";

    G_El* first_g = v_el->getForm()->getHead();
	print_g_el(file, first_g);
	G_El* curr = first_g->getNext();
	while (curr != NULL) {
		file << "->";
		cout << "->";
		print_g_el(file, curr);
		curr = curr->getNext();
	}
	file << "->NULL" << endl;
	cout << "->NULL" << endl;
}


void print_list(fstream& file, FormV* list) {
	V_El* first_v = list->getHead();
	if (first_v == nullptr) {
		file << "The list is empty" << endl << endl;
		cout << "The list is empty" << endl << endl;
	}
	else {
		int num = 0;
		print_v_el(file, first_v, num);
		num++;
		V_El* curr = first_v->getNext();
		while (curr != NULL) {
			print_v_el(file, curr, num);
			curr = curr->getNext();
			num++;
		}
		file << "-> NULL" << endl << endl;
		cout << "-> NULL" << endl << endl;
	}
}


void out(string filename, All* all, FormV* diff, bool result, bool app = false) {
	fstream file;
	if (app) {
		file.open(filename, ios::app);
	}
	else {
		file.open(filename, ios::out);
	}
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    FormV* form_list[3] = {all->list1, all->list2, all->list3};
    for (int i = 0; i < 3; i++) {
        cout << "LIST " << i + 1 << endl;
        file << "LIST " << i + 1 << endl;
        FormV* list = form_list[i];
		print_list(file, list);
    }

	if (diff != nullptr) {
		cout << "LIST 1 \\ LIST 2" << endl;
		file << "LIST 1 \\ LIST 2" << endl;
		print_list(file, diff);
	}

	cout << "LIST 3 belongs LIST 1 \\ LIST 2: ";
	file << "LIST 3 belongs LIST 1 \\ LIST 2: ";
	if (result) {
		cout << "true" << endl;
		file << "true" << endl;
	}
	else {
		cout << "false" << endl;
		file << "false" << endl;
	}

    file.close();
}

