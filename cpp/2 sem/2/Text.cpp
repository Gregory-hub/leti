#include "Text.h"

using namespace std;

Word Text::getWord(int index) {
    if (index >= len) {
        cerr << "Index is bigger than length of text" << endl;
        exit(0);
    }
    return words[index];
}

bool Text::setWord(int index, Word word) {
    if (index > len) return false;

    words[index] = word;
    if (index == len) len++;

    return true;
}

bool Text::setLen(int l) {
    len = l;
    return true;
}

int Text::getLen(){
    return len;
}

bool Text::is_sep_symbol(char sym) {
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

bool Text::readFromFile(string filename) {
    fstream file;
    file.open(filename, ios::in);
    if (!file.is_open()) {
        perror("Error opening file");
        return false;
    }

    len = 0;
    string line;
    while (getline(file, line)) {
        int st = 0;
        while (line[st] && is_sep_symbol(line[st])) st++;
        int end = st + 1;

        while(line[st]) {
            while (line[st] && is_sep_symbol(line[st])) st++;
            end = st + 1;
            while (line[end] && !is_sep_symbol(line[end])) end++;
            if (line[end]) {
                Word word;
                word.setMarker('#');
                for (int j = st; j < end; j++) {
                    word.setLetter(j - st, line[j]);
                }
                word.setLetter(end, word.getMarker());
                words[len] = word;
                len++;
                cout << st << ' ' << end << ' ' << len << ' ' << getWord(len - 1).getLetter(0) << getWord(len - 1).getLetter(1) << endl;
            }
            st = end + 1;
            end += 2;
        }
    }

    file.close();
    return true;
}
