#include <iostream>
#include <fstream>
#include <sstream>

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


int get_number_of_els_in_line(string filename, int line_index = 0) {
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

    int count = 0;
    stringstream ss(line);
    string x;
    while (getline(ss, x, ' ')) count++;

    file.close();
    return count;
}


void read_word_with_len(string filename, Word& word, int line_index = 0) {
    int number_of_lines = get_number_of_lines(filename);
    if (line_index >= number_of_lines) {
        cerr << "Error: trying to read non-existing or empty line" << endl;
    }

    int num_of_els = get_number_of_els_in_line(filename, line_index);
    if (num_of_els < 2) {
        cerr << "Error: invalid line" << endl;
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

    stringstream ss(line);
    string x;
    getline(ss, x, ' ');
    // check if len consists of digits
    for (char d : x) {
        bool is_digit = false;
        for (char i = '0'; i <= '9'; i++) {
            if (d == i) {
                is_digit = true;
                break;
            }
        }
        if (!is_digit) {
            cerr << "Error: invalid word length" << endl;
            exit(1);
        }
    }
    int len = stoi(x);

    getline(ss, x, ' ');
    if (x.length() != 1) {
        cerr << "Error: invalid marker" << endl;
        exit(1);
    }
    word.marker = x.c_str()[0];

    getline(ss, x, ' ');
    if (x.length() < len) {
        for (int i = 0; i < x.length(); i++) {
            word.letters[i] = x[i];
        }
    } else {
        for (int i = 0; i < len; i++) {
            word.letters[i] = x[i];
        }
    }
    word.letters[len] = word.marker;

    file.close();
}


void read_word_with_sep(string filename, Word& word, int line_index = 0) {
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


int exchange(Word &word, string removed, string inserted) {
    int len = removed.length();
    if (len != inserted.length()) return 0;
    cout << endl;
    cout << "len: " << len << endl;
    char seq[len];
    int exc_count = 0;
    int i = 0;
    for (int i = 0; i < len; i++) {
        if (word.letters[i] == word.marker) return 0;
    }
    while (word.letters[i + len - 1] != word.marker and i < word.sz - len) {
        for (int j = i; j < i + len; j++) {
            seq[j - i] = word.letters[j];
        }
        for (int i = 0; i < len; i++) cout << seq[i];
        cout << endl;
        bool equal = true;
        for (int i = 0; i < len; i++) {
            if (seq[i] != removed[i]) equal = false;
        }
        if (equal) {
            for (int j = i; j < i + len; j++) {
                word.letters[j] = inserted[j - i];
            }
            exc_count++;
        }
        i++;
    }
    return exc_count;
}


int main(int argc, char const *argv[])
{
    Word word1, word2;
    read_word_with_len("in.txt", word1, 0);
    read_word_with_sep("in.txt", word2, 1);
    cout << word1.marker << endl;
    for (char l : word1.letters) {
        cout << l;
    }
    cout << endl;
    cout << word2.marker << endl;
    for (char l : word2.letters) {
        cout << l;
    }
    cout << endl;

    exchange(word1, "our", "iji");
    cout << word1.marker << endl;
    for (char l : word1.letters) {
        cout << l;
    }
    
    return 0;
}
