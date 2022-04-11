#include <iostream>
#include <fstream>
#include <string>
#include <sstream>


class Word {
    public:
        char getLetter(int index);
        char getMarker();
        void setLetter(int index, char letter);
        void setMarker(char mark);
        bool equals(Word word);
        bool replaced = false;

    private:
        char letters[100];
        char marker;
};
