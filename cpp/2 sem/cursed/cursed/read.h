#pragma once
#include "FormV.h"


FormV* read(string filename, unsigned int max_line_len);
FormG* create_formG(char* line, unsigned int max_line_len);
G_El* create_G_El(char* line, unsigned int i, unsigned int str_len);
Str* create_str(char* line, unsigned int i, unsigned int str_len);

