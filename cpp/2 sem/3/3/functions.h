#pragma once
#include "Elem.h"

Elem* in(string filename);
Str create_str(char* word);

void out(fstream& file, Elem* first);
void printel(fstream& file, Elem* el, int num);

Elem* insert_before_last(Elem* first, Elem* first_inserted);

void desintegrate(Elem*& first);

