#include "Line.h"


using namespace std;


Word Line::getWord(int index) {
	return words[index];
};

void Line::setWord(int index, Word word) {
    int k = 0;
    if (index > len) {
        cerr << "Line setWord: Index is bigger than len" << endl;
        exit(1);
    }

	int i = 0;
    while (word.getLetter(i) != word.getMarker()) {
		words[index].setLetter(i, word.getLetter(i));
        i++;
    }
    words[index].setMarker(word.getMarker());
    words[index].setLetter(i, word.getMarker());
    words[index].setNumber(word.getNumber());
    words[index].replaced = word.replaced;

	if (index == len) len++;
};

int Line::getLen() {
	return len;
};

void Line::setLen(char length) {
	len = length;
};

void Line::printWord(int index, fstream &file) {
    Word word = getWord(index);
    int i = 0;
    while (word.getLetter(i) != word.getMarker()) {
        cout << word.getLetter(i);
        file << word.getLetter(i);
        i++;
    }
};

