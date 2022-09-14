#include "functions.h"
#include "read.h"
#include "out.h"

// max_line_len from console		DONE
// read from file					DONE
// read from console
// set current line(index of line)	DONE
// exit								DONE
// line								DONE
// print							DONE
// delete							DONE
// insert
// replace
// apply
// write to file


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
	//out("out.txt", form_v, false);

	string command = "";
	string line = "";
	int line_index = 0;
	bool success = true;
	while (true) {
		success = true;
		getline(cin, line);
		stringstream ss(line);
		getline(ss, command, ' ');

		if (command == "exit") {
			// exit							- exit
			break;
		}
		else if (command == "line") {
			// line							- print current line number
			// line <int>					- go to line
			set_line_index(ss, line_index);
		}
		else if (command == "print") {
			// print <int>					- print given number of lines
			print(ss, form_v, line_index);
		}
		else if (command == "del") {
			// del down...				- in lines starting with current				1
			// del up...				- in lines finishing with current				1
			// del...after <string>...	- after subtext		|-------------->|			2
			// del...before <string>...	- before subtext	|-------------->|			2
			// del...subline "<string>"	- subline							|			2
			// del...prefix <int>		- first N symbols of line	|------>|			2
			// del...postfix <int>		- last N symbols of line	|------>|			2
			// del...<int> symbols...	- number of symbols	|<--------------|			3
			del(ss, form_v, line_index);
		}
		cout << ">>";
	}

	return 0;
}

