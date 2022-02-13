#include <iostream>

using namespace std;


struct Word {
    char* letters;
    char marker;
};


int main(int argc, char const *argv[])
{
    int word_sz = 10;
    Word word1;
    word1.letters = new char[word_sz + 1];
    char temp[word_sz] = {'B', 'i', 'b', 'a'};
    for (int i = 0; i <= word_sz; i++) {
        word1.letters[i] = temp[i];
    }
    for (int i = 0; i <= word_sz; i++) {
        cout << word1.letters[i] << endl;
    }
    return 0;
}
