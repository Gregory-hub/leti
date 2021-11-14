#include <iostream>

using namespace std;

struct LongNum {
    int len;
    int arr[500];
};


int* func() {
    LongNum a;
    a.len = 100;
    a.arr[0] = 1;

    int* arr_pointer = a.arr;
    return a.arr;
};

int main(int argc, char const *argv[])
{
    int* longnum = func();
    cout << longnum << endl;
    return 0;
}

