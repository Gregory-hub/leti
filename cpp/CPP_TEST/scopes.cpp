#include <iostream>

using namespace std;


void fillarr(int* arr, int size) {
    for (int i = 0; i < size; i++) {
        arr[i] = i;
    }
}


void printarr(int* arr, int &size) {
    for (int i = 0; i < size; i++) {
        cout << arr[i] << endl;
    }
    size = 192;
}


int main(int argc, char const *argv[])
{
    int size = 9;
    int arr[size];
    fillarr(arr, size);
    printarr(arr, size);
    cout << size << endl;
    return 0;
}
