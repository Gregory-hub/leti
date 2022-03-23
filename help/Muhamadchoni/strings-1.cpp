#include <iostream>
#include <fstream>
#include <string>
#include <sstream>


using namespace std;


int main(int argc, char const *argv[])
{
    fstream file;
    string in_filename = "in.txt";
    string out_filename = "out.txt";

    file.open(in_filename, ios::in);
    if (!file.is_open()) {
        perror("File error 1");
        exit(1);
    }

    string text;
    getline(file, text);
    stringstream ss(text);
    string sign;
    string num;
    int sum = 0;

    getline(ss, num, ' ');
    if (num.length() != 1 || !isdigit(num.c_str()[0])) {
        cout << "Err 1" << endl;
        exit(1);
    }
    sum += stoi(num);

    while (getline(ss, sign, ' ')) {
        if (sign.length() != 1 || !(sign == "+" || sign == "-")) {
            cout << "Err 2" << endl;
            exit(1);
        }

        if (!getline(ss, num, ' ')) {
            cout << "Err 3" << endl;
            exit(1);
        }
        if (num.length() != 1 || !isdigit(num.c_str()[0])) {
            cout << "Err 4" << endl;
            exit(1);
        }
        if (sign == "+") {
            sum += stoi(num);
        } else if (sign == "-") {
            sum -= stoi(num);
        }
    }


    file.close();

    file.open(out_filename, ios::out);
    if (!file.is_open()) {
        perror("File error 2");
        exit(1);
    }

    file << "Text: " << text << endl;
    file << "Sum: " << sum << endl;

    cout << "Text: " << text << endl;
    cout << "Sum: " << sum << endl;

    file.close();

    return 0;
}
