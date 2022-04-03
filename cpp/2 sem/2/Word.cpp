#include "Word.h"

using namespace std;

char Word::getMarker(){
    return marker;
};

char Word::getLetter(int index){
    return letters[index];
};

void Word::setLetter(int index, char letter){
    letters[index] = letter;
};

void Word::setMarker(char mark){
    marker = mark;
};

bool Word::equals(Word word) {
    int i = 0;
    while (getLetter(i) != getMarker()) {
        if (getLetter(i) != word.getLetter(i)) return false;
        i++;
    }
    if (word.getLetter(i) == word.getMarker()) return true;
    return false;
}

