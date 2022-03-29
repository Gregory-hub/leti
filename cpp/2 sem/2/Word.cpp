#include "Word.h"

using namespace std;

char Word::getMarker(){
    return marker;
};

char Word::getLetter(int index){
    return letters[index];
};

bool Word::setLetter(int index, char letter){
    letters[index] = letter;
    return true;
};

bool Word::setMarker(char mark){
    marker = mark;
    return true;
};
