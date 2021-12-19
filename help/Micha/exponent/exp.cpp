#include <iostream>
#include <iomanip>
#include <fstream>
#include <math.h>

using namespace std;


long double expo(long double x, long double eps) {
    long double exp = 0;
    long double a = 1;
    for (int n = 1; abs(a) >= eps; n++) {
        exp += a;
        a *= x / n;
    }

    return exp;
}


int main(int argc, char const *argv[])
{
    fstream ifile;
    ifile.open("input.txt", ios::in);
    if (!ifile.is_open()) {
        perror("Cannot open file");
        return 1;
    }

    fstream ofile;
    ofile.open("output.txt", ios::out);
    if (!ofile.is_open()) {
        perror("Cannot open file");
        return 1;
    }

    long double eps = 1e-323;
    string line;
    getline(ifile, line);
    int n = atoi(line.c_str());
    long double nums[n];
    for (int i = 0; i < n; i++) {
        getline(ifile, line);
        nums[i] = stof(line);
        ofile << setprecision(15) << expo(nums[i], eps) << endl;
    }

    ifile.close();
    ofile.close();
    return 0;
}
