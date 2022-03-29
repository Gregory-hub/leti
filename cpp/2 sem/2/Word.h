#include <iostream>
#include <fstream>
#include <string>
#include <sstream>


class Word {
    public:
        char getLetter(int index);
        char getMarker();
        bool setLetter(int index, char letter);
        bool setMarker(char mark);

    private:
        char* letters = new char;
        char marker;
};
