#include <iostream>

using namespace std;

int main(int argc, char const *argv[])
{
    int var = 190;
    int* ptr = &var;
    int arr[] = {12, 90, 8, 32, 101, 56};

    for (int x: arr) {
        cout << x << endl;
    };

    return 0;
}
