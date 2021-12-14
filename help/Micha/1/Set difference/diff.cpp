#include <iostream>
#include <fstream>
#include <sstream>
#include <string>

using namespace std;


int read_size(fstream &ifile) {
    string line;
    int size;
    getline(ifile, line);
    stringstream ss(line);
    if (!(ss >> size)) {
        cerr << "Invalid input" << endl;
        exit(1);
    }
    return size;
};


void read_array(fstream &file, int* arr) {
    string line;
    getline(file, line);
    int i = 0;
    while () {
        cout << i << endl;
        i++;
    }
};


int main(int argc, char const *argv[])
{
    fstream ifile;
    ifile.open("input.txt", ios::in);
    if (!ifile.is_open()) {
        perror("Cannot open file");
        return 1;
    }

    fstream ofile;
    ofile.open("input.txt", ios::in);
    if (!ofile.is_open()) {
        perror("Cannot open file");
        return 1;
    }

    int A_size = read_size(ifile);
    int A[A_size];
    read_array(ifile, A);

    cout << A[0] << ' ' << A[1] << endl;

    ifile.close();
    ofile.close();
    return 0;
}
