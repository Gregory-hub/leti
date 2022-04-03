#include "functions.h"


using namespace std;


int main(int argc, char const *argv[]) {
    Text text;
    text.readFromFile("../in.txt");

    for (int i = 0; i < text.getLen(); i++) {
        text.printWord(i);
    }
    cout << endl;

    replace_words(text);

    for (int i = 0; i < text.getLen(); i++) {
        text.printWord(i);
    }
    cout << endl;

    return 0;
}

