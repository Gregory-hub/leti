#include "Word.h"


using namespace std;


class Text {
    public:
        Word getWord(int index);
        bool setWord(int index, Word word);
        int getLen();
        bool setLen(int l);
        bool readFromFile(string filename);
        
    private:
        Word* words = new Word;
        int len = 0;
        bool is_sep_symbol(char sym);
};
