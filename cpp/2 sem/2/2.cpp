#include "Text.h"
// #include <iostream>

using namespace std;


int main(int argc, char const *argv[]) {
    Text text;
    text.readFromFile("in.txt");
    cout << text.getLen() << endl;
    cout << text.getWord(0).getLetter(0);
    cout << text.getWord(0).getLetter(1);
    cout << text.getWord(0).getLetter(2);
    cout << text.getWord(0).getLetter(3);
    cout << text.getWord(0).getLetter(4) << endl;
    cout << text.getWord(1).getLetter(1) << endl;
    
    // char* arr = new char;
    // arr[1] = 'a';
    // cout << arr[1] << endl;

    // char a = ' ';
    // if (a) {
    //     cout << true << endl;
    // } else {
    //     cout << false << endl;
    // }

    return 0;
}
