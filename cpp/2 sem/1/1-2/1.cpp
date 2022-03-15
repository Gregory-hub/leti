#include <iostream>
#include <fstream>
#include <sstream>


using namespace std;


class Word {
    public:

    static const int sz = 80;
    char def = '.';

    Word() {
        for (int i = 0; i < sz; i++) {
            letters[i] = def;
        }
    }

    int getLen() {
        return len;
    }

    void setLen(int l) {
        len = l;
    }

    char getLetter(int i) {
        return letters[i];
    }

    void setLetter(int i, char letter) {
        if (0 <= i && i < sz) {
            letters[i] = letter;
        }
    }

    void read_substrs(string filename, Word subs[][2], int len) {
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

            getline(file, line);
            int k = 0;
            while (line[k] != ' ') {
                sub1.letters[k] = line[k];
                k++;
            }
            sub1.setLen(k);
            int l = k + 1;

            while (line[l] && line[l] != ' ') {
                sub2.letters[l - k - 1] = line[l];
                l++;
            }
            sub2.setLen(l - k - 1);

            subs[i][0] = sub1;
            subs[i][1] = sub2;
        }

        file.close();
    }

    void replace(Word &word, Word &removed, Word &inserted) {
        int i = word.find_substr(word, removed);
        while (i != -1) {
            int shift = inserted.getLen() - removed.getLen();
            if(word.getLen() + shift > word.sz) {
                cerr << "Error: length after replacement is bigger than max size" << endl;
                exit(1);
            }

            if (shift > 0) {
                for (int k = word.getLen() - 1; k >= i; k--) {
                    word.setLetter(k + shift, word.getLetter(k));
                }
            } else if(shift < 0) {
                for (int k = i; k < word.getLen() + shift; k++) {
                    word.setLetter(k, word.getLetter(k - shift));
                }
                for (int k = word.getLen() + shift; k < word.getLen(); k++) {
                    word.setLetter(k, word.def);
                }
            }

            for (int k = i; k < i + inserted.getLen(); k++) {
                word.setLetter(k, inserted.getLetter(k - i));
            }
            word.setLen(word.getLen() + shift);
            i = word.find_substr(word, removed, i + inserted.getLen());
        }
    }

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
        word.setLen(len);

        // read word
        getline(ss, x);
        if (x.length() < len) {
            for (int i = 0; i < x.length(); i++) {
                word.setLetter(i, x[i]);
            }
            len = x.length();
            word.setLen(len);
        } else {
            for (int i = 0; i < len; i++) {
                word.setLetter(i, x[i]);
            }
        }

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

        // find start and end separators
        int start;
        int end;
        i = 2;
        while (i < line.length() && line[i] != sep) i++;
        start = i;

        i++;
        while (i < line.length() && line[i] != sep) i++;
        end = i;
        int len = end - start - 1;
        if (len < 0) len = 0;
        word.setLen(len);

        // read word from start + 1 to end - 1
        if (start < line.length() - 1) {
            for (i = start + 1; i < end; i++) {
                word.letters[i - start - 1] = line[i];
            }
        }

        file.close();
        return sep;
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
        cout << "Separator: " << sep << endl;
        file << "Length: " << word.getLen() << endl;
        cout << "Length: " << word.getLen() << endl;
        file << "Word: ";
        cout << "Word: ";
        for (int i = 0; i < word.getLen(); i++) {
            file << word.getLetter(i);
            cout << word.getLetter(i);
        }
        file << endl;
        cout << endl;
        file.close();
    }

    void out_init_len(string filename, Word word, bool app = false) {
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

        file << "Length: " << word.getLen() << endl;
        file << "Word: ";
        cout << "Length: " << word.getLen() << endl;
        cout << "Word: ";
        for (int i = 0; i < word.getLen(); i++) {
            file << word.getLetter(i);
            cout << word.getLetter(i);
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
        for (int i = 0; i < word.getLen(); i++) {
            file << word.getLetter(i);
            cout << word.getLetter(i);
        }
        file << endl;
        cout << endl;
        file.close();
    }

    private:

    char letters[sz];
    int len = 0;

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

    int find_substr(Word word, Word sub, int search_start = 0) {
        if(sub.getLen() == 0) return -1;
        bool is_equal = true;
        for(int i = search_start; i <= word.getLen() - sub.getLen(); i++) {
            is_equal = true;
            for(int j = 0; j < sub.getLen(); j++) {
                if(sub.getLetter(j) != getLetter(i + j)) {
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
};


int main(int argc, char const *argv[]) {
    cout << "Author: Novikov G. \n"
            "Group: 1302 \n"
            "Start date: 14.02.2022 \n"
            "End date: 15.02.2022 \n"
            "Version 1.1.1\n" << endl;

    Word word;

    // s or l
    char mode = 's';

    if (mode == 's') {
        char sep = word.read_word_with_sep("in.txt", word);
        word.out_init_sep("out.txt", word, sep);
    } else if (mode == 'l') {
        int len = word.read_word_with_len("in.txt", word);
        word.out_init_len("out.txt", word);
    } else exit(1);

    int subs_count = word.count_lines("substrings.txt");
    Word subs[subs_count][2];

    word.read_substrs("substrings.txt", subs, subs_count);
    for (int i = 0; i < subs_count; i++) {
        word.replace(word, subs[i][0], subs[i][1]);
    }

    word.out_res("out.txt", word);

    return 0;
}
