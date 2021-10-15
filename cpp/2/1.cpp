// Автор: Новиков Г.В.
// Группа: 1302
// Дата начала: 7.10.2021
// Дата окончания: 
// Версия: 2.1.01

#include <iostream>
#include <cmath>

using namespace std;


bool belongs_base_area(double coords[2], double a) {
    // looks if point belongs area 
    // input x must be in [0; 4a]
    double x = coords[0];
    double y = coords[1];
    if (x < 0 || x > 4 * a) {
        cout << "ERROR: x = " << x << " does not belong [0; " << 4 * a << "]" << endl;
        return false;
    };

    if (x < a) {
        if (y >= 0 && (pow(x, 2) + pow(y, 2) <= pow(a, 2))) {
            return true;
        };
    } else if (x < 2 * a) {
        if (y <= 0 && y >= -1 * a) {
            return true;
        };
    } else if (x < 3 * a) {
        if (y <= 0 && pow(x - 2 * a, 2) + pow(y, 2) <= pow(a, 2)) {
            return true;
        };
    } else if (x < 4 * a) {
        if (y >= 0 && y <= 1 * a) {
            return true;
        };
    };
    return false;
};


bool belongs_area(double coords[2], double a) {
    // get rid of period
    double t = abs(4 * a);
    double x = fmod(coords[0], t);
    double y = fmod(coords[1], t);
    if (a < 0) {
        x = -x;
        y = -y;
        a = -a;
    };
    if (x < 0) {
        x += t;
    };
    coords[0] = x;
    coords[1] = y;
    // if belongs area on [0; 4]
    return belongs_base_area(coords, a);
};


int main(int argc, char const *argv[])
{
	cout << "Автор: Новиков Г.В.\n"
	"Группа: 1302\n"
	"Дата начала: 7.10.2021 \n"
	"Версия: 2.1.01" << endl;
    while (true) {
        // input
        double coords[2];
        double a;
        cout << "Enter a: ";
        cin >> a;
        cout << "Enter x: ";
        cin >> coords[0];
        cout << "Enter y: ";
        cin >> coords[1];

        // output
        cout << belongs_area(coords, a) << endl;
    };

    return 0;
}
