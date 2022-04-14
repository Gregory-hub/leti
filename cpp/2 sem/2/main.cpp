#include "functions.h"


using namespace std;


int main(int argc, char const *argv[]) {
    cout << "Author: Novikov G. \n"
            "Group: 1302 \n"
            "Start date: 1.04.2022 \n"
            "End date: 17.04.2022 \n"
            "Version 2.1.1\n" << endl;

    Text text;
    text.readFromFile("../in.txt");

    text.out("../out.txt", "INPUT\n");

    replace_words(text);

    text.out("../out.txt", "OUTPUT\n", true);

    return 0;
}

