#pragma once
#include "FormV.h"


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


void out(string filename, FormV* form_v, bool app) {
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

	V_El* first_v = form_v->getHead();
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

	file.close();
}

