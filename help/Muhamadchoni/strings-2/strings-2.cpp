#include <iostream>
#include <fstream>
#include <string>
#include <sstream>


using namespace std;


bool is_sep_symbol(char sym) {
    if (sym >= 32 && sym <= 47) {
        return true;
    }
    if (sym >= 58 && sym <= 64) {
        return true;
    }
    if (sym >= 91 && sym <= 96) {
        return true;
    }
    if (sym >= 123 && sym <= 126) {
        return true;
    }
    return false;
}


int read_s2(string s2[30], string filename) {
    fstream file;
    file.open(filename, ios::in);
    if (!file.is_open()) {
        perror("File error 1");
        exit(1);
    }

    string text;
    getline(file, text);
    stringstream ss(text);
    string word;
    int i = 0;
    while (getline(ss, word, ',')) {
        if (word[0] == ' ') {
            s2[i] = word.substr(1);
        } else {
            s2[i] = word;
        }
        i++;
    }
    if (i % 2 != 0) {
        cout << "Err 1" << endl;
        exit(1);
    }

    file.close();
    return i;   // len
}


int main(int argc, char const *argv[]) {
    string s2[30];
    int len = read_s2(s2, "s2.txt");
    fstream file;
    file.open("s1.txt", ios::in);
    if (!file.is_open()) {
        perror("File error 1");
        exit(1);
    }

    size_t start = 0;
    string text;
    getline(file, text);
    for (int i = 1; i <= len / 2; i++) {
        string from = s2[i * 2 - 2];
        string to = s2[i * 2 - 1];
        int search_start = 0;
        while (text.find(from, search_start) != string::npos) {
            start = text.find(from, search_start);
            if ((start == 0 || is_sep_symbol(text[start - 1])) && (start + from.length() >= text.length() || is_sep_symbol(text[start + from.length()]))) {
                text.replace(start, from.length(), to);
            } else {
                search_start = start + 1;
            }
        }
    }

    file.close();

    file.open("out.txt", ios::out);
    if (!file.is_open()) {
        perror("File error 2");
        exit(1);
    }

    file << text << endl;
    cout << text << endl;

    file.close();
    return 0;
}
