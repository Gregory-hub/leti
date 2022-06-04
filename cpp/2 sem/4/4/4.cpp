#include "functions.h"


int main()
{
	All* all = new All;
	all->read("in1.txt", "in2.txt", "in3.txt");
	FormV* result = subtract(all->list1, all->list2);
	out("out.txt", all, result, false);
	return 0;
}

