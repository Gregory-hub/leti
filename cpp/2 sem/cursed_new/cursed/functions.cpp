#include "functions.h"


Str* copy_str(Str* str) {
	Str* str_copy = new Str;
	for (int i = 0; i < str->getLen(); i++) {
		str_copy->setLetter(i, str->getLetter(i));
	}
	str_copy->setLen(str->getLen());
	return str_copy;
}


FormG* copy_form_g(FormG* form_g) {
	FormG* form_g_copy = new FormG;
	G_El* curr_g_el = form_g->getCurr();
	G_El* prev_g_el = form_g->getPrev();
	G_El* curr_g_el_copy = nullptr;
	G_El* prev_g_el_copy = nullptr;

	while (form_g->getCurr() != nullptr) {
		G_El* g_el = form_g->getCurr();
		G_El* g_el_copy = new G_El;
		Str* str = g_el->getStr();
		Str* str_copy = copy_str(str);
		g_el_copy->setStr(str_copy);

		if (form_g_copy->getCurr() == nullptr) {
			form_g_copy->setHead(g_el_copy);
			form_g_copy->setCurr(g_el_copy);
		}
		else {
			form_g_copy->setPrev(form_g_copy->getCurr());
			form_g_copy->setCurr(g_el_copy);

			form_g_copy->getPrev()->setNext(g_el_copy);
			form_g_copy->getCurr()->setPrev(form_g_copy->getPrev());
		}

		if (form_g_copy->getCurr()->equals(curr_g_el)) {
			curr_g_el_copy = form_g_copy->getCurr();
			prev_g_el_copy = form_g_copy->getPrev();
		}

		form_g->setPrev(form_g->getCurr());
		form_g->setCurr(form_g->getCurr()->getNext());
	}

	form_g->setCurr(curr_g_el);
	form_g->setPrev(prev_g_el);
	form_g_copy->setCurr(curr_g_el_copy);
	form_g_copy->setPrev(prev_g_el_copy);

	return form_g_copy;
}


//FormV* copy(FormV* form_v) {
//	FormV* form_v_copy = new FormV;
//	V_El* v_el_copy_prev = nullptr;
//
//	while (form_v->getCurr() != nullptr) {
//		V_El* v_el = form_v->getCurr();
//		V_El* v_el_copy = new V_El;
//		FormG* form_g = v_el->getForm();
//		FormG* form_g_copy = copy_form_g(form_g);
//		v_el_copy->setForm(form_g_copy);
//
//		if (v_el_copy_prev == nullptr) {
//			form_v_copy->setHead(v_el_copy);
//		}
//		else {
//			v_el_copy_prev->setNext(v_el_copy);
//		}
//		v_el_copy_prev = v_el_copy;
//		form_v->setPrev(form_v->getCurr());
//		form_v->setCurr(form_v->getCurr()->getNext());
//	}
//
//	form_v->setCurr(form_v->getHead());
//	form_v->setPrev(nullptr);
//	form_v_copy->setCurr(form_v_copy->getHead());
//	form_v_copy->setPrev(nullptr);
//
//	return form_v_copy;
//}
//
//
//bool el_is_in(G_El* target_el, FormV* form_v) {
//	while (form_v->getCurr() != nullptr) {
//		V_El* curr_v_el = form_v->getCurr();
//		FormG* form_g = curr_v_el->getForm();
//		while (form_g->getCurr() != nullptr) {
//			if (target_el->equals(form_g->getCurr())) {
//				form_g->setCurr(form_g->getHead());
//				form_v->setCurr(form_v->getHead());
//				return true;
//			}
//			form_g->setCurr(form_g->getCurr()->getNext());
//		}
//		form_g->setCurr(form_g->getHead());
//		form_v->setCurr(form_v->getCurr()->getNext());
//	}
//	form_v->setCurr(form_v->getHead());
//	return false;
//}
//
//
//void handle_line(V_El* curr_v_el, FormV* form_v_2) {
//	FormG* form_g_1 = curr_v_el->getForm();
//	while (form_g_1->getCurr() != nullptr) {
//		if (el_is_in(form_g_1->getCurr(), form_v_2)) {
//			if (form_g_1->getCurr() == form_g_1->getHead()) {
//				form_g_1->setHead(form_g_1->getCurr()->getNext());
//				delete form_g_1->getCurr();
//				form_g_1->setCurr(form_g_1->getHead());
//				form_g_1->setPrev(nullptr);
//			}
//			else {
//				form_g_1->getPrev()->setNext(form_g_1->getCurr()->getNext());
//				delete form_g_1->getCurr();
//				form_g_1->setCurr(form_g_1->getPrev()->getNext());
//			}
//		}
//		else {
//			form_g_1->setPrev(form_g_1->getCurr());
//			form_g_1->setCurr(form_g_1->getCurr()->getNext());
//		}
//	}
//	form_g_1->setCurr(form_g_1->getHead());
//	form_g_1->setPrev(nullptr);
//}
//
//
//void handle_text(FormV* form_v_1, FormV* form_v_2) {
//	while (form_v_1->getCurr() != nullptr) {
//		handle_line(form_v_1->getCurr(), form_v_2);
//
//		if (form_v_1->getCurr()->getForm()->getHead() == nullptr) {
//			if (form_v_1->getCurr() == form_v_1->getHead()) {
//				form_v_1->setHead(form_v_1->getCurr()->getNext());
//				delete form_v_1->getCurr();
//				form_v_1->setCurr(form_v_1->getHead());
//				form_v_1->setPrev(nullptr);
//			}
//			else {
//				form_v_1->getPrev()->setNext(form_v_1->getCurr()->getNext());
//				delete form_v_1->getCurr();
//				form_v_1->setCurr(form_v_1->getPrev()->getNext());
//			}
//		}
//		else {
//			form_v_1->setPrev(form_v_1->getCurr());
//			form_v_1->setCurr(form_v_1->getCurr()->getNext());
//		}
//	}
//
//	form_v_1->setCurr(form_v_1->getHead());
//	form_v_1->setPrev(nullptr);
//}
//
//
//FormV* subtract(FormV* divided, FormV* dividor) {
//	FormV* result = copy(divided);
//	handle_text(result, dividor);
//	return result;
//}
//
//
//bool belongs(FormV* small_set, FormV* large_set) {
//	FormV* diff = subtract(small_set, large_set);
//	if (diff->getHead() == nullptr) {
//		return true;
//	}
//	return false;
//}


void desintegrate(FormV* form_v) {
	while (form_v->getCurr() != nullptr) {
		V_El* v_el = form_v->getCurr();
		FormG* form_g = v_el->getForm();
		while (form_g->getCurr() != nullptr) {
			delete form_g->getCurr()->getStr();
			form_g->setCurr(form_g->getCurr()->getNext());
			delete form_g->getPrev();
		}
		form_v->setCurr(form_v->getCurr()->getNext());
		delete form_v->getPrev();
	}

	delete form_v;
}


void set_line_index(stringstream& ss, int& line_index) {
	bool success = true;
	string index = "";
	getline(ss, index, ' ');
	if (index.length() != 0) {
		for (int i = 0; i < index.length(); i++) {
			if (!isdigit(index[i])) {
				if (i == 0 && index[i] == '-') {
					cerr << "Error: current line number must be greater than 0" << endl;
				}
				else {
					cerr << "Error: current line number must be an integer" << endl;
				}
				cout << "Current line number is " << line_index + 1 << endl;
				success = false;
			}
		}
	}
	else {
		cout << "Current line number is " << line_index + 1 << endl;
		success = false;
	}
	if (success && stoi(index) == 0) {
		cerr << "Error: current line number must be greater than 0" << endl;
		cout << "Current line number is " << line_index + 1 << endl;
		success = false;
	}
	if (success) {
		line_index = stoi(index) - 1;
		cout << "Line number has been set to " << index << endl;
	}
}


void print_g_el(G_El* g_el) {
	for (int i = 0; i < g_el->getStr()->getLen(); i++) {
		cout << g_el->getStr()->getLetter(i);
	}
}


void print_v_el(V_El* v_el) {
	G_El* first_g = v_el->getForm()->getHead();
	print_g_el(first_g);
	G_El* curr = first_g->getNext();
	while (curr != NULL) {
		print_g_el(curr);
		curr = curr->getNext();
	}
	cout << endl;
}


bool is_number(string num) {
	if (num.length() == 0) return false;
	for (int i = 0; i < num.length(); i++) {
		if (!isdigit(num[i]) && !(i == 0 && num[i] == '-')) {
			return false;
		}
	}
	return true;
}


void print(stringstream& ss, FormV* form_v, int line_index) {
	string num = "";
	getline(ss, num, ' ');
	if (num.length() != 0) {
		if (!is_number(num)) {
			cerr << "Error: number of lines must be an integer" << endl;
			return;
		}
		else if (stoi(num) < 0) {
			cerr << "Error: number of lines must be 0 or greater" << endl;
			return;
		}
	}
	else {
		num = "1";
	}
	if (stoi(num) < 0) {
		cerr << "Error: number of lines must be 0 or greater" << endl;
	}
	V_El* curr = form_v->getCurr();
	int i = 0;
	while (i < line_index && curr != nullptr) {
		curr = curr->getNext();
		i++;
	}

	int printed_count = 0;
	while (curr != nullptr && printed_count < stoi(num)) {
		print_v_el(curr);
		curr = curr->getNext();
		printed_count++;
	}
}


void throw_arg_exception(string arg, int len = -1) {
	cerr << "Error: invalid argument: '";
	if (len < 0) {
		cout << arg;
	}
	else {
		for (int i = 0; i < len; i++) {
			cout << arg[i];
		}
	}
	cout << "'" << endl;
}


int read_sequence(string rest_of_line, char* sequence) {
	if (rest_of_line == "") {
		cerr << "Error: sequence is not provided" << endl;
		return -1;
	}

	int i = 0;
	while (i < rest_of_line.length() && rest_of_line[i] == ' ') i++;
	if (rest_of_line[i] != '"') {
		cerr << "Error: sequence must be inside double quotes" << endl;
		return -1;
	}

	i++;
	int j = 0;
	for (; i + j < rest_of_line.length(); j++) {
		if (rest_of_line[i + j] == '"') break;
		sequence[j] = rest_of_line[i + j];
	}

	if (rest_of_line[i + j] != '"') {
		cerr << "Error: sequence must be inside double quotes" << endl;
		return -1;
	}

	if (i + j + 1 < rest_of_line.length() && rest_of_line[i + j + 1] != ' ') {
		int k = i + j;
		for (; (k < rest_of_line.length() && rest_of_line[k] != ' '); k++) {
			sequence[k - i] = rest_of_line[k];
		}
		throw_arg_exception('"' + string(sequence), k - i + 1);
		return -1;
	}
	if (j == 0) {
		cerr << "Error: sequence is not provided" << endl;
		return -1;
	}
	return j;
}


int read_number_of_symbols(stringstream& ss) {
	string num = "";
	getline(ss, num, ' ');
	getline(ss, num, ' ');
	string symbols_word = "";
	getline(ss, symbols_word, ' ');
	if (num.length() == 0) {
		cerr << "Error: number of symbols is not provided" << endl;
		return -1;
	}
	if (!is_number(num)) {
		throw_arg_exception(num);
		return -1;
	}
	if (symbols_word != "symbols" && symbols_word != "") {
		throw_arg_exception(symbols_word);
		return -1;
	}
	if (stoi(num) < 0) {
		cerr << "Error: number of symbols must be 0 or greater" << endl;
		return -1;
	}
	return stoi(num);
}


bool rest_of_line_is_empty(stringstream& ss) {
	string arg = "";
	getline(ss, arg, ' ');
	getline(ss, arg, ' ');
	if (arg != "") {
		throw_arg_exception(arg);
		return false;
	}
	return true;
}


void del_line_if_empty(FormV* form_v) {
	if (form_v->getCurr()->getForm()->getHead() == nullptr) {
		if (form_v->getCurr() == form_v->getHead()) {
			form_v->setHead(form_v->getCurr()->getNext());
			delete form_v->getCurr();
			form_v->setCurr(form_v->getHead());
		} 
		else {
			form_v->getPrev()->setNext(form_v->getCurr()->getNext());
			delete form_v->getCurr();
			form_v->setCurr(form_v->getPrev()->getNext());
		}
	}
	else {
		form_v->setPrev(form_v->getCurr());
		if (form_v->getCurr() != nullptr) form_v->setCurr(form_v->getCurr()->getNext());
	}
}


int find_in_g_el(FormG* form_g, char* subline, int len) {
	FormG* form_g_copy = copy_form_g(form_g);
	Str* str = form_g->getCurr()->getStr();
	int init_str_len = str->getLen();

	for (int i = 0; i < init_str_len; i++) {
		form_g_copy = copy_form_g(form_g);
		str = form_g->getCurr()->getStr();

		int j = 0;
		if (i + len <= str->getLen()) {
			while (j < len && str->getLetter(i + j) == subline[j]) j++;
		}
		else {
			while (j + i < str->getLen() && str->getLetter(i + j) == subline[j]) j++;

			if (j > 0) {
				while (form_g_copy->getCurr()->getNext() != nullptr && j < len) {
					int j_in_str = 0;
					form_g_copy->setCurr(form_g_copy->getCurr()->getNext());
					str = form_g_copy->getCurr()->getStr();
					while (j_in_str < str->getLen() && j < len && str->getLetter(j_in_str) == subline[j]) {
						j_in_str++;
						j++;
					}
					if (j < len && j_in_str < str->getLen()) break;
				}
			}
		}
		if (j == len) {
			return i;
		}
	}
	return -1;
}


void del_using_context(stringstream& ss, FormV* form_v, bool del_up, int number_of_lines, int line_index, bool del_before) {
	string rest_of_line = "";
	getline(ss, rest_of_line);

	char context[1000];
	int len = read_sequence(rest_of_line, context);
	if (len == -1) return;

	rest_of_line = rest_of_line.substr(len + 2) + ' ';		// +2 because of quotes
	ss.clear();
	ss.str(rest_of_line);
	int number_of_symbols = read_number_of_symbols(ss);

	cout << "Del using context:" << endl;
	cout << "Context:";
	for (int j = 0; j < len; j++) cout << context[j];
	cout << endl;
	cout << "Number of symbols:" << number_of_symbols << endl;

	if (!rest_of_line_is_empty(ss)) return;

	int i = 0;
	for (; form_v->getCurr() != nullptr && i < line_index; i++) {
		form_v->setPrev(form_v->getCurr());
		form_v->setCurr(form_v->getCurr()->getNext());
	}

	for (; form_v->getCurr() != nullptr && i < line_index + number_of_lines; i++) {
		FormG* form_g = form_v->getCurr()->getForm();
		int i = -1;
		while (form_g->getCurr() != nullptr && i < 0) {
			i = find_in_g_el(form_g, context, len);
			if (i < 0) {
				form_g->setPrev(form_g->getCurr());
				form_g->setCurr(form_g->getCurr()->getNext());
			}
		}
		if (i >= 0) {		// deletion place is identified by form_g->getCurr() for g_el and i for index in g_el
			del_sequence(form_g, i + len, number_of_symbols);
			del_line_if_empty(form_v);
		}
		else {
			form_v->setPrev(form_v->getCurr());
			form_v->setCurr(form_v->getCurr()->getNext());
		}
	}

	form_v->reset();
}


void del_sequence(FormG* form_g, int i, int len) {
	// deletion place is identified by form_g->getCurr() for g_el and i for index in g_el
	if (form_g->getCurr() == nullptr) return;
	Str* str = form_g->getCurr()->getStr();

	if (i + len <= str->getLen()) {
		for (int j = i; j + len < str->getLen(); j++) {
			str->setLetter(j, str->getLetter(j + len));
		}
		str->setLen(str->getLen() - len);
		if (str->getLen() == 0) {
			if (form_g->getPrev() == nullptr && form_g->getCurr()->getNext() == nullptr) {
				delete form_g->getCurr();
				form_g->setCurr(nullptr);
				form_g->setHead(nullptr);
			}
			else if (form_g->getPrev() == nullptr) {
				form_g->setCurr(form_g->getCurr()->getNext());
				form_g->setHead(form_g->getCurr());
				delete form_g->getCurr()->getPrev();
			}
			else {
				form_g->getPrev()->setNext(form_g->getCurr()->getNext());
				delete form_g->getCurr();
				form_g->setCurr(form_g->getPrev()->getNext());
			}
			delete str;
		}
		len = 0;
	}
	else if (i != 0) {
		len -= str->getLen();
		len += i;
		str->setLen(i);
		form_g->setPrev(form_g->getCurr());
		form_g->setCurr(form_g->getCurr()->getNext());
	}

	while (len > 0 && form_g->getCurr() != nullptr) {
		str = form_g->getCurr()->getStr();
		if (len >= str->getLen()) {
			if (form_g->getPrev() == nullptr && form_g->getCurr()->getNext() == nullptr) {
				delete form_g->getCurr();
				form_g->setCurr(nullptr);
				form_g->setHead(nullptr);
			}
			else if (form_g->getPrev() == nullptr) {
				form_g->setCurr(form_g->getCurr()->getNext());
				form_g->setHead(form_g->getCurr());
				delete form_g->getCurr()->getPrev();
			}
			else {
				form_g->getPrev()->setNext(form_g->getCurr()->getNext());
				delete form_g->getCurr();
				form_g->setCurr(form_g->getPrev()->getNext());
			}
			len -= str->getLen();
			delete str;
		}
		else {
			for (int j = 0; j + len < str->getLen(); j++) {
				str->setLetter(j, str->getLetter(j + len));
			}
			str->setLen(str->getLen() - len);
			len = 0;
		}
	}
}
 

void del_subline(stringstream& ss, FormV* form_v, bool del_up, int number_of_lines, int line_index) {
	string rest_of_line = "";
	getline(ss, rest_of_line);

	char subline[1000];
	int len = read_sequence(rest_of_line, subline);
	if (len == -1) return;

	rest_of_line = rest_of_line.substr(len + 2) + ' ';		// +2 because of quotes
	ss.clear();
	ss.str(rest_of_line);
	if (!rest_of_line_is_empty(ss)) return;

	int i = 0;
	for (; form_v->getCurr() != nullptr && i < line_index; i++) {
		form_v->setPrev(form_v->getCurr());
		form_v->setCurr(form_v->getCurr()->getNext());
	}

	for (; form_v->getCurr() != nullptr && i < line_index + number_of_lines; i++) {
		FormG* form_g = form_v->getCurr()->getForm();
		int i = -1;
		while (form_g->getCurr() != nullptr && i < 0) {
			i = find_in_g_el(form_g, subline, len);
			if (i < 0) {
				form_g->setPrev(form_g->getCurr());
				form_g->setCurr(form_g->getCurr()->getNext());
			}
		}
		if (i >= 0) {		// deletion place is identified by form_g->getCurr() for g_el and i for index in g_el
			del_sequence(form_g, i, len);
			del_line_if_empty(form_v);
		}
		else {
			form_v->setPrev(form_v->getCurr());
			form_v->setCurr(form_v->getCurr()->getNext());
		}
	}

	form_v->reset();
}


void del_prefix(FormG* form_g, int number_of_symbols) {
	while (form_g->getHead() != nullptr && number_of_symbols > 0) {
		Str* str = form_g->getCurr()->getStr();
		if (number_of_symbols >= str->getLen()) {
			number_of_symbols -= str->getLen();
			form_g->setHead(form_g->getCurr()->getNext());
			delete form_g->getCurr()->getStr();
			delete form_g->getCurr();
			form_g->setCurr(form_g->getHead());
		}
		else {
			for (int i = 0; i + number_of_symbols < str->getLen(); i++) {
				str->setLetter(i, str->getLetter(i + number_of_symbols));
			}
			str->setLen(str->getLen() - number_of_symbols);
			number_of_symbols = 0;
		}
	}
}


void del_postfix(FormG* form_g, int number_of_symbols) {
	while (form_g->getCurr()->getNext() != nullptr) {
		form_g->setCurr(form_g->getCurr()->getNext());
	}

	while (form_g->getCurr() != nullptr && number_of_symbols > 0) {
		Str* str = form_g->getCurr()->getStr();
		if (number_of_symbols >= str->getLen()) {
			number_of_symbols -= str->getLen();
			if (form_g->getCurr()->getPrev() != nullptr) {
				form_g->setCurr(form_g->getCurr()->getPrev());
				delete form_g->getCurr()->getNext()->getStr();
				delete form_g->getCurr()->getNext();
				form_g->getCurr()->setNext(nullptr);
			}
			else {
				delete form_g->getCurr()->getStr();
				delete form_g->getCurr();
				form_g->setCurr(nullptr);
				form_g->setHead(nullptr);
			}
		}
		else {
			str->setLen(str->getLen() - number_of_symbols);
			number_of_symbols = 0;
		}
	}
}


void del_somefix(stringstream& ss, FormV* form_v, bool del_up, int number_of_lines, int line_index, bool is_prefix) {
	int number_of_symbols = read_number_of_symbols(ss);
	if (!rest_of_line_is_empty(ss)) return;

	for (int i = 0; form_v->getCurr() != nullptr && i < line_index; i++) {
		form_v->setPrev(form_v->getCurr());
		form_v->setCurr(form_v->getCurr()->getNext());
	}

	for (int i = 0; i < number_of_lines && form_v->getCurr() != nullptr; i++) {
		if (is_prefix) {
			del_prefix(form_v->getCurr()->getForm(), number_of_symbols);
		}
		else {
			del_postfix(form_v->getCurr()->getForm(), number_of_symbols);
		}
		del_line_if_empty(form_v);
	}

	form_v->reset();
}


void del(stringstream& ss, FormV* form_v, int line_index) {
	string arg = "";
	getline(ss, arg, ' ');
	bool del_up;
	if (arg == "up") {
		del_up = true;
	}
	else if (arg == "down") {
		del_up = false;
	}
	else if (arg == "") {
		cerr << "Error: arguments not provided" << endl;
		return;
	}
	else {
		throw_arg_exception(arg);
		return;
	}

	string num= "";
	getline(ss, num, ' ');
	if (num == "") {
		cerr << "Error: number of lines is not provided" << endl;
		return;
	}
	else if (!is_number(num)) {
		cerr << "Error: number of lines must be integer" << endl;
		return;
	}

	int number_of_lines = stoi(num);
	if (number_of_lines < 0) {
		cerr << "Error: number of lines must be 0 or greater" << endl;
		return;
	}

	arg = "";
	getline(ss, arg, ' ');
	if (arg == "before") {
		del_using_context(ss, form_v, del_up, number_of_lines, line_index, true);
	}
	else if (arg == "after") {
		del_using_context(ss, form_v, del_up, number_of_lines, line_index, false);
	}
	else if (arg == "subline") {
		del_subline(ss, form_v, del_up, number_of_lines, line_index);
	}
	else if (arg == "prefix") {
		del_somefix(ss, form_v, del_up, number_of_lines, line_index, true);
	}
	else if (arg == "postfix") {
		del_somefix(ss, form_v, del_up, number_of_lines, line_index, false);
	}
	else if (arg == "") {
		cerr << "Error: arguments not provided" << endl;
		return;
	}
	else {
		throw_arg_exception(arg);
		return;
	}
}

