#include <iostream>
#include <fstream>
#include <sstream>
#include <string>

using namespace std;

int main(int argc, char const *argv[])
{
    // read
    fstream file;
    file.open("in2.txt", ios::in);
    if (!file.is_open()) {
        perror("Cannot open file");
        exit(1);
    }

    int n = 0;
    string line;
    while (getline(file, line)) {
        if (!line.empty()) {
            n++;
        }
    }

    cout << n << endl;

    file.clear();
    file.seekg(0);

    int i = 0;
    int arr[n][n];
    string el;
    while (getline(file, line) && !line.empty()) {
        if (i > n) {
            cerr << "Number of rows error" << endl;
            exit(1);
        }
        int j = 0;
        stringstream ss(line);
        while(getline(ss, el, ' ')) {
            if (j > n) {
                cerr << "Error: number of cols does not match number of rows(row " << i << endl;
                exit(1);
            }
            arr[i][j] = atoi(el.c_str());
            j++;
        }

        i++;
    }

    // for (auto &row: arr) {
    //     for (auto &x: row) {
    //         cout << x << ' ';
    //     }
    //     cout << endl;
    // }

    // find sum
    int sum = 0;
    for (int i = 0; i < n; i++) {
        sum += arr[0][i];
    }
    for (int i = 1; i < n; i++) {
        sum += arr[i][0];
    }
    for (int i = 1; i < n; i++) {
        sum += arr[i][n - 1];
    }
    for (int i = 1; i < n - 1; i++) {
        sum += arr[n - 1][i];
    }

    cout << endl << sum << endl;

    return 0;
}
