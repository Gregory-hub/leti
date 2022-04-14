#include <iostream>
#include <fstream>
#include <string>
#include <sstream>


using namespace std;


class Word {
    public:
        char getLetter(int index);
        void setLetter(int index, char letter);
        char getMarker();
        void setMarker(char mark);
        int getNumber();
        void setNumber(int num);
        bool equals(Word word);
        bool replaced = false;

    private:
        char letters[100];
        char marker = '#';
        int number = -1;
};

