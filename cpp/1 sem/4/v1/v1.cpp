// Автор: Новиков Г.В.
// Группа: 1302
// Дата начала: 11.11.2021
// Дата окончания: 12.11.2021
// Версия: 4.1.01

#include <iostream>
#include <fstream>

using namespace std;


bool isnumber(string str) {
    for (char c: str) {
        if (!isdigit(c) and c != '-') return false;
    }
    return true;
}


void out(int* array, string arrayname, int arraysize, bool clear_file = false) {
    fstream file;
    if (clear_file) {
        file.open("out.txt", ios::out);
        if (!file.is_open()) {
            cerr << "Error opening file" << endl;
            exit(1);
        }
    } else {
        file.open("out.txt", ios::app);
        file << endl;
        if (!file.is_open()) {
            cerr << "Error opening file" << endl;
            exit(1);
        }
    }

    file << arrayname << ":" << endl;
    for(int i = 0; i < arraysize; i++) {
        file << array[i] << endl;
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

    string test_version = "2";
    int F_size = 3;
    int G_size = 5;
    int H_size = 1;

    int Q_size;
    if (F_size > 0) Q_size += F_size;
    if (G_size > 0) Q_size += G_size;
    if (H_size > 0) Q_size += H_size;
    int Q[Q_size];


    // F
    // open file
    fstream file;
    file.open("arrays/F" + test_version + ".txt", ios::in);
    if (!file.is_open()) {
        cerr << "Error opening file('arrays/F" << test_version << ".txt')" << endl;
        exit(1);
    }

    // read size
    string line;
    getline(file, line);
    if (!isnumber(line)) {
        cerr << "Array F: file contains non-digit character" << endl;
        return 1;
    }

    if (F_size != atoi(line.c_str())) {
        cerr << "Array F: invalid data(first line and number of elements do not match)" << endl;
        return 1;
    }

    if (F_size < 0) {
        F_size = 0;
    }

    // read array
    int F[F_size];
    int i = 0;
    while (getline(file, line)) {
        if (i > F_size) {
            cerr << "Array F: invalid data(first line and number of elements do not match)" << endl;
            return 1;
        }
        if (!isnumber(line)) {
            cerr << "Array F: file contains non-digit character" << endl;
            return 1;
        }
        F[i] = atoi(line.c_str());
        i++;
    }

    file.close();


    // G
    // open file
    file.open("arrays/G" + test_version + ".txt", ios::in);
    if (!file.is_open()) {
        cerr << "Error opening file('arrays/G" << test_version << ".txt')" << endl;
        exit(1);
    }

    // read size
    getline(file, line);
    if (!isnumber(line)) {
        cerr << "Array G: file contains non-digit character" << endl;
        return 1;
    }

    if (G_size != atoi(line.c_str())) {
        cerr << "Array G: invalid data(first line and number of elements do not match)" << endl;
        return 1;
    }

    if (G_size < 0) {
        G_size = 0;
    }

    // read array
    int G[G_size];
    i = 0;
    while (getline(file, line)) {
        if (i > G_size) {
            cerr << "Array G: invalid data(first line and number of elements do not match)" << endl;
            return 1;
        }
        if (!isnumber(line)) {
            cerr << "Array G: file contains non-digit character" << endl;
            return 1;
        }
        G[i] = atoi(line.c_str());
        i++;
    }

    file.close();


    // H
    // open file
    file.open("arrays/H" + test_version + ".txt", ios::in);
    if (!file.is_open()) {
        cerr << "Error opening file('arrays/H" << test_version << ".txt')" << endl;
        exit(1);
    }

    // read size
    getline(file, line);
    if (!isnumber(line)) {
        cerr << "Array H: file contains non-digit character" << endl;
        return 1;
    }

    if (H_size != atoi(line.c_str())) {
        cerr << "Array H: invalid data(first line and number of elements do not match)" << endl;
        return 1;
    }

    if (H_size < 0) {
        H_size = 0;
    }

    // read array
    int H[H_size];
    i = 0;
    while (getline(file, line)) {
        if (i > H_size) {
            cerr << "Array H: invalid data(first line and number of elements do not match)" << endl;
            return 1;
        }
        if (!isnumber(line)) {
            cerr << "Array H: file contains non-digit character" << endl;
            return 1;
        }
        H[i] = atoi(line.c_str());
        i++;
    }

    file.close();


    // Q
    for(int i = 0; i < F_size; i++) {
        Q[i] = F[i];
    }

    for(int i = 0; i < G_size; i++) {
        Q[F_size + i] = G[i];
    }

    for(int i = 0; i < H_size; i++) {
        Q[F_size + G_size + i] = H[i];
    }

    int Q_set_size = 0;
    int Q_set[Q_size];

    for(int i = 0; i < Q_size; i++) {
        bool add = true;
        for(int j = 0; j < Q_set_size; j++) {
            if (Q[i] == Q_set[j]) {
                add = false;
                break;
            }
        }

        if (add) {
            Q_set[Q_set_size] = Q[i];
            Q_set_size++;
        }
    }

    int Q_res_size = Q_set_size;
    int* Q_res = new int[Q_res_size];
    for (int i = 0; i < Q_res_size; i++) {
        Q_res[i] = Q_set[i];
    }

    out(F, "F", F_size, true);
    out(G, "G", G_size);
    out(H, "H", H_size);
    out(Q_res, "Q", Q_res_size);

    return 0;
}
