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


int count_els(string arrayname) {
    fstream file;
    file.open("arrays/" + arrayname + ".txt", ios::in);
    if (file.fail()) {
        cerr << "Error opening file('arrays/" << arrayname << ".txt')" << endl;
        exit(1);
    }

    string line;
    int array_size = 0;
    while (!file.eof()) {
        getline(file, line);
        if (line.length() != 0) {
            array_size++;
        }
    }
    return array_size;
}


int* read_array(string arrayname, int array_size) {
    fstream file;
    file.open("arrays/" + arrayname + ".txt", ios::in);
    if (file.fail()) {
        cerr << "Error opening file('arrays/" << arrayname << ".txt')" << endl;
        exit(1);
    }

    int* array = new int[array_size];

    string el;
    for (int i = 0; i < array_size; i++) {
        if (!file.eof()) {
            getline(file, el);
            if (isnumber(el)) {
                *(array + i) = atoi(el.c_str());
            } else {
                cerr << "Array " + arrayname + ": file contains non-digit character" << endl;
                exit(1);
            }
        } else {
            cerr << "Array " + arrayname + ": wrong size of array(" << to_string(array_size) << ")" << endl;
            exit(1);
        }
    }

    file.close();

    return array;
}


void out(int* array, string arrayname, int arraysize, bool clear_file = false) {
    fstream file;
    if (clear_file) {
        file.open("out.txt", ios::out);
        if (file.fail()) {
            cerr << "Error opening file" << endl;
            exit(1);
        }
    } else {
        file.open("out.txt", ios::app);
        file << endl;
        if (file.fail()) {
            cerr << "Error opening file" << endl;
            exit(1);
        }
    }

    file << arrayname << ":" << endl;
    for(int i = 0; i < arraysize; i++) {
        file << *(array + i) << endl;
    }

    file.close();
}


int main(int argc, char const *argv[])
{
    string test_version = "1";
    int F_count = count_els("F" + test_version);
    int G_count = count_els("G" + test_version);
    int H_count = count_els("H" + test_version);
    int* F = read_array("F" + test_version, F_count);
    int* G = read_array("G" + test_version, G_count);
    int* H = read_array("H" + test_version, H_count);

    int Q_count = F_count + G_count + H_count;
    int* Q = new int[Q_count];

    for(int i = 0; i < F_count; i++) {
        *(Q + i) = F[i];
    }

    for(int i = 0; i < G_count; i++) {
        *(Q + F_count + i) = G[i];
    }

    for(int i = 0; i < H_count; i++) {
        *(Q + F_count + G_count + i) = H[i];
    }

    out(F, "F", F_count, true);
    out(G, "G", G_count);
    out(H, "H", H_count);
    out(Q, "Q", Q_count);

    delete[] F, G, H, Q;

    return 0;
}
