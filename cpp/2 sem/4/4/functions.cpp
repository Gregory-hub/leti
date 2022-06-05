#include "All.h"


Str* copy_str(Str* str) {
	Str* str_copy = new Str;
	for (int i = 0; i < str->getLen(); i++) {
		str_copy->setLetter(i, str->getLetter(i));
	}
	str_copy->setLen(str->getLen());
	return str_copy;
}


FormG* copy_form_g(FormG* form_g) {
	G_El* g_el_copy_prev = nullptr;
	FormG* form_g_copy = new FormG;

	while (form_g->getCurr() != nullptr) {
		G_El* g_el = form_g->getCurr();
		G_El* g_el_copy = new G_El;
		Str* str = g_el->getStr();
		Str* str_copy = copy_str(str);
		g_el_copy->setStr(str_copy);
		if (g_el_copy_prev == nullptr) {
			form_g_copy->setHead(g_el_copy);
		}
		else {
			g_el_copy_prev->setNext(g_el_copy);
		}
		g_el_copy_prev = g_el_copy;
		form_g->setPrev(form_g->getCurr());
		form_g->setCurr(form_g->getCurr()->getNext());
	}

	form_g->setCurr(form_g->getHead());
	form_g->setPrev(nullptr);
	form_g_copy->setCurr(form_g_copy->getHead());
	form_g_copy->setPrev(nullptr);

	return form_g_copy;
}


FormV* copy(FormV* form_v) {
	FormV* form_v_copy = new FormV;
	V_El* v_el_copy_prev = nullptr;

	while (form_v->getCurr() != nullptr) {
		V_El* v_el = form_v->getCurr();
		V_El* v_el_copy = new V_El;
		FormG* form_g = v_el->getForm();
		FormG* form_g_copy = copy_form_g(form_g);
		v_el_copy->setForm(form_g_copy);

		if (v_el_copy_prev == nullptr) {
			form_v_copy->setHead(v_el_copy);
		}
		else {
			v_el_copy_prev->setNext(v_el_copy);
		}
		v_el_copy_prev = v_el_copy;
		form_v->setPrev(form_v->getCurr());
		form_v->setCurr(form_v->getCurr()->getNext());
	}

	form_v->setCurr(form_v->getHead());
	form_v->setPrev(nullptr);
	form_v_copy->setCurr(form_v_copy->getHead());
	form_v_copy->setPrev(nullptr);

	return form_v_copy;
}


bool el_is_in(G_El* target_el, FormV* form_v) {
	while (form_v->getCurr() != nullptr) {
		V_El* curr_v_el = form_v->getCurr();
		FormG* form_g = curr_v_el->getForm();
		while (form_g->getCurr() != nullptr) {
			if (target_el->equals(form_g->getCurr())) {
				form_g->setCurr(form_g->getHead());
				form_v->setCurr(form_v->getHead());
				return true;
			}
			form_g->setCurr(form_g->getCurr()->getNext());
		}
		form_g->setCurr(form_g->getHead());
		form_v->setCurr(form_v->getCurr()->getNext());
	}
	form_v->setCurr(form_v->getHead());
	return false;
}


void handle_line(V_El* curr_v_el, FormV* form_v_2) {
	FormG* form_g_1 = curr_v_el->getForm();
	while (form_g_1->getCurr() != nullptr) {
		if (el_is_in(form_g_1->getCurr(), form_v_2)) {
			if (form_g_1->getCurr() == form_g_1->getHead()) {
				form_g_1->setHead(form_g_1->getCurr()->getNext());
				delete form_g_1->getCurr();
				form_g_1->setCurr(form_g_1->getHead());
				form_g_1->setPrev(nullptr);
			}
			else {
				form_g_1->getPrev()->setNext(form_g_1->getCurr()->getNext());
				delete form_g_1->getCurr();
				form_g_1->setCurr(form_g_1->getPrev()->getNext());
			}
		}
		else {
			form_g_1->setPrev(form_g_1->getCurr());
			form_g_1->setCurr(form_g_1->getCurr()->getNext());
		}
	}
	form_g_1->setCurr(form_g_1->getHead());
	form_g_1->setPrev(nullptr);
}


void handle_text(FormV* form_v_1, FormV* form_v_2) {
	while (form_v_1->getCurr() != nullptr) {
		handle_line(form_v_1->getCurr(), form_v_2);

		if (form_v_1->getCurr()->getForm()->getHead() == nullptr) {
			if (form_v_1->getCurr() == form_v_1->getHead()) {
				form_v_1->setHead(form_v_1->getCurr()->getNext());
				delete form_v_1->getCurr();
				form_v_1->setCurr(form_v_1->getHead());
				form_v_1->setPrev(nullptr);
			}
			else {
				form_v_1->getPrev()->setNext(form_v_1->getCurr()->getNext());
				delete form_v_1->getCurr();
				form_v_1->setCurr(form_v_1->getPrev()->getNext());
			}
		}
		else {
			form_v_1->setPrev(form_v_1->getCurr());
			form_v_1->setCurr(form_v_1->getCurr()->getNext());
		}
	}

	form_v_1->setCurr(form_v_1->getHead());
	form_v_1->setPrev(nullptr);
}


FormV* subtract(FormV* divided, FormV* dividor) {
	FormV* result = copy(divided);
	handle_text(result, dividor);
	return result;
}

	
bool belongs(FormV* small_set, FormV* large_set) {
	FormV* diff = subtract(small_set, large_set);
	if (diff->getHead() == nullptr) {
		return true;
	}
	return false;
}


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

