#include "functions.h"


int main()
{
	All* all = new All;
	all->read("in1.txt", "in2.txt", "in3.txt");
	FormV* diff = subtract(all->list1, all->list2);
	bool result = belongs(all->list3, diff);
	out("out.txt", all, diff, result, false);
	desintegrate(all->list1);
	desintegrate(all->list2);
	desintegrate(all->list3);
	desintegrate(diff);
	delete all;

	return 0;
}

