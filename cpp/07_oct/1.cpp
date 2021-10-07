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


int main(int argc, char const *argv[])
{
    while (true) {
        // input
        double coords[2];
        double a;
        cout << "Enter a: ";
        cin >> a;
        if (a <= 0) {
            cout << "ERROR: a must be > 0" << endl;
            break;
        };
        cout << "Enter x: ";
        cin >> coords[0];
        cout << "Enter y: ";
        cin >> coords[1];

        // output
        coords[0] = fmod(coords[0], 4 * a);
        cout << belongs_base_area(coords, a) << endl;
    };

    return 0;
}
