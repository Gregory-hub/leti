#pragma once
#include "All.h"


FormV* subtract(FormV* form_v_1, FormV* form_v_2);
bool el_is_in(G_El* target_el, FormV* form_v);
void out(string filename, All* all, FormV* result, bool app);
void out(string filename, All* all, bool app);
void out(string filename, FormV* result, bool app);
void print_g_el(fstream& file, G_El* g_el);
void print_v_el(fstream& file, V_El* v_el, int num);
void print_list(fstream& file, FormV* list);

