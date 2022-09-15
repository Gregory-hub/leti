#pragma once
#include "FormV.h"


//bool belongs(FormV* small_set, FormV* large_set);
//FormV* subtract(FormV* form_v_1, FormV* form_v_2);
//bool el_is_in(G_El* target_el, FormV* form_v);
void out(string filename, FormV* form_v, bool app);
void desintegrate(FormV* form_v);
void set_line_index(stringstream& ss, int& line_index);
void print(stringstream& ss, FormV* form_v, int line_index);
void print_g_el(G_El* g_el);
void print_v_el(V_El* v_el);
void del_sequence(FormG* form_g, int i, int len);
void del(stringstream& ss, FormV* form_v, int line_index);
void ins(stringstream& ss, FormV* form_v, int line_index);

