#include <iostream>
#include <fstream>

using namespace std;


struct Word {
    static const int sz = 80;
    char letters[sz + 1];
    char marker;

    Word() {
        for (int i = 0; i <= sz; i++) {
            letters[i] = '0';
        }
    }
};


int get_number_of_lines(string filename) {
    fstream file;
    file.open(filename, ios::in);
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    string line;
    int number_of_lines = 0;
    while(getline(file, line)) number_of_lines++;

    file.close();
    return number_of_lines;
}


void read_word(string filename, Word& word, int line_index = 0) {
    int number_of_lines = get_number_of_lines(filename);
    if (line_index >= number_of_lines) {
        cerr << "Error: trying to read non-existing or empty line" << endl;
    }

    fstream file;
    file.open(filename, ios::in);
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }
    
    // find line
    string line;
    int i = 0;
    while (i <= line_index) {
        getline(file, line);
        i++;
    }

    // error if the marker is missing
    if (line.length() < 2) {
        cerr << "Invalid line" << endl;
        exit(1);
    }
    // read separator and marker
    char sep = line[0];
    word.marker = line[1];

    // find start and end separators
    int start;
    int end;
    i = 2;
    while (i < line.length() && line[i] != sep) i++;
    start = i;

    i++;
    while (i < line.length() && line[i] != sep) i++;
    end = i;

    // read word from start + 1 to end - 1
    if (start < line.length() - 1) {
        for (i = start + 1; i < end; i++) {
            word.letters[i - start - 1] = line[i];
        }
    }
    // terminating character
    word.letters[i - start - 1] = word.marker;

    file.close();
}


int main(int argc, char const *argv[])
{
    Word word;
    read_word("in.txt", word, 0);
    cout << word.marker << endl;
    for (char l : word.letters) {
        cout << l;
    }
    return 0;
}
