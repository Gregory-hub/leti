#include "functions.h"
#include "read.h"
#include "out.h"


int main()
{
	cout << "Author: Novikov G.V.\n"
		"Start date: 06.06.2022\n"
		"End date: 06.06.2022\n"
		"Version: cursed.01\n" << endl;

	cout << "Enter max line size: ";
	int LINE_SIZE = 0;
	cin >> LINE_SIZE;
	if (LINE_SIZE <= 0) {
		cout << "INVALID LINE SIZE" << endl;
		return 0;
	}
	
	string filename = "in.txt";
	FormV* form_v = read(filename, LINE_SIZE);

	return 0;
}

