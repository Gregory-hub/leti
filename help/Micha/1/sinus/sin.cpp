#include <iostream>
#include <iomanip>
#include <fstream>
#include <math.h>

using namespace std;


long double sinus(long double x, long double eps) {
    long double sin = 0;
    int n = 0;
    long double a = x;
    while (abs(a) > eps) {
        sin += a;
        a *= -x * x / ((2 * n + 2) * (2 * n + 3));
        n++;
    }

    return sin;
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

    long double eps = 1e-12;
    string line;
    getline(ifile, line);
    int n = atoi(line.c_str());
    double nums[n];
    for (int i = 0; i < n; i++) {
        getline(ifile, line);
        nums[i] = atof(line.c_str());
        ofile << setprecision(15) << sinus(nums[i], eps) << endl;
    }

    ifile.close();
    ofile.close();
    return 0;
}
