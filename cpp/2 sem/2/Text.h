#include "Word.h"


using namespace std;


class Text {
    public:
        Word getWord(int index);
        void setWord(int index, Word word);
        int getLen();
        void setLen(int length);
        void readFromFile(string filename);
        void out(string filename, string message, bool append = false);
        void printWord(int index, fstream &file);

    private:
        Word words[1000];
        int len = 0;
        bool is_sep_symbol(char sym);
};

