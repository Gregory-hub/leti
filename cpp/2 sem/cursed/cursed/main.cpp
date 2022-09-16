#include "functions.h"
#include "read.h"

// max_line_len from console		DONE
// read from file					DONE
// read from console				DONE
// set current line(index of line)	DONE
// exit								DONE
// line								DONE
// print							DONE
// delete							DONE
// insert							DONE
// replace							DONE
// write to file					DONE


int main()
{
	cout << "Author: Novikov G.V.\n"
		"Start date: 06.06.2022\n"
		"End date: 16.09.2022\n"
		"Version: cursed.01\n" << endl;

	clear_file(PROTOCOL_FILENAME);

	cout << "Enter max line size: ";
	int LINE_SIZE = 0;
	cin >> LINE_SIZE;
	if (LINE_SIZE <= 0) {
		cerr << "Error: invalid line size" << endl;
		return 0;
	}

	cout << "Enter input mode('f' for file, 'c' for console): ";
	string input_mode = "";
	cin >> input_mode;

	FormV* form_v;
	if (input_mode == "f") {
		string filename = "in.txt";
		form_v = read(filename, LINE_SIZE);
	}
	else if (input_mode == "c") {
		cout << "Enter number of lines: ";
		int n = -1;
		cin >> n;
		if (n < 0) {
			cerr << "Error: number of lines cannot be < 0" << endl;
			return 0;
		}
		form_v = read_from_console(n, LINE_SIZE);
		cout << ">>";
	}
	else {
		cerr << "Error: invalid input mode" << endl;
		return 0;
	}

	string protocol_str = "Max line size: " + to_string(LINE_SIZE) + "\n";
	protocol_str = protocol_str + "Input mode: " + input_mode + "\n\n";
	protocol_str = protocol_str + "INPUT";
	protocol(form_v, protocol_str, false);

	string command = "";
	string line = "";
	int line_index = 0;
	bool success = true;

	while (true) {
		success = true;
		getline(cin, line);
		stringstream ss(line);
		getline(ss, command, ' ');

		if (command != "") protocol(form_v, "Command '" + command + "'", true);
		if (command == "exit") {
			// exit							- exit
			protocol(nullptr, "Exit program", true);
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
		else if (command == "ins") {
			// ins...after <string>...	- after subtext									1
			// ins...before <string>...	- before subtext								1
			// ins...subline "<string>"	- subline										2
			ins(ss, form_v, line_index);
		}
		else if (command == "replace") {
			// replace <string> with <string>
			replace(ss, form_v, line_index);
		}
		else if (command == "write") {
			// write to <filename>
			write(ss, form_v);
		}
		else if (command != "") {
			cout << "Unknown command: " << command << endl;
			protocol(nullptr, "Unknown command\n", true);
		}
		cout << ">>";
	}

	return 0;
}

