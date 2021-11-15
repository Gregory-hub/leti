// Автор: Новиков Г.В.
// Группа: 1302
// Дата начала: 11.11.2021
// Дата окончания: 12.11.2021
// Версия: 4.1.01

#include <iostream>
#include <fstream>
#include <vector>

using namespace std;


bool isnumber(string str) {
    for (char c: str) {
        if (!isdigit(c) and c != '-') return false;
    }
    return true;
}


vector<int> read_array(string arrayname) {
    fstream file;
    file.open("arrays/" + arrayname + ".txt", ios::in);
    if (!file.is_open()) {
        cerr << "Error opening file('arrays/" << arrayname << ".txt')" << endl;
        exit(1);
    }

    string line;
    getline(file, line);
    if (!isnumber(line)) {
        cerr << "Array " + arrayname + ": file contains non-digit character" << endl;
        exit(1);
    }
    int array_size = atoi(line.c_str());
    if (array_size < 0) {
        cerr << "Array " + arrayname + ": invalid array size('" << array_size << "')" << endl;
        exit(1);
    }

    if (array_size < 0) {
        array_size = 0;
    }

    vector<int> array;

    while (getline(file, line)) {
        if (!isnumber(line)) {
            cerr << "Array " + arrayname + ": file contains non-digit character" << endl;
            exit(1);
        }
        array.push_back(atoi(line.c_str()));
    }

    file.close();

    bool sizes_are_equal = array.size() == array_size;
    if (!sizes_are_equal) {
        cerr << "Array " + arrayname + ": invalid data(first line and number of elements do not match)" << endl;
        exit(1);
    }

    return array;
}


vector<int> concatenate_vecs(vector<int> F, vector<int> G, vector<int> H) {
    vector<int> Q = F;
    
    for(int x: G) {
        Q.push_back(x);
    }

    for(int x: H) {
        Q.push_back(x);
    }

    return Q;
}


void out(vector<int> F, vector<int> G, vector<int> H, vector<int> Q) {
    fstream file;
    file.open("out.txt", ios::out);
    if (!file.is_open()) {
        cerr << "Error opening file('out.txt')" << endl;
        exit(1);
    }

    file << "F:" << endl;
    for(int x: F) {
        file << x << endl;
    }
    
    file << endl << "G:" << endl;
    for(int x: G) {
        file << x << endl;
    }

    file << endl << "H:" << endl;
    for(int x: H) {
        file << x << endl;
    }

    file << endl << "Q:" << endl;
    for(int x: Q) {
        file << x << endl;
    }

    file.close();
}


int main(int argc, char const *argv[])
{
	cout << "Author: Novikov G. \n"
	"Group: 1302 \n"
	"Start date: 11.11.2021 \n"
	"End date: 12.11.2021 \n"
	"Version: 4.1.01 \n" << endl;

    string test_version = "4";
    vector<int> F = read_array("F" + test_version);
    vector<int> G = read_array("G" + test_version);
    vector<int> H = read_array("H" + test_version);

    vector<int> Q = concatenate_vecs(F, G, H);
    out(F, G, H, Q);

    return 0;
}