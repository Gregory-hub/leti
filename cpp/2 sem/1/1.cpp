#include <iostream>
#include <fstream>
#include <sstream>
#include <iomanip>


using namespace std;


const int SIZE = 80;

struct Word {
    static const int sz = SIZE;
    char letters[sz + 1];
    char marker = 0;

    Word() {
        for (int i = 0; i <= sz; i++) {
            letters[i] = '.';
        }
        letters[sz] = marker;
    }
};


int count_lines(string filename) {
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


int read_word_with_len(string filename, Word& word, int line_index = 0) {
    int number_of_lines = count_lines(filename);
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
    // read len
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

    // read marker
    getline(ss, x, ' ');
    if (x.length() != 1) {
        cerr << "Error: invalid marker" << endl;
        exit(1);
    }
    word.marker = x.c_str()[0];

    // read word
    getline(ss, x);
    if (x.length() < len) {
        for (int i = 0; i < x.length(); i++) {
            word.letters[i] = x[i];
        }
        len = x.length();
    } else {
        for (int i = 0; i < len; i++) {
            word.letters[i] = x[i];
        }
    }
    word.letters[len] = word.marker;

    file.close();
    return len;
}


char read_word_with_sep(string filename, Word& word, int line_index = 0) {
    int number_of_lines = count_lines(filename);
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
    return sep;
}


void read_substrs(string filename, Word subs[][2], int len, char marker) {
    fstream file;
    file.open(filename, ios::in);
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    string line;
    for (int i = 0; i < len; i++) {
        int els_count = get_number_of_els_in_line(filename, i);
        if(els_count < 2) {
            cerr << "Too little substrings in line " << i + 1 << endl;
            exit(1);
        }

        Word sub1;
        Word sub2;
        sub1.marker = marker;
        sub2.marker = marker;
        sub1.letters[sub1.sz] = marker;
        sub2.letters[sub2.sz] = marker;

        getline(file, line);
        int k = 0;
        while (line[k] != ' ') {
            sub1.letters[k] = line[k];
            k++;
        }
        sub1.letters[k] = marker;
        int l = k + 1;

        while (line[l] && line[l] != ' ') {
            sub2.letters[l - k - 1] = line[l];
            l++;
        }
        sub2.letters[l - k - 1] = marker;

        subs[i][0] = sub1;
        subs[i][1] = sub2;
    }

    file.close();
}


int find_substr(const Word &word, const Word &sub, int word_len, int sub_len, int search_start = 0) {
    if(sub_len == 0) return -1;
    bool is_equal = true;
    for(int i = search_start; i <= word_len - sub_len; i++) {
        is_equal = true;
        for(int j = 0; j < sub_len; j++) {
            if(sub.letters[j] != word.letters[i + j]) {
                is_equal = false;
                break;
            }
        }
        if(is_equal) {
            return i;
        }
    }
    return -1;
}


void replace(Word &word, const Word &removed, const Word &inserted) {
    int word_len = 0;
    int removed_len = 0;
    int inserted_len = 0;
    while (word.letters[word_len] != word.marker) {
        word_len++;
    }
    while (removed.letters[removed_len] != removed.marker) {
        removed_len++;
    }
    while (inserted.letters[inserted_len] != inserted.marker) {
        inserted_len++;
    }

    int i = find_substr(word, removed, word_len, removed_len);
    while(i != -1) {
        int shift = inserted_len - removed_len;
        if(word_len + shift > word.sz) {
            cerr << "Error: word length after replacement is bigger than max size" << endl;
            exit(1);
        }
        if(shift > 0) {
            for(int k = word_len - 1; k >= i; k--) {
                word.letters[k + shift] = word.letters[k];
            }
        } else if(shift < 0) {
            for(int k = i; k < word_len + shift; k++) {
                word.letters[k] = word.letters[k - shift];
            }
        }
        for (int k = i; k < i + inserted_len; k++) {
            word.letters[k] = inserted.letters[k - i];
        }
        word.letters[word_len + shift] = word.marker;
        word_len += shift;
        i = find_substr(word, removed, word_len, removed_len, i + inserted_len);
    }
}


void out_init_sep(string filename, Word word, char sep, bool app = false) {
    fstream file;
    if (app) {
        file.open(filename, ios::app);
    } else {
        file.open(filename, ios::out);
    }
    
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    if (!app) {
        file << "INPUT" << endl;
        cout << "INPUT" << endl;
    } else {
        file << endl;
        cout << endl;
    }

    file << "Separator: " << sep << endl;
    file << "Marker: " << word.marker << endl;
    cout << "Separator: " << sep << endl;
    cout << "Marker: " << word.marker << endl;
    int last_marker;
    for (int i = 0; i < word.sz; i++) {
        if (word.letters[i] == word.marker) {
            last_marker = i;
        }
    }
    file << "Word: ";
    cout << "Word: ";
    for (int i = 0; i < last_marker; i++) {
        file << word.letters[i];
        cout << word.letters[i];
    }
    file << endl;
    cout << endl;
    file.close();
}


void out_init_len(string filename, Word word, int len, bool app = false) {
    fstream file;
    if (app) {
        file.open(filename, ios::app);
    } else {
        file.open(filename, ios::out);
    }
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    if (!app) {
        file << "INPUT" << endl;
        cout << "INPUT" << endl;
    } else {
        file << endl;
        cout << endl;
    }

    file << "Length: " << len << endl;
    file << "Marker: " << word.marker << endl;
    file << "Word: ";
    cout << "Length: " << len << endl;
    cout << "Marker: " << word.marker << endl;
    cout << "Word: ";
    for (int i = 0; i < len; i++) {
        file << word.letters[i];
        cout << word.letters[i];
    }
    file << endl;
    cout << endl;
    file.close();
}


void out_res(string filename, Word word, bool app_res = false) {
    fstream file;
    file.open(filename, ios::app);
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    if (!app_res) {
        file << endl << "OUTPUT" << endl;
        cout << endl << "OUTPUT" << endl;
    } else {
        file << endl;
        cout << endl;
    }

    file << "Word: ";
    cout << "Word: ";
    int i = 0;
    while (word.letters[i] != word.marker) {
        file << word.letters[i];
        cout << word.letters[i];
        i++;
    }
    file << endl;
    cout << endl;
    file.close();
}


int main(int argc, char const *argv[]) {
    cout << "Author: Novikov G. \n"
            "Group: 1302 \n"
            "Start date: 14.02.2022 \n"
            "End date: 15.02.2022 \n"
            "Version 1.1.1\n" << endl;

    Word word;

    char sep = read_word_with_sep("in.txt", word);
    out_init_sep("out.txt", word, sep);

    // int len = read_word_with_len("in.txt", word);
    // out_init_len("out.txt", word, len);

    int subs_count = count_lines("substrings.txt");
    Word subs[subs_count][2];

    read_substrs("substrings.txt", subs, subs_count, word.marker);
    for (int i = 0; i < subs_count; i++) {
        replace(word, subs[i][0], subs[i][1]);
    }

    out_res("out.txt", word);

    return 0;
}
