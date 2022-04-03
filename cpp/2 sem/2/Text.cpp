#include "Text.h"

using namespace std;

Word Text::getWord(int index) {
    if (index >= len) {
        cerr << "Index is bigger than length of text" << endl;
        exit(1);
    }
    return words[index];
}

void Text::setWord(int index, Word word) {
    if (index > len) {
        cerr << "Index is bigger than len" << endl;
        exit(1);
    }

    int i = 0;
    while (word.getLetter(i) != word.getMarker()) {
		words[index].setLetter(i, word.getLetter(i));
        i++;
    }
    words[index].setMarker(word.getMarker());
    words[index].setLetter(i, word.getMarker());
    words[index].replaced = word.replaced;
    if (index == len) len++;
}

void Text::setLen(int length) {
    len = length;
}

int Text::getLen(){
    return len;
}

void Text::printWord(int index) {
    Word word = getWord(index);
    int i = 0;
    cout << index << ' ';
    while (word.getLetter(i) != word.getMarker()) {
        cout << word.getLetter(i);
        i++;
    }
    cout << endl;
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

void Text::readFromFile(string filename) {
    fstream file;
    file.open(filename, ios::in);
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    len = 0;
    string line;
    while (getline(file, line)) {
        //cout << line.length() << endl;
        int st = 0;
        while (st < line.length() && is_sep_symbol(line[st])) st++;
        int end = st + 1;
        while (end < line.length() && !is_sep_symbol(line[end])) end++;

        while (st < line.length()) {
            Word word;
            word.setMarker('#');

            for (int i = st; i < end; i++) {
                word.setLetter(i - st, line[i]);
            }
            word.setLetter(end - st, word.getMarker());
            setWord(len, word);

            st = end;
			while (st < line.length() && is_sep_symbol(line[st])) st++;
			end = st + 1;
			while (end < line.length() && !is_sep_symbol(line[end])) end++;
        }
    }

    file.close();
}

