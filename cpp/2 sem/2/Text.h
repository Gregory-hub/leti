#include "Line.h"


class Text {
    public:
        Line getLine(int index);
        void setLine(int index, Line line);
        int getLen();
        void setLen(int length);
        void out(string filename, string message, bool append = 0);
        void printLine(int index, fstream& file);
        void readFromFile(string filename);
        bool is_sep_symbol(char sym);

    private:
        Line lines[100];
        int len = 0;
};

