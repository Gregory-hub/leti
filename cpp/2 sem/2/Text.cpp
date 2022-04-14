#include "Text.h"


using namespace std;


Line Text::getLine(int index) {
    if (index >= len) {
        cerr << "Text getLine: Index is bigger than length of text" << endl;
        exit(1);
    }
    return lines[index];
};

void Text::setLine(int index, Line line) {
    if (index > len) {
        cerr << "Text setLine: Index is bigger than len" << endl;
        exit(1);
    }

    for (int i = 0; i < line.getLen(); i++) {
        lines[index].setWord(i, line.getWord(i));
    }
    if (index == len) len++;
};

void Text::setLen(int length) {
    len = length;
};

int Text::getLen() {
    return len;
};

void Text::printLine(int index, fstream& file) {
    Line line = getLine(index);
    for (int i = 0; i < line.getLen(); i++) {
        line.printWord(i, file);
        if (i != line.getLen() - 1) {
            cout << ' ';
            file << ' ' << endl;
        }
    }
	cout << endl;
	file << endl;
};

void Text::out(string filename, string message, bool append) {
    fstream file;
    if (append) {
        file.open(filename, ios::app);
    }
    else {
        file.open(filename, ios::out);
    }
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    cout << message;
    file << message;

    for (int i = 0; i < getLen(); i++) {
        printLine(i, file);
    }
    cout << endl;
    file << endl;

    file.close();
};

void Text::readFromFile(string filename) {
    fstream file;
    file.open(filename, ios::in);
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    int word_count = 0;
    len = 0;
    string input;
    while (getline(file, input)) {
        Line line;
        int st = 0;
        while (st < input.length() && is_sep_symbol(input[st])) st++;
        int end = st + 1;
        while (end < input.length() && !is_sep_symbol(input[end])) end++;

        int line_len = 0;
        while (st < input.length()) {
            word_count++;
            Word word;
            word.setMarker('#');

            for (int i = st; i < end; i++) {
                word.setLetter(i - st, input[i]);
            }
            word.setLetter(end - st, word.getMarker());
            word.setNumber(word_count);
            line.setWord(line_len, word);
            line_len++;

            st = end;
            while (st < input.length() && is_sep_symbol(input[st])) st++;
            end = st + 1;
            while (end < input.length() && !is_sep_symbol(input[end])) end++;
        }
        setLine(len, line);
    }

    file.close();
};

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
};

