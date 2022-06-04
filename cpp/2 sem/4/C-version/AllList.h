#pragma once
#include"FormB.h"
#include"numberse.h"
struct AllList {
	FormB T1, T2, T3;
};
void Input(fstream& f, fstream& f1, fstream& f2, AllList* p);
void Output(fstream& f, AllList p);
void Delete(AllList *p);
//void lengthOut(FormB T);
int search(AllList T,int& ch1, int& fst, int& lst);
int searchStr(FormB T, FormS s1, int& ch1, int& st);
int change(AllList& T, int& ch1, int& fst, int& lst);
int obrabotka(AllList& T);
int set_numbers(AllList T, numbers* headn);
FormB cut(FormB T, int n, int k);
FormB cutLast(FormB T, int n);
int puchBack(FormB& T, FormB c);