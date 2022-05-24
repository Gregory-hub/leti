#pragma once
#ifndef TextType1
#define TextType1
#include"strL.h"
#include<math.h>
using namespace std;
struct Elem {
	strL s;
	Elem* next;
};
void initList(Elem* head, strL st);
Elem* DeleteHead(Elem* head);
Elem* deleteList(Elem* head);
bool is_Empty(Elem* head);
Elem* change(Elem* head, Elem* lst1, Elem* lst2);
Elem* obrabotka(Elem* head);
void inp(Elem* head, fstream& f);
void out(Elem* head, fstream& f);
int minStrLen(Elem* head);
int maxStrLen(Elem* head);
#endif