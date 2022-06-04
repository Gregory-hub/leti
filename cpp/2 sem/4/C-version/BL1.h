#pragma once
#include"FormS.h"
struct BL1 {
	FormS S;
	BL1* next;
	void init();
};
bool is_Empty(BL1* head);
void DeleteHeadBL1(BL1** head);
