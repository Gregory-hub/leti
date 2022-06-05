#pragma once
#include "All.h"


bool belongs(FormV* small_set, FormV* large_set);
FormV* subtract(FormV* form_v_1, FormV* form_v_2);
bool el_is_in(G_El* target_el, FormV* form_v);
void out(string filename, All* all, FormV* diff, bool result, bool app);
void print_g_el(fstream& file, G_El* g_el);
void print_v_el(fstream& file, V_El* v_el, int num);
void print_list(fstream& file, FormV* list);
void desintegrate(FormV* form_v);

