// Автор: Новиков Г.В.
// Группа: 1302
// Дата начала: 25.10.2021
// Дата окончания: 25.10.2021
// Версия: 3.1.01

#include <iostream>
#include <iomanip>
#include <fstream>

using namespace std;


void print_line(double n, double u, double sum) {
    ofstream file;
    file.open("out.txt", ios::app);

    cout << setw(9) << n;
    cout << "|";
    cout << setw(23) << u;
    cout << "|";
    cout << setw(23) << sum << endl;

    file << setw(9) << n;
    file << "|";
    file << setw(23) << u;
    file << "|";
    file << setw(23) << sum << endl;

    file.close();
}


double absolute(double num) {
    if (num < 0) {
        num = -num;
    }
    return num;
}


double sqrt(double num) {
    double res = num / 2;
    for (int i = 0; i < 25; i++) {
        res = (res + num / res);
    };
    return res;
}


double next_u(double u, int n, double x) {
    return u * (-(x * x * (2 * n + 1)) / (2 * (n + 1) * (2 * n + 3)));
};


int main(int argc, char const *argv[]) 
{
	cout << "Author: Novikov G. \n"
	"Group: 1302 \n"
	"Start date: 25.10.2021 \n"
	"End date: 25.10.2021 \n"
	"Version: 3.1.01 \n"
	"Formulation: ((1/(sqrt(2*PI))*(-1)^n*x^(2n+1))/(2^n*n!*(2*n + 1))" << endl << endl;
    double eps, x;
    bool eps_is_valid = false;
    cout << "Enter x: ";
    cin >> x;
    for (int i = 0; i < 3; i++) {
        cout << "Enter eps(between 0 and 1e-20): ";
        cin >> eps;
        if (eps > 0 && eps <= 1e-20) {
            eps_is_valid = true;
            break;
        };
        cout << "Invalid value(tries left: " << 2 - i << ")" << endl;
    };
    if (!eps_is_valid) {
        cout << "No tries left" << endl;
        return 0;
    };

    ofstream file;
    file.open("out.txt", ios::trunc);
    
    for (int i = 0; i < 57; i++) {
        cout << "_";
    }
    cout << endl;

    cout << setw(5) << "n";
    cout << setw(5) << "|";
    cout << setw(13) << "u(n)";
    cout << setw(11) << "|";
    cout << setw(13) << "sum";
    cout << setw(10) << endl;

    for (int i = 0; i < 57; i++) {
        file << "_";
    }
    file << endl;

    file << setw(5) << "n";
    file << setw(5) << "|";
    file << setw(13) << "u(n)";
    file << setw(11) << "|";
    file << setw(13) << "sum";
    file << setw(10) << endl;

    file.close();

    double PI = 3.14159265358;
    bool success = false;
    int final_n;
    int n = 0;
    double u = 1 / sqrt(2 * PI);
    double sum = u;

    for (int n = 1; n <= 1000; n++) {
        u = next_u(u, n, x);
        sum += u;
        print_line(n, u, sum);
        if (absolute(u) < eps) {
            success = true;
            final_n = n;
            break;
        }
    }
    file.open("out.txt", ios::app);
    if (success) {
        cout << "First EPS-locality occurence is attained at n = " << final_n << endl;
        file << "First EPS-locality occurence is attained at n = " << final_n << endl;
    } else {
        cout << "Didn`t find EPS-locality occurencies" << endl;
        file << "Didn`t find EPS-locality occurencies" << endl;
    }
    file.close();

    return 0;
}
